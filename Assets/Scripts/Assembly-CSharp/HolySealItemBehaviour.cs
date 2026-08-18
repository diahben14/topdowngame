using System.Collections;

public class HolySealItemBehaviour : SpecificItemBehaviour
{
	protected override IEnumerator onActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		HolySealDoor best = ProximityRegistered<HolySealDoor>.GetBest();
		if (best != null)
		{
			item.Quantity--;
			best.Open();
		}
		yield return 0;
	}
}
