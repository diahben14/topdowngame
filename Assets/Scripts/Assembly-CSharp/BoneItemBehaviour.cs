using System.Collections;
using UnityEngine;

public class BoneItemBehaviour : SpecificItemBehaviour
{
	[SerializeField]
	protected LanguageLine confirmationPromptLine;

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem usedItem, int actionButton, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		GenericPrompt.ShowPrompt(confirmationPromptLine, confirmBoneUseCallback, null);
		yield break;
	}

	protected void confirmBoneUseCallback()
	{
		StartCoroutine(useBoneCoroutine());
	}

	protected IEnumerator useBoneCoroutine()
	{
		PlayerCharacter.AnimationSetTrigger("use bone");
		CameraController.AddScreenshake(1f);
		yield return 0;
		GUIMode.ClearStack();
		MusicManager.Stop();
		if (!CycleController.IsNight)
		{
			CycleController.AnimateSunset();
		}
		yield return new WaitForSeconds(2f);
		for (float f = 0f; f < 1f; f += TimeManager.UnscaledDeltaTime)
		{
			CameraController.CutsceneFade = f;
			yield return 0;
		}
		Campfire.RespawnAtLastCampfire();
	}
}
