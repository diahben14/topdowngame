using System.Collections;

public abstract class EdibleItemBehaviour : SpecificItemBehaviour
{
	protected static EdibleItemBehaviour itemInHand;

	public void __AnimationEvent__FireEatEffect()
	{
		if (itemInHand == this)
		{
			item.Quantity--;
			onEat();
			itemInHand = null;
		}
	}

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonUp, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		if (item.Quantity > 0)
		{
			cachedAnimator.SetTrigger("eat");
			itemInHand = this;
		}
		yield break;
	}

	protected abstract void onEat();
}
