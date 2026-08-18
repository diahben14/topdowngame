using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class PlayerCharacterDemoSpawn : PlayerCharacterSpawn
{
	[Serializable]
	protected class ItemQuantityPair
	{
		public Item item;

		public int quantity;
	}

	[Header("Demo Spawn")]
	[SerializeField]
	protected ItemQuantityPair[] itemMinimums;

	[SerializeField]
	protected StateVariable inventoryHasBeenSetUp;

	[SerializeField]
	protected ButtonAssignableItem[] buttonSlots;

	[SerializeField]
	protected StateVariable[] setToTrue;

	protected override void onArrivalStart()
	{
		Profile.SetCampfire(SceneManager.GetActiveScene().name, FullID);
		for (int i = 0; i < itemMinimums.Length; i++)
		{
			itemMinimums[i].item.Quantity = Mathf.Max(itemMinimums[i].item.Quantity, itemMinimums[i].quantity);
		}
		if (!inventoryHasBeenSetUp.BoolValue)
		{
			for (int j = 0; j < buttonSlots.Length; j++)
			{
				Inventory.AssignItemToButton(j, buttonSlots[j]);
			}
			inventoryHasBeenSetUp.BoolValue = true;
		}
		for (int k = 0; k < setToTrue.Length; k++)
		{
			setToTrue[k].BoolValue = true;
		}
	}
}
