using UnityEngine;

public class Pedestal : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	private StateVariable stateVar;

	[SerializeField]
	private Renderer insertedMedallionRenderer;

	public bool Interact(Item i)
	{
		if (!stateVar.BoolValue)
		{
			stateVar.BoolValue = true;
			insertedMedallionRenderer.enabled = true;
			return true;
		}
		return false;
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return true;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}

	private void Start()
	{
		insertedMedallionRenderer.enabled = stateVar.BoolValue;
	}
}
