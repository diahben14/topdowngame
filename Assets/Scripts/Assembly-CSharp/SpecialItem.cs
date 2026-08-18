public class SpecialItem : Item
{
	public override bool ShouldShowInInventory
	{
		get
		{
			return false;
		}
	}
}
