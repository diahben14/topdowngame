public abstract class SpecificMagicItemBehaviour : SpecificItemBehaviour
{
	protected Dat[] usageCostDat = new Dat[3];

	protected override void itemBehaviour_Awake()
	{
	}

	protected virtual void specificMagicItemBehaviour_Awake()
	{
	}

	protected bool mpSaysOkToUse()
	{
		return false;
	}

	protected void applyMPCost()
	{
	}
}
