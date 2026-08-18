using UnityEngine;

public class ShopItemPickup : ItemPickup
{
	[Header("ShopItemPickup")]
	[SerializeField]
	protected Item currency;

	[SerializeField]
	protected LanguageLine confirmPurchaseFormattedLanguageLine;

	[SerializeField]
	protected LanguageLine notEnoughLangaugeLine;

	protected override bool pickupSequence()
	{
		return true;
	}

	protected void buy()
	{
		if (itemToGive.Value <= currency.Quantity)
		{
			currency.Quantity -= itemToGive.Value;
			getIt();
		}
		else
		{
			GenericMessage.ShowMessage(notEnoughLangaugeLine);
		}
	}
}
