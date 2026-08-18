using UnityEngine;

public class WellSupplyItem : ButtonAssignableItem
{
	[Header("Supplies")]
	[SerializeField]
	protected SupplyCoinItem supplyCoinItem;

	[SerializeField]
	protected int capacityPerCoin;

	[SerializeField]
	protected Transform icon3D;

	public override ItemType Type
	{
		get
		{
			return ItemType.WELL_SUPPLIES;
		}
	}

	public Transform Icon3D
	{
		get
		{
			return icon3D;
		}
	}

	public override bool HasDenominator
	{
		get
		{
			return false;
		}
	}

	public override string DenominatorString
	{
		get
		{
			return MaxRefillCapacity.ToString();
		}
	}

	public int MaxRefillCapacity
	{
		get
		{
			return supplyCoinItem.DepositCount * capacityPerCoin;
		}
	}

	public override bool ShouldShowInInventory
	{
		get
		{
			if (Quantity <= 0)
			{
				return AvailableForPurchase;
			}
			return true;
		}
	}

	public bool AvailableForPurchase
	{
		get
		{
			if ((bool)supplyCoinItem)
			{
				return supplyCoinItem.DepositCount > 0;
			}
			Debug.LogWarning("WellSupplyItem probably needs SupplyCoinItem specified.");
			return true;
		}
	}

	public override void OnCampfireRest()
	{
	}
}
