public class QuaffFlaskItemBehaviour : QuaffItemBehaviour
{
	public override bool ShouldUsageMakeItemRootVisible
	{
		get
		{
			return item.Quantity > 0;
		}
	}

	protected override bool okToUse()
	{
		if (item.Quantity > 0)
		{
			return PlayerCharacter.HP < PlayerCharacter.MaxHP;
		}
		return false;
	}

	protected override void doEffect()
	{
		PlayerCharacter.FlaskSwig();
	}
}
