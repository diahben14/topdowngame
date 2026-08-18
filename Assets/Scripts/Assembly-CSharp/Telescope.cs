using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

[SelectionBase]
public class Telescope : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	protected CameraPositionOverrideTrigger cameraOverrideTrigger;

	[SerializeField]
	protected bool DEMO_demoEndingTelescope;

	[SerializeField]
	protected float DEMO_whiteoutDelay;

	[SerializeField]
	protected float DEMO_whiteoutDuration;

	[SerializeField]
	protected Transform DEMO_magnetizeTransform;

	[SerializeField]
	[EventRef]
	private string sfx_use;

	[SerializeField]
	[EventRef]
	private string sfx_reset;

	protected CameraPositionOverrideTrigger camTrigger
	{
		get
		{
			if (cameraOverrideTrigger == null)
			{
				Debug.LogError("[Telescope] No camera trigger.");
			}
			return cameraOverrideTrigger;
		}
	}

	protected void OnTriggerExit(Collider c)
	{
		if ((bool)c.GetComponent<PlayerCharacter>() && camTrigger.enabled)
		{
			camTrigger.enabled = false;
			SFX.PlayAudioClipHere(sfx_reset, base.transform.position);
		}
	}

	public bool Interact(Item i)
	{
		if (i == null)
		{
			StartCoroutine(showTelescopeView());
			return true;
		}
		return false;
	}

	protected void Awake()
	{
		camTrigger.enabled = false;
	}

	protected IEnumerator showTelescopeView()
	{
		if (DEMO_demoEndingTelescope)
		{
			PlayerCharacter.AnimationGotoState("demoend");
			PlayerCharacter.MagnetizeToPositionAndFacing(DEMO_magnetizeTransform.position, DEMO_magnetizeTransform.rotation);
			camTrigger.enabled = true;
			GUIMode.ClearStack();
			yield return new WaitForSeconds(DEMO_whiteoutDelay);
			if (DEMO_whiteoutDuration > 0f)
			{
				FullscreenFader.Whiteout = true;
				for (float i = 0f; i < 1f; i += TimeManager.UnscaledDeltaTime / DEMO_whiteoutDuration)
				{
					CameraController.CutsceneFade = i;
					yield return 0;
				}
			}
			CameraController.CutsceneFade = 1f;
			Profile.Reset();
			SceneManager.LoadScene("TitleScreen");
		}
		else
		{
			SFX.PlayAudioClipHere(camTrigger.enabled ? sfx_reset : sfx_use, base.transform.position);
			camTrigger.enabled = !camTrigger.enabled;
			yield return 0;
		}
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return true;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}
}
