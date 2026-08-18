using FMODUnity;
using UnityEngine;

public class ItemPickup : Pickup
{
	[Header("Item")]
	[SerializeField]
	protected Item itemToGive;

	[SerializeField]
	protected int quantityToGive = 1;

	[SerializeField]
	protected bool showPresentation;

	[SerializeField]
	protected bool showMoneyGoUp;

	[SerializeField]
	protected StateVariable pickupStateVar;

	[SerializeField]
	[EventRef]
	protected string damageEvent;

	protected override bool destroyOnPickup
	{
		get
		{
			return false;
		}
	}

	public int QuantityToGive
	{
		get
		{
			return quantityToGive;
		}
	}

	protected override bool alreadyPickedUp()
	{
		if ((bool)pickupStateVar)
		{
			return pickupStateVar.BoolValue;
		}
		return false;
	}

	protected override void onGetIt()
	{
		if ((bool)pickupStateVar)
		{
			pickupStateVar.BoolValue = true;
		}
		itemToGive.Quantity += quantityToGive;
		if (showPresentation)
		{
			ItemPresentation.PresentItem(itemToGive);
		}
		if (showMoneyGoUp)
		{
			MoneyGoUp.IncrementMoneyGoUp(quantityToGive);
		}
	}

	protected void OnCollisionEnter()
	{
		SFX.PlayAudioClipHere(damageEvent, base.transform.position);
	}
}
