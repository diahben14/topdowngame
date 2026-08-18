using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
public class ShopItem : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	protected int price;

	[SerializeField]
	protected Item itemToGive;

	[SerializeField]
	protected int quantityToGive;

	[SerializeField]
	protected GameObject graphic;

	[SerializeField]
	protected StateVariable optionalBoughtStatevar;

	[HideInInspector]
	[SerializeField]
	protected Item currency;

	[HideInInspector]
	[SerializeField]
	protected LanguageLine confirmPurchaseFormattedLanguageLine;

	[HideInInspector]
	[SerializeField]
	protected LanguageLine notEnoughLangaugeLine;

	bool IInteractionReceiver.CanReceiveInteration()
	{
		if (!(optionalBoughtStatevar == null))
		{
			return !optionalBoughtStatevar.BoolValue;
		}
		return true;
	}

	bool IInteractionReceiver.Interact(Item i)
	{
		return true;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}

	protected void buy()
	{
		if (currency.Quantity >= price)
		{
			currency.Quantity -= price;
			itemToGive.Quantity += quantityToGive;
			ItemPresentation.PresentItem(itemToGive);
			if ((bool)optionalBoughtStatevar)
			{
				optionalBoughtStatevar.BoolValue = true;
			}
		}
		else
		{
			GenericMessage.ShowMessage(notEnoughLangaugeLine);
		}
	}

	private void onStateChange()
	{
		updateGraphic();
	}

	private void updateGraphic()
	{
		graphic.SetActive(optionalBoughtStatevar == null || !optionalBoughtStatevar.BoolValue);
	}

	private void OnEnable()
	{
		if ((bool)optionalBoughtStatevar)
		{
			optionalBoughtStatevar.SubscribeToChanges(onStateChange);
		}
		updateGraphic();
	}

	private void OnDisable()
	{
		if ((bool)optionalBoughtStatevar)
		{
			optionalBoughtStatevar.UnSubscribeToChanges(onStateChange);
		}
	}
}
