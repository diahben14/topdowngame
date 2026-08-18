using UnityEngine;

public class UpgradeStatue : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	protected UpgradeType upgradeType;

	[SerializeField]
	protected int price = 1;

	[SerializeField]
	protected StateVariable stateVariable;

	[HideInInspector]
	[SerializeField]
	protected CameraPositionOverrideTrigger approachCameraZone;

	[HideInInspector]
	[SerializeField]
	protected CameraPositionOverrideTrigger inMenuCameraZone;

	[HideInInspector]
	[SerializeField]
	protected GameObject kobanGraphicsRoot;

	[HideInInspector]
	[SerializeField]
	protected GameObject[] kobanGraphics;

	public UpgradeType UpgradeType
	{
		get
		{
			return upgradeType;
		}
	}

	public int Price
	{
		get
		{
			return price;
		}
	}

	public StateVariable StateVariable
	{
		get
		{
			return stateVariable;
		}
	}

	public bool InMenuForThisStatue
	{
		set
		{
			inMenuCameraZone.enabled = value;
		}
	}

	bool IInteractionReceiver.Interact(Item item)
	{
		UpgradeStatueMenu.Show(this);
		return false;
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return !stateVariable.BoolValue;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}

	protected void OnValidate()
	{
		setKobanGraphic();
	}

	protected void Awake()
	{
		setKobanGraphic();
		inMenuCameraZone.enabled = false;
		updateKobanVisabilityAndCameraZone();
		stateVariable.SubscribeToChanges(onStateChange);
	}

	protected void onStateChange()
	{
		updateKobanVisabilityAndCameraZone();
	}

	protected void updateKobanVisabilityAndCameraZone()
	{
		kobanGraphicsRoot.SetActive(!stateVariable.BoolValue);
		if ((bool)approachCameraZone)
		{
			approachCameraZone.enabled = !stateVariable.BoolValue;
		}
	}

	protected void setKobanGraphic()
	{
		for (int i = 0; i < kobanGraphics.Length; i++)
		{
			kobanGraphics[i].SetActive(i == (int)upgradeType);
		}
	}
}
