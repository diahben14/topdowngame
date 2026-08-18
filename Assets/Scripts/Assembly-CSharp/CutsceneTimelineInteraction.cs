using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(InteractionTrigger))]
public class CutsceneTimelineInteraction : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	protected PlayableDirector timelineDirector;

	[SerializeField]
	protected Transform foxMagnetizeTo;

	[SerializeField]
	protected string foxAnimationName;

	protected bool hasBeenTriggered;

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return !hasBeenTriggered;
	}

	bool IInteractionReceiver.Interact(Item i)
	{
		hasBeenTriggered = true;
		if ((bool)foxMagnetizeTo)
		{
			PlayerCharacter.MagnetizeToPositionAndFacing(foxMagnetizeTo.position, foxMagnetizeTo.rotation);
		}
		if (foxAnimationName != "")
		{
			PlayerCharacter.AnimationGotoState(foxAnimationName);
		}
		timelineDirector.Play();
		return true;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return !hasBeenTriggered;
	}
}
