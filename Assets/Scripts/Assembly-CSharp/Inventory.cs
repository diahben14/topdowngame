using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
	public static readonly int kButtonCount = 4;

	public static readonly string kItemAssetPath = "Items";

	public static readonly string kButtonEquippedItemsString = "Item on Button ";

	private static List<Item> itemList = new List<Item>();

	private static ButtonAssignableItem[] buttonAssignedItems = new ButtonAssignableItem[kButtonCount];

	private static bool setupDone;

	public static void ReloadButtonAssignments()
	{
		for (int i = 0; i < kButtonCount; i++)
		{
			string itemString = Profile.GetString(kButtonEquippedItemsString + i);
			if (itemString != string.Empty)
			{
				Item item = itemList.Find((Item x) => x.name == itemString);
				if (item != null)
				{
					buttonAssignedItems[i] = item as ButtonAssignableItem;
					continue;
				}
				Debug.LogErrorFormat("During Inventory setup, save file says player has {0} but it can't be found.", itemString);
			}
		}
	}

	public static void OnDeath()
	{
		setupIfNeeded();
		foreach (Item item in itemList)
		{
			item.OnDeath();
		}
		Profile.Save();
	}

	public static Item GetItemByName(string s)
	{
		setupIfNeeded();
		for (int i = 0; i < itemList.Count; i++)
		{
			if (itemList[i].name == s)
			{
				return itemList[i];
			}
		}
		Debug.LogErrorFormat("[Inventory] Couldn't find item `{0}`", s);
		return null;
	}

	public static void OnCampfireRest()
	{
		setupIfNeeded();
		foreach (Item item in itemList)
		{
			item.OnCampfireRest();
		}
	}

	public static void Reset()
	{
		for (int i = 0; i < kButtonCount; i++)
		{
			buttonAssignedItems[i] = null;
		}
	}

	public static void RecoverSuppliesFromBloodstain()
	{
		setupIfNeeded();
		BloodstainRecoveryNotification.Show();
		foreach (Item item in itemList)
		{
			item.OnRecoverBloodstain();
		}
	}

	public static ButtonAssignableItem ButtonAssignableItemFromButtonNumber(int i)
	{
		setupIfNeeded();
		if (i < kButtonCount)
		{
			return buttonAssignedItems[i];
		}
		Debug.LogErrorFormat("No such thing as button {0}", i);
		return null;
	}

	public static void AssignItemToButton(int index, ButtonAssignableItem item)
	{
		setupIfNeeded();
		if (index >= 0 && index < kButtonCount)
		{
			for (int i = 0; i < kButtonCount; i++)
			{
				if (ButtonAssignableItemFromButtonNumber(i) == item)
				{
					Profile.SetString(kButtonEquippedItemsString + i, "");
					buttonAssignedItems[i] = null;
				}
			}
			buttonAssignedItems[index] = item;
			Profile.SetString(kButtonEquippedItemsString + index, item.name);
			Profile.Save();
		}
		else
		{
			Debug.LogErrorFormat("No such thing as button {0}", index);
		}
	}

	public static List<Item> GetHeldItemsOfType(Item.ItemType type)
	{
		setupIfNeeded();
		List<Item> list = new List<Item>();
		for (int i = 0; i < itemList.Count; i++)
		{
			Item item = itemList[i];
			if (item.Type == type && item.ShouldShowInInventory)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public static bool AnyWellSupplyItemsUnlocked()
	{
		setupIfNeeded();
		for (int i = 0; i < itemList.Count; i++)
		{
			if (itemList[i].Type == Item.ItemType.WELL_SUPPLIES && (itemList[i] as WellSupplyItem).AvailableForPurchase)
			{
				return true;
			}
		}
		return false;
	}

	private static void setupIfNeeded()
	{
		if (setupDone)
		{
			return;
		}
		Object[] array = Resources.LoadAll(kItemAssetPath);
		for (int i = 0; i < array.Length; i++)
		{
			Item item = array[i] as Item;
			if ((bool)item)
			{
				itemList.Add(item);
			}
		}
		itemList.Sort((Item a, Item b) => a.Sort - b.Sort);
		ReloadButtonAssignments();
		setupDone = true;
	}
}
