using UnityEngine;

public class InteractToChangeStateVar : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	protected StateVariable stateVar;

	[Header("On Interact...")]
	[SerializeField]
	protected bool turnOnIfOff;

	[SerializeField]
	protected bool turnOffIfOn;

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

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}
}
