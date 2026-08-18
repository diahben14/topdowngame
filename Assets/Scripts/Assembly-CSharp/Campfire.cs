using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Campfire : PlayerCharacterSpawn, IInteractionReceiver
{
	[Header("Campfire")]
	[SerializeField]
	protected Transform spawnTransform;

	[SerializeField]
	protected GameObject checkpointUnlockedEffect;

	[SerializeField]
	protected LanguageLine deadMessage;

	protected bool suppressReSleepPrompt;

	protected const float kCampfireReuseCooldownDuration = 3f;

	public static bool SleepSequenceRunning { get; protected set; }

	private bool isUseableAccordingToConduitSystem
	{
		get
		{
			return true;
		}
	}

	private bool isSuppressed
	{
		get
		{
			if (suppressReSleepPrompt)
			{
				return Time.timeSinceLevelLoad < 3f;
			}
			return false;
		}
	}

	protected override Transform playerSpawnTransform
	{
		get
		{
			return spawnTransform;
		}
	}

	public static void RespawnAtLastCampfire()
	{
		PlayerCharacter.RebootCharacter();
		restEffects();
		PlayerCharacterSpawn.GoTo(Profile.GetCampfireScene(), Profile.GetCampfireID());
	}

	protected static void restEffects()
	{
		PlayerCharacter.Restore();
		Inventory.OnCampfireRest();
		Monster.ClearRuntimeDeadMonsters();
	}

	public bool Interact(Item i)
	{
		if (isUseableAccordingToConduitSystem)
		{
			restEffects();
			Profile.UnlockSpiritWorldSpawn(PlayerCharacterSpiritSpawn.GetSpiritWorldSpawnID(PlayerCharacterSpawn.GetCurrentSceneName(), id));
			setMostRecentCampfire();
			StartCoroutine(sleepSequence());
			return true;
		}
		GenericMessage.ShowMessage(deadMessage);
		return true;
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return !isSuppressed;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}

	protected IEnumerator sleepSequence()
	{
		SleepSequenceRunning = true;
		GUIMode.ClearStack();
		PlayerCharacter.EnterRestState(base.transform.position);
		float num = 1f;
		if (!CycleController.IsNight)
		{
			num = CycleController.AnimateSunset();
		}
		yield return new WaitForSeconds(num / 2f);
		for (float f = 0f; f < 1f; f += Time.deltaTime / transitionDuration)
		{
			PlayerCharacterSpawn.SceneTransitionFade = f;
			yield return 0;
		}
		PlayerCharacterSpawn.SceneTransitionFade = 1f;
		yield return 0;
		SleepSequenceRunning = false;
		PlayerCharacterSpawn.GoTo(Profile.GetCampfireScene(), Profile.GetCampfireID());
	}

	protected void setMostRecentCampfire()
	{
		Profile.SetCampfire(SceneManager.GetActiveScene().name, FullID);
	}

	protected override void onArrivalStart()
	{
		suppressReSleepPrompt = true;
		PlayerCharacter.EnterRestStateInstant(base.transform.position);
		PlayerCharacter.ExitRestState();
		if (!CycleController.IsNight)
		{
			CycleController.AnimateSunrise();
		}
	}

	protected void Awake()
	{
		if (Application.isPlaying)
		{
			updateUnlockGraphics();
		}
	}

	private void updateUnlockGraphics()
	{
		if (checkpointUnlockedEffect == null)
		{
			Debug.LogError("checkpoint unlocked effect is null");
		}
		else
		{
			checkpointUnlockedEffect.SetActive(isUseableAccordingToConduitSystem);
		}
	}
}
