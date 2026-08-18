using System.Collections;
using UnityEngine;

public abstract class QuaffItemBehaviour : SpecificItemBehaviour
{
	protected static QuaffItemBehaviour rememberQuaffBehaviour;

	[SerializeField]
	protected ParticleSystem effect;

	protected bool quaffDone;

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem usedItem, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		if (item.Quantity > 0 && okToUse())
		{
			cachedAnimator.SetBool("quaff", true);
			rememberQuaffBehaviour = this;
			quaffDone = false;
			while (okToUse() && !animationWasInterrupted && !quaffDone)
			{
				yield return new WaitForFixedUpdate();
			}
			cachedAnimator.SetBool("quaff", false);
		}
	}

	public void __AnimationEvent__FireQuaffEffect()
	{
		if (rememberQuaffBehaviour == this)
		{
			item.Quantity--;
			effect.Play();
			doEffect();
			quaffDone = true;
		}
	}

	protected abstract bool okToUse();

	protected abstract void doEffect();
}
