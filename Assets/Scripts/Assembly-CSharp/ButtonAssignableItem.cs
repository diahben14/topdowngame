using UnityEngine;

public class ButtonAssignableItem : Item
{
	[Header("Button Assignable Item")]
	[SerializeField]
	protected bool suppressQuantity;

	[SerializeField]
	protected bool usageBroadcastsTelegraph;

	[SerializeField]
	protected float telegraphRange = 8f;

	[SerializeField]
	protected bool useMPUsesForQuantity;

	protected Dat usageCostDat;

	public bool Telegraphs
	{
		get
		{
			return usageBroadcastsTelegraph;
		}
	}

	public float TelegraphRange
	{
		get
		{
			return telegraphRange;
		}
	}

	public override ItemType Type
	{
		get
		{
			if (!suppressQuantity)
			{
				return ItemType.SUPPLIES;
			}
			return ItemType.EQUIPMENT;
		}
	}

	public override bool SuppressQuantity
	{
		get
		{
			if (useMPUsesForQuantity)
			{
				return false;
			}
			return suppressQuantity;
		}
	}

	public override string QuantityString
	{
		get
		{
			if (useMPUsesForQuantity)
			{
				return usesLeftAccordingToMP().ToString();
			}
			return base.QuantityString;
		}
	}

	protected int usesLeftAccordingToMP()
	{
		if (usageCostDat == null)
		{
			usageCostDat = new Dat("mpCost_" + base.name + "_mp2");
		}
		return Mathf.CeilToInt(PlayerCharacter.GetMP(2) / (float)usageCostDat.IntValue);
	}
}
