using UnityEngine;

public class OfferingItem : Item
{
	[Header("Offering Item Behaviour")]
	[SerializeField]
	public Item upgradeItemReceived;

	[SerializeField]
	public LanguageLine statLabel;

	[SerializeField]
	public LanguageLine statLabel_short;

	[SerializeField]
	protected int basePrice;

	[SerializeField]
	protected float priceIncreasePerLevelup;

	public override ItemType Type
	{
		get
		{
			return ItemType.OFFERINGS;
		}
	}

	public override bool SuppressQuantity
	{
		get
		{
			return false;
		}
	}

	public override bool ShouldShowInInventory
	{
		get
		{
			return Quantity > 0;
		}
	}

	public int PriceForNext()
	{
		return basePrice + Mathf.RoundToInt(priceIncreasePerLevelup * (float)upgradeItemReceived.Quantity);
	}
}
