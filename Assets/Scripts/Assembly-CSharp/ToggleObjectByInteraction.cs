using UnityEngine;

public class ToggleObjectByInteraction : ToggleObjectByStateVariable, IInteractionReceiver
{
	[Header("Toggle Object By Interaction")]
	[SerializeField]
	private bool turnOnIfOff = true;

	[SerializeField]
	private bool turnOffIfOn;

	[SerializeField]
	private bool hidePrompt;

	bool IInteractionReceiver.CanReceiveInteration()
	{
		if (stateVar != null)
		{
			if (!turnOffIfOn || !stateVar.BoolValue)
			{
				if (turnOnIfOff)
				{
					return !stateVar.BoolValue;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	bool IInteractionReceiver.Interact(Item i)
	{
		if (stateVar.BoolValue && turnOffIfOn)
		{
			stateVar.BoolValue = false;
			return true;
		}
		if (!stateVar.BoolValue && turnOnIfOff)
		{
			stateVar.BoolValue = true;
			return true;
		}
		return false;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return !hidePrompt;
	}
}
