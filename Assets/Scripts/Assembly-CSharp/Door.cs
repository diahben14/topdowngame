using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StatefulActive))]
public class Door : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	protected Item keyItem;

	[SerializeField]
	protected Transform characterTransform;

	[SerializeField]
	protected StateVariable stateVariable;

	[SerializeField]
	protected LanguageLine useKeyPromptLanguageLine;

	[SerializeField]
	[EventRef]
	protected string openDoorSFX;

	[SerializeField]
	[EventRef]
	protected string jiggleDoorSFX;

	bool IInteractionReceiver.Interact(Item i)
	{
		if (i == keyItem)
		{
			if (!stateVariable.BoolValue)
			{
				open();
				return true;
			}
			Debug.LogWarning("Tried to open door that was already open");
		}
		else if (!stateVariable.BoolValue)
		{
			if (keyItem.Quantity > 0)
			{
				GenericPrompt.ShowPrompt(useKeyPromptLanguageLine, open, rattle);
			}
			else
			{
				rattle();
			}
			return false;
		}
		return false;
	}

	private void rattle()
	{
		SFX.PlayAudioClipHere(jiggleDoorSFX, base.transform.position);
		GetComponent<Animator>().SetTrigger("jiggle");
	}

	private void open()
	{
		keyItem.Quantity--;
		PlayerCharacter.MagnetizeToPositionAndFacing(characterTransform.position, characterTransform.rotation);
		SFX.PlayAudioClipHere(openDoorSFX, base.transform.position);
		stateVariable.BoolValue = true;
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return !stateVariable.BoolValue;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return !stateVariable.BoolValue;
	}
}
