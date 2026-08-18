using System.Collections;
using UnityEngine;

public class IntroHypersword : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	private Animator cameraWhiteoutAnimator;

	[SerializeField]
	private GameObject swordRoot;

	protected float smoothedZoom;

	protected float smoothedZoomVelocity;

	protected bool transitionStarted;

	private void Awake()
	{
		cameraWhiteoutAnimator.speed = 0f;
		GetComponent<Animator>().speed = 0f;
	}

	private void Update()
	{
		float magnitude = (PlayerCharacter.WorldPosition - base.transform.position).magnitude;
		if (!transitionStarted)
		{
			smoothedZoom = Mathf.SmoothDamp(smoothedZoom, Mathf.Lerp(12f, 0f, Mathf.InverseLerp(1f, 24f, magnitude)), ref smoothedZoomVelocity, 1f, float.PositiveInfinity, TimeManager.UnscaledDeltaTime);
			CameraController.CutsceneZoom = smoothedZoom;
		}
		else
		{
			TimeManager.CutsceneTimeScale = 0f;
		}
	}

	protected IEnumerator sequence()
	{
		PlayerCharacter.SetFacing(base.transform.position);
		PlayerCharacter.AnimationGotoState("character reach for sword");
		GetComponent<Animator>().speed = 1f;
		transitionStarted = true;
		cameraWhiteoutAnimator.speed = 1f;
		for (float f = 0f; f < 12f; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
		for (float f = 0f; f < 3f; f += TimeManager.UnscaledDeltaTime)
		{
			CameraController.CutsceneFade = f;
			yield return 0;
		}
		MusicManager.Stop();
		CameraController.CutsceneFade = 1f;
		TimeManager.CutsceneTimeScale = 1f;
		Campfire.RespawnAtLastCampfire();
	}

	bool IInteractionReceiver.Interact(Item i)
	{
		StartCoroutine(sequence());
		return true;
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return !transitionStarted;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}
}
