using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
public class ItemRecepticle : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	protected StateVariable boolToSetTrue;

	[SerializeField]
	protected Item itemNeeded;

	[SerializeField]
	protected bool consumeItemOnUse;

	[SerializeField]
	protected LanguageLine messageToShowOnInteractBeforeItemInserted;

	[SerializeField]
	protected LanguageLine promptMessageForUsingItem;

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return !boolToSetTrue.BoolValue;
	}

	bool IInteractionReceiver.Interact(Item i)
	{
		if (messageToShowOnInteractBeforeItemInserted != null && itemNeeded.Quantity == 0)
		{
			GenericMessage.ShowMessage(messageToShowOnInteractBeforeItemInserted);
		}
		else if (itemNeeded.Quantity > 0 && !boolToSetTrue.BoolValue)
		{
			if (promptMessageForUsingItem != null)
			{
				GenericPrompt.ShowPrompt(promptMessageForUsingItem, placeItem, null);
			}
			else
			{
				placeItem();
			}
			return true;
		}
		return false;
	}

	private void placeItem()
	{
		if (consumeItemOnUse)
		{
			itemNeeded.Quantity--;
		}
		boolToSetTrue.BoolValue = true;
		onPlaced();
	}

	protected virtual void onPlaced()
	{
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		if (messageToShowOnInteractBeforeItemInserted != null && !boolToSetTrue.BoolValue)
		{
			return true;
		}
		return false;
	}
}
