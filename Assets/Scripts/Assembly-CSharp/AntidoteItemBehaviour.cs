using System.Collections;
using UnityEngine;

public class AntidoteItemBehaviour : SpecificItemBehaviour
{
	[SerializeField]
	protected ParticleSystem effect;

	protected static SpecificItemBehaviour rembemberQuaffBehaviour;

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem usedItem, int actionButtonUp, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		if (item.Quantity > 0 && PlayerCharacter.Poison > 0)
		{
			cachedAnimator.SetTrigger("quaff");
			rembemberQuaffBehaviour = this;
			yield return 0;
		}
	}

	public void __AnimationEvent__FireQuaffEffect()
	{
		item.Quantity--;
		effect.Play();
		GetComponent<Creature>().ClearPoison();
	}
}
