using System;
using System.Collections;
using InControl;
using UnityEngine;
using UnityEngine.UI;

public class ItemPresentation : GUIMode
{
	[SerializeField]
	protected float appearAnimationDuration = 2f;

	[SerializeField]
	protected float disappearAnimationDuration = 0.5f;

	[SerializeField]
	protected MixedTextBuilder collectionMessage;

	[SerializeField]
	protected LanguageLine defaultCollectionMessage;

	[SerializeField]
	protected Text quantityText;

	[SerializeField]
	protected GameObject quantityBackingRoot;

	[SerializeField]
	protected Transform rotationRoot;

	[SerializeField]
	protected float rotationSpeed = 90f;

	[Header("Special Behaviour Items")]
	[SerializeField]
	protected Item sealLeft;

	[SerializeField]
	protected Item sealRight;

	[SerializeField]
	protected GameObject hack_shield_kb;

	[SerializeField]
	protected GameObject hack_potion_kb;

	[SerializeField]
	protected GameObject hack_shield_controller;

	[SerializeField]
	protected GameObject hack_potion_controller;

	protected static ItemPresentation instance;

	protected Animator cachedAnimator;

	protected ItemPresentationGraphic[] itemGraphics;

	protected bool okToDismiss;

	protected Action afterPresentationCallback;

	private Quaternion userItemRotation = Quaternion.identity;

	protected override bool pauseTime
	{
		get
		{
			return true;
		}
	}

	public static void PresentItem(Item item)
	{
		PresentItem(item, 1);
	}

	public static void PresentItem(Item item, int quantity)
	{
		instance.presentItem(item, quantity);
	}

	protected void presentItem(Item item, int quantity)
	{
		userItemRotation = Quaternion.identity;
		rotationRoot.localRotation = Quaternion.identity;
		disableAllGraphicsBut(item);
		if (quantity > 1)
		{
			quantityText.text = quantity.ToString();
			quantityBackingRoot.SetActive(true);
		}
		else
		{
			quantityText.text = string.Empty;
			quantityBackingRoot.SetActive(false);
		}
		okToDismiss = false;
		if (item.CollectionMessage != null)
		{
			collectionMessage.InputLanguageLine = item.CollectionMessage;
		}
		else
		{
			collectionMessage.InputLanguageLine = defaultCollectionMessage;
		}
		afterPresentationCallback = null;
		if ((item == sealLeft && sealRight.Quantity == 1) || (item == sealRight && sealLeft.Quantity == 1))
		{
			afterPresentationCallback = SealCombine.Show;
		}
		bool flag = PlayerInput.Actions.LastDeviceClass != InputDeviceClass.Controller;
		hack_potion_kb.SetActive(flag && item.name.Contains("Flask"));
		hack_shield_kb.SetActive(flag && item.name.Contains("Shield"));
		hack_potion_controller.SetActive(!flag && item.name.Contains("Flask"));
		hack_shield_controller.SetActive(!flag && item.name.Contains("Shield"));
		GUIMode.PushMode(this);
	}

	protected void disableAllGraphicsBut(Item item)
	{
		bool flag = false;
		for (int i = 0; i < itemGraphics.Length; i++)
		{
			itemGraphics[i].gameObject.SetActive(false);
			for (int j = 0; j < itemGraphics[i].items.Length; j++)
			{
				if (itemGraphics[i].items[j] == item)
				{
					itemGraphics[i].gameObject.SetActive(true);
					flag = true;
				}
			}
		}
		if (!flag)
		{
			Debug.LogErrorFormat("No item presentation graphic for item `{0}`", item.name);
		}
	}

	protected override void guiModeSetup()
	{
		instance = this;
		cachedAnimator = GetComponent<Animator>();
		itemGraphics = GetComponentsInChildren<ItemPresentationGraphic>();
		base.transform.position = new Vector3(-9999f, -9999f, -9999f);
	}

	protected override IEnumerator afterEnable()
	{
		cachedAnimator.SetBool("show", true);
		for (float f = 0f; f < appearAnimationDuration; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
		okToDismiss = true;
	}

	protected override IEnumerator beforeDisable()
	{
		cachedAnimator.SetBool("show", false);
		for (float f = 0f; f < disappearAnimationDuration; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
		if (afterPresentationCallback != null)
		{
			afterPresentationCallback();
		}
	}

	protected void Update()
	{
		if (okToDismiss && (InputManager.ActiveDevice.AnyButton.WasPressed || Input.anyKeyDown))
		{
			okToDismiss = false;
			GUIMode.PopMode(instance);
		}
	}

	protected void LateUpdate()
	{
		if (okToDismiss)
		{
			Vector2 vector = GUIInput.Actions.Look.Vector * rotationSpeed * TimeManager.UnscaledDeltaTime;
			Quaternion quaternion = Quaternion.Euler(vector.y, 0f - vector.x, 0f);
			userItemRotation = quaternion * userItemRotation;
			rotationRoot.rotation = userItemRotation;
		}
	}
}
