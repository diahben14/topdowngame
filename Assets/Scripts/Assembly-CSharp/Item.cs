using UnityEngine;

public class Item : ScriptableObject
{
	public enum ItemType
	{
		EQUIPMENT = 0,
		SUPPLIES = 1,
		WELL_SUPPLIES = 2,
		FLASK = 3,
		MONEY = 4,
		GEAR = 5,
		OFFERINGS = 6
	}

	[Header("Basic Info")]
	[SerializeField]
	protected Sprite icon;

	[SerializeField]
	protected int baseValue;

	[SerializeField]
	protected LanguageLine collectionMessage;

	[SerializeField]
	public int Sort;

	protected bool _inventoryStringCached;

	protected string _cachedInventoryString;

	public Sprite Icon
	{
		get
		{
			return icon;
		}
	}

	public int Value
	{
		get
		{
			return baseValue;
		}
	}

	public virtual string QuantityString
	{
		get
		{
			if (SuppressQuantity)
			{
				return string.Empty;
			}
			return Quantity.ToString();
		}
	}

	public LanguageLine CollectionMessage
	{
		get
		{
			return collectionMessage;
		}
	}

	public virtual ItemType Type
	{
		get
		{
			return ItemType.GEAR;
		}
	}

	public virtual int Quantity
	{
		get
		{
			return Profile.GetInventoryQuantity(this);
		}
		set
		{
			int quantity = Quantity;
			Profile.SetInventoryQuantity(this, value);
			if (value > quantity)
			{
				OnCollect();
			}
		}
	}

	public virtual bool ShouldShowInInventory
	{
		get
		{
			return Quantity > 0;
		}
	}

	public virtual bool SuppressQuantity
	{
		get
		{
			return Quantity == 1;
		}
	}

	public virtual bool HasDenominator
	{
		get
		{
			return false;
		}
	}

	public virtual string DenominatorString
	{
		get
		{
			return "";
		}
	}

	public string CachedInventoryString
	{
		get
		{
			if (!_inventoryStringCached)
			{
				_cachedInventoryString = Profile.kInventoryQuantityPrefx + base.name;
				_inventoryStringCached = true;
			}
			return _cachedInventoryString;
		}
	}

	public virtual void OnCollect()
	{
	}

	public virtual void OnDeath()
	{
	}

	public virtual void OnRecoverBloodstain()
	{
	}

	public virtual void OnCampfireRest()
	{
	}
}
