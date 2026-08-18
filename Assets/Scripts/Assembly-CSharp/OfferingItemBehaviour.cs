using System.Collections;
using UnityEngine;

public class OfferingItemBehaviour : ItemBehaviour
{
	[SerializeField]
	protected Item currencyItem;

	[SerializeField]
	protected Item hiddenMoneyLevelItem;

	[SerializeField]
	protected LanguageLine formattedPriceLine;

	[SerializeField]
	protected LanguageLine notEnoughMoneyLine;

	protected int price;

	protected OfferingItem offeringItem;

	public override bool IsItemBehaviourForItem(Item i)
	{
		return i is OfferingItem;
	}

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		yield break;
	}
}
