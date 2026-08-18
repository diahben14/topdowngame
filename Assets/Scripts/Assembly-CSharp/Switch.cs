using FMODUnity;
using UnityEngine;

public class Switch : ToggleObjectByStateVariable, IInteractionReceiver
{
	[SerializeField]
	protected Transform playerMagnetTransformRight;

	[SerializeField]
	protected Transform playerMagnetTransformLeft;

	[SerializeField]
	protected SwitchType switchType;

	[SerializeField]
	[EventRef]
	protected string throwSFX;

	public SwitchType Type
	{
		get
		{
			return switchType;
		}
	}

	public void Throw()
	{
		SFX.PlayAudioClipHere(throwSFX, base.transform.position);
		stateVar.BoolValue = true;
	}

	bool IInteractionReceiver.Interact(Item i)
	{
		if (!stateVar.BoolValue)
		{
			if (Vector3.Dot(base.transform.right, PlayerCharacter.Forward) > 0f)
			{
				PlayerCharacter.AttemptToActivateSwitch(this, playerMagnetTransformLeft);
			}
			else
			{
				PlayerCharacter.AttemptToActivateSwitch(this, playerMagnetTransformRight);
			}
			return true;
		}
		return false;
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		if (stateVar != null)
		{
			return !stateVar.BoolValue;
		}
		return false;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}
}
