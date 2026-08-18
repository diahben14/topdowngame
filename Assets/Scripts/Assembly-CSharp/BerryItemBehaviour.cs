using UnityEngine;

public class BerryItemBehaviour : EdibleItemBehaviour
{
	[SerializeField]
	protected int hpPerBerry = 4;

	protected override void onEat()
	{
		PlayerCharacter.GiveHP(hpPerBerry);
	}
}
