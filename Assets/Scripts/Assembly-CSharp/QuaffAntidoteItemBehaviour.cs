public class QuaffAntidoteItemBehaviour : QuaffItemBehaviour
{
	protected override bool okToUse()
	{
		return PlayerCharacter.Poison > 0;
	}

	protected override void doEffect()
	{
		GetComponent<Creature>().ClearPoison();
	}
}
