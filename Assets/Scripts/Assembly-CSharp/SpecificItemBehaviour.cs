using UnityEngine;

public abstract class SpecificItemBehaviour : ItemBehaviour
{
	[SerializeField]
	protected ButtonAssignableItem item;

	public override bool IsItemBehaviourForItem(Item i)
	{
		return i == item;
	}
}
