public class SupplyCoinItem : ButtonAssignableItem
{
	public int DepositCount
	{
		get
		{
			return Profile.GetSupplyCoinDepositCount(this);
		}
		set
		{
			Profile.SetSupplyCoinDepositCount(this, value);
		}
	}

	public void DepositInWell()
	{
		DepositCount++;
	}
}
