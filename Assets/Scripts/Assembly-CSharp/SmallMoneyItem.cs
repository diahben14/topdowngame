using UnityEngine;

public class SmallMoneyItem : Item
{
	[SerializeField]
	private Item bigMoneyItem;

	[SerializeField]
	private Item moneyLevelItem;

	[SerializeField]
	private LanguageLine convertedSmallToBigMessage;

	protected static Dat _conversionThreshold;

	protected static SmallMoneyItem instance;

	public static int PlayerQuantity
	{
		get
		{
			return instance.Quantity;
		}
		protected set
		{
			instance.Quantity = value;
		}
	}

	public static int ConversionThreshold
	{
		get
		{
			if (_conversionThreshold == null)
			{
				_conversionThreshold = new Dat("smallCoinsPerBig");
			}
			return _conversionThreshold.IntValue;
		}
	}

	public override ItemType Type
	{
		get
		{
			return ItemType.MONEY;
		}
	}

	public int bloodstainQuantity
	{
		get
		{
			return Profile.GetBloodstainQuantity(this);
		}
		set
		{
			Profile.SetBloodstainQuantity(this, value);
		}
	}

	public static void DoMoneyCombine()
	{
		if (ConversionThreshold < 0)
		{
			Debug.LogErrorFormat("Money conversation threshold is {0}.", ConversionThreshold);
			return;
		}
		while (PlayerQuantity >= ConversionThreshold)
		{
			PlayerQuantity -= ConversionThreshold;
			instance.bigMoneyItem.Quantity++;
			instance.moneyLevelItem.Quantity++;
			MoneyCombineNotification.Show();
		}
	}

	protected void OnEnable()
	{
		instance = this;
	}

	public override void OnCollect()
	{
	}

	public override void OnDeath()
	{
		int num = (bloodstainQuantity = Quantity);
		Quantity -= num;
	}

	public override void OnRecoverBloodstain()
	{
		Quantity += bloodstainQuantity;
		bloodstainQuantity = 0;
	}
}
