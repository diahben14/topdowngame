using UnityEngine;

public class FlaskItem : ButtonAssignableItem
{
	[SerializeField]
	protected Item flaskContainerItem;

	public override ItemType Type
	{
		get
		{
			return ItemType.FLASK;
		}
	}

	public override int Quantity
	{
		get
		{
			return flaskContainerItem.Quantity - Profile.GetInventoryQuantity(this);
		}
		set
		{
			Profile.SetInventoryQuantity(this, flaskContainerItem.Quantity - value);
		}
	}

	public override bool ShouldShowInInventory
	{
		get
		{
			return MaxCapacity > 0;
		}
	}

	public override bool HasDenominator
	{
		get
		{
			return true;
		}
	}

	public override string DenominatorString
	{
		get
		{
			return MaxCapacity.ToString();
		}
	}

	public int MaxCapacity
	{
		get
		{
			return flaskContainerItem.Quantity;
		}
	}

	public override void OnCampfireRest()
	{
		Quantity = flaskContainerItem.Quantity;
	}
}
