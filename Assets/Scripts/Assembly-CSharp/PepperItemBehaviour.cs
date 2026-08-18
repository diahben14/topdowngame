using UnityEngine;

public class PepperItemBehaviour : EdibleItemBehaviour
{
	[SerializeField]
	protected float damageBoostTime = 20f;

	protected override void onEat()
	{
		PlayerCharacter.AddDamageBoostTime(damageBoostTime);
	}
}
