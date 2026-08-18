using UnityEngine;

public class TempleDoor : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	protected StateVariable templeDoorOpen;

	[SerializeField]
	protected StateVariable visitedSpiritRealmForDoorVision;

	[SerializeField]
	protected StateVariable bell_east;

	[SerializeField]
	protected StateVariable bell_west;

	[SerializeField]
	protected LanguageLine ringBellsMessage;

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return !templeDoorOpen.BoolValue;
	}

	bool IInteractionReceiver.Interact(Item i)
	{
		if (!visitedSpiritRealmForDoorVision.BoolValue)
		{
			visitedSpiritRealmForDoorVision.BoolValue = true;
			PlayerCharacterSpawn.GoTo("Playable Intro", "start");
		}
		else if (!bell_east.BoolValue || !bell_west.BoolValue)
		{
			GenericMessage.ShowMessage(ringBellsMessage);
		}
		else
		{
			templeDoorOpen.BoolValue = true;
		}
		return true;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return !templeDoorOpen.BoolValue;
	}
}
