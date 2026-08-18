using System.Collections.Generic;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
	[SerializeField]
	[EventRef]
	protected string openSFX;

	[SerializeField]
	[EventRef]
	protected string closeSFX;

	[SerializeField]
	[EventRef]
	protected string[] assignSFX;

	[SerializeField]
	protected Animator animator;

	[SerializeField]
	protected Image[] wieldedItemImages;

	[SerializeField]
	protected Animator[] wieldedItemAnimators;

	[SerializeField]
	protected Text[] wieldedItemQuantity;

	[SerializeField]
	protected Image[] wieldedItemBackingImages;

	[SerializeField]
	protected Image[] wieldedItemQuantityBackingImages;

	[SerializeField]
	protected Image[] wieldedItemButtonIconImages;

	[SerializeField]
	protected Color[] buttonColours_PS4;

	[SerializeField]
	protected Color[] buttonColours_XB1;

	[SerializeField]
	protected LayoutGroup equipmentRoot;

	[SerializeField]
	protected LayoutGroup suppliesRoot;

	[SerializeField]
	protected LayoutGroup wellSuppliesRoot;

	[SerializeField]
	protected LayoutGroup gearRoot;

	[SerializeField]
	protected LayoutGroup flaskRoot;

	[SerializeField]
	protected LayoutGroup offeringsRoot;

	[SerializeField]
	protected ItemIcon inventoryItemPrefab;

	[SerializeField]
	protected ItemIcon inventoryItemWithQuantityPrefab;

	[SerializeField]
	protected ItemIcon inventoryItemWithDenominatorPrefab;

	[SerializeField]
	protected ItemIcon inventiryIconForOfferingPrefab;

	[SerializeField]
	protected Text moneyQuantityText;

	[SerializeField]
	protected SmallMoneyItem moneyItem;

	[SerializeField]
	protected FlaskItem flaskItem;

	[SerializeField]
	protected Animator equipPromptAnimator;

	[SerializeField]
	protected float inventoryOpenCloseDurationApproximation = 0.2f;

	[SerializeField]
	protected Color zeroQuantityItemTint;

	protected static InventoryDisplay instance;

	protected static bool inventoryOpenToggle;

	protected List<ItemIcon> allItemIcons = new List<ItemIcon>();

	protected List<ItemIcon> offeringIcons = new List<ItemIcon>();

	protected GUIMode controllingGUIMode;

	protected float lastInventoryOpenTime = float.NegativeInfinity;

	[Header("Offering Popup")]
	[SerializeField]
	protected GameObject offeringIcon;

	[SerializeField]
	protected Transform offeringPopup_movableRoot;

	[SerializeField]
	protected GameObject offeringPopup_questionMarkRoot;

	[SerializeField]
	protected GameObject offeringPopup_root;

	[SerializeField]
	protected GameObject offeringPopup_offerText;

	[SerializeField]
	protected GameObject offeringPopup_nextText;

	[SerializeField]
	protected TextMeshProUGUI offeringPopup_priceText;

	[SerializeField]
	protected MixedTextBuilder offeringPopup_statText;

	[SerializeField]
	protected TextMeshProUGUI offeringPopup_levelText;

	[SerializeField]
	protected TextMeshProUGUI offeringPopup_levelPlusOneText;

	[SerializeField]
	protected GameObject offeringPopup_levelPlusOneGlow;

	[SerializeField]
	protected GameObject offeringPopup_ButtonPrompt;

	[SerializeField]
	protected LanguageLine statLabelQuestionMarkLanguageLine;

	[SerializeField]
	protected LanguageLine formattedPriceLine;

	[SerializeField]
	protected Item hiddenMoneyLevelItem;

	[SerializeField]
	protected ParticleSystem[] nearAltarParticleSystems;

	protected OfferingItem offeringItemToOffer;

	public static bool InventoryOpen { get; private set; }

	public static float InventoryOpenParam { get; private set; }

	private void Awake()
	{
		instance = this;
		Transform parent = base.transform;
		while (controllingGUIMode == null && parent != null)
		{
			controllingGUIMode = parent.GetComponent<GUIMode>();
			parent = parent.parent;
		}
		setupOfferingPopup(null, null, false);
	}

	private void FixedUpdate()
	{
		if (!InventoryOpen && PlayerInput.Actions != null && PlayerInput.Actions.Inventory.WasPressed)
		{
			inventoryOpenToggle = true;
		}
	}

	private void Update()
	{
		if (InventoryOpen && PlayerInput.Actions != null && PlayerInput.Actions.Inventory.WasPressed)
		{
			inventoryOpenToggle = false;
		}
		bool num = inventoryOpenToggle && controllingGUIMode.ReceiveInput;
		if (!controllingGUIMode.ReceiveInput)
		{
			inventoryOpenToggle = false;
		}
		if (num && !InventoryOpen)
		{
			SFX.PlayAudioClip(openSFX);
			updateInventory();
		}
		if (!num && InventoryOpen)
		{
			SFX.PlayAudioClip(closeSFX);
		}
		InventoryOpen = num;
		TimeManager.UpdateTimeScale();
		animator.SetBool("show", InventoryOpen);
		InventoryOpenParam = Mathf.Clamp01(InventoryOpenParam + TimeManager.UnscaledDeltaTime / inventoryOpenCloseDurationApproximation * (float)(InventoryOpen ? 1 : (-1)));
		int num2 = 0;
		for (int i = 0; i < Inventory.kButtonCount; i++)
		{
			ButtonAssignableItem buttonAssignableItem = Inventory.ButtonAssignableItemFromButtonNumber(i);
			if ((bool)buttonAssignableItem && buttonAssignableItem.ShouldShowInInventory)
			{
				wieldedItemImages[i].sprite = buttonAssignableItem.Icon;
				wieldedItemImages[i].enabled = true;
				wieldedItemQuantity[i].text = buttonAssignableItem.QuantityString;
				wieldedItemButtonIconImages[i].enabled = false;
				wieldedItemQuantityBackingImages[i].enabled = !buttonAssignableItem.SuppressQuantity;
				num2++;
			}
			else
			{
				wieldedItemImages[i].sprite = null;
				wieldedItemImages[i].enabled = false;
				wieldedItemQuantityBackingImages[i].enabled = false;
				wieldedItemQuantity[i].text = string.Empty;
				wieldedItemButtonIconImages[i].enabled = true;
			}
		}
		equipPromptAnimator.SetBool("show", num2 == 0 && (Inventory.GetHeldItemsOfType(Item.ItemType.SUPPLIES).Count > 0 || Inventory.GetHeldItemsOfType(Item.ItemType.EQUIPMENT).Count > 0) && !InventoryOpen && !GUIMode.PauseTime);
		if (!InventoryOpen)
		{
			return;
		}
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		if (!(currentSelectedGameObject != null))
		{
			return;
		}
		ItemIcon component = currentSelectedGameObject.GetComponent<ItemIcon>();
		if (!component)
		{
			return;
		}
		ButtonAssignableItem buttonAssignableItem2 = component.Item as ButtonAssignableItem;
		if ((bool)component && (bool)buttonAssignableItem2 && PlayerInput.Actions != null)
		{
			if (PlayerInput.Actions.InteractDodge.WasPressed)
			{
				PlayerCharacter.AttemptToUseFromMenu(buttonAssignableItem2);
			}
			if (PlayerInput.Actions.Item1.WasPressed)
			{
				equip(0, buttonAssignableItem2);
			}
			if (PlayerInput.Actions.Item2.WasPressed)
			{
				equip(1, buttonAssignableItem2);
			}
			if (PlayerInput.Actions.Item3.WasPressed)
			{
				equip(2, buttonAssignableItem2);
			}
		}
		OfferingItem oi = component.Item as OfferingItem;
		setupOfferingPopup(oi, component, PlayerInput.Actions.InteractDodge.WasPressed);
		for (int j = 0; j < offeringIcons.Count; j++)
		{
			PlayerCharacter.StatState statState = PlayerCharacter.StatStateFromOfferingItem(offeringIcons[j].Item as OfferingItem);
			offeringIcons[j].Upgrade_X_OutImage.gameObject.SetActive(statState == PlayerCharacter.StatState.SUPRESSED);
			offeringIcons[j].Upgrade_Star.gameObject.SetActive(statState == PlayerCharacter.StatState.HERO);
		}
	}

	protected void setupOfferingPopup(OfferingItem oi, ItemIcon itemIcon, bool actionButtonPressed)
	{
		for (int i = 0; i < offeringIcons.Count; i++)
		{
			offeringIcons[i].Upgrade_CurrentLevelForComparison.gameObject.SetActive(false);
			offeringIcons[i].Upgrade_NextLevelForComparison.gameObject.SetActive(false);
			offeringIcons[i].Upgrade_ComparisonArrow.gameObject.SetActive(false);
			offeringIcons[i].Upgrade_CurrentLevel.gameObject.SetActive(true);
		}
		if (oi == null)
		{
			offeringPopup_questionMarkRoot.SetActive(false);
			offeringPopup_root.SetActive(false);
			return;
		}
		Vector3 position = itemIcon.transform.position;
		offeringPopup_movableRoot.transform.position = position;
		offeringPopup_questionMarkRoot.SetActive(true);
		offeringPopup_root.SetActive(false);
	}

	protected void buy()
	{
		int num = offeringItemToOffer.PriceForNext();
		moneyItem.Quantity -= num;
		offeringItemToOffer.Quantity--;
		offeringItemToOffer.upgradeItemReceived.Quantity++;
		hiddenMoneyLevelItem.Quantity++;
	}

	protected void doneCallback()
	{
		ItemPresentation.PresentItem(offeringItemToOffer.upgradeItemReceived);
	}

	protected void equip(int index, ButtonAssignableItem bui)
	{
		Inventory.AssignItemToButton(index, bui);
		SFX.PlayAudioClip(assignSFX[index]);
		wieldedItemAnimators[index].SetTrigger("equip");
	}

	private void updateInventory()
	{
		for (int i = 0; i < nearAltarParticleSystems.Length; i++)
		{
			nearAltarParticleSystems[i].gameObject.SetActive(false);
		}
		cleanup();
		updateType(Item.ItemType.EQUIPMENT, equipmentRoot, inventoryItemPrefab, true);
		updateType(Item.ItemType.SUPPLIES, suppliesRoot, inventoryItemWithQuantityPrefab, true);
		updateType(Item.ItemType.GEAR, gearRoot, inventoryItemWithQuantityPrefab, false);
		updateType(Item.ItemType.OFFERINGS, offeringsRoot, inventiryIconForOfferingPrefab, true);
		moneyQuantityText.text = moneyItem.QuantityString;
		EventSystem current = EventSystem.current;
		if ((bool)current && !current.alreadySelecting && allItemIcons != null && allItemIcons.Count > 0)
		{
			current.SetSelectedGameObject(allItemIcons[0].gameObject);
		}
	}

	private void updateType(Item.ItemType type, LayoutGroup root, ItemIcon itemIconPrefab, bool interactable)
	{
		List<Item> heldItemsOfType = Inventory.GetHeldItemsOfType(type);
		for (int i = 0; i < heldItemsOfType.Count; i++)
		{
			if (!heldItemsOfType[i].ShouldShowInInventory)
			{
				continue;
			}
			ItemIcon itemIcon = Object.Instantiate(itemIconPrefab);
			itemIcon.Item = heldItemsOfType[i];
			itemIcon.Image.sprite = heldItemsOfType[i].Icon;
			itemIcon.RectTransform.SetParent(root.transform);
			itemIcon.RectTransform.transform.localPosition = Vector3.zero;
			itemIcon.RectTransform.SetAsLastSibling();
			itemIcon.RectTransform.localScale = Vector3.one;
			itemIcon.button.interactable = interactable;
			if ((bool)itemIcon.Quantity)
			{
				if (!heldItemsOfType[i].SuppressQuantity)
				{
					itemIcon.Quantity.text = heldItemsOfType[i].QuantityString;
				}
				else
				{
					itemIcon.Quantity.text = "";
				}
			}
			if ((bool)itemIcon.Denominator && itemIcon.Item.HasDenominator)
			{
				itemIcon.Denominator.text = heldItemsOfType[i].DenominatorString.ToString();
			}
			if (heldItemsOfType[i].Quantity == 0)
			{
				itemIcon.Image.color = zeroQuantityItemTint;
			}
			allItemIcons.Add(itemIcon);
			if (type == Item.ItemType.OFFERINGS)
			{
				offeringIcons.Add(itemIcon);
				itemIcon.Upgrade_PanelRoot.SetActive(false);
			}
		}
	}

	private void cleanup()
	{
		for (int i = 0; i < allItemIcons.Count; i++)
		{
			Object.Destroy(allItemIcons[i].gameObject);
		}
		allItemIcons.Clear();
		offeringIcons.Clear();
	}
}
