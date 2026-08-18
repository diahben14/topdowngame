using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeStatueMenu : GUIMode
{
	[SerializeField]
	protected GameObject yesBuyButton;

	[SerializeField]
	protected GameObject noCancelButton;

	[SerializeField]
	protected GameObject tooPoorCloseButton;

	[SerializeField]
	protected float animationDuration;

	[SerializeField]
	protected Item currencyItem;

	[SerializeField]
	protected Sprite[] kobanCoinGraphics;

	[SerializeField]
	protected Item[] upgradeItems;

	[SerializeField]
	protected Image kobanCoinRenderer;

	[SerializeField]
	protected Image[] triangleRenderers;

	[SerializeField]
	protected Sprite emptyTriangleGraphic;

	[SerializeField]
	protected Sprite fullTriangleGraphic;

	protected static UpgradeStatue currentStatue;

	protected static UpgradeStatueMenu instance;

	protected Animator cachedAnimator;

	protected override bool pauseTime
	{
		get
		{
			return true;
		}
	}

	public static void Show(UpgradeStatue statue)
	{
		currentStatue = statue;
		GUIMode.PushMode(instance);
	}

	protected override IEnumerator afterEnable()
	{
		currentStatue.InMenuForThisStatue = true;
		if (currencyItem.Quantity < currentStatue.Price)
		{
			noCancelButton.SetActive(false);
			yesBuyButton.SetActive(false);
			tooPoorCloseButton.SetActive(true);
			EventSystem.current.SetSelectedGameObject(tooPoorCloseButton);
		}
		else
		{
			noCancelButton.SetActive(true);
			yesBuyButton.SetActive(true);
			tooPoorCloseButton.SetActive(false);
			EventSystem.current.SetSelectedGameObject(noCancelButton);
		}
		kobanCoinRenderer.sprite = kobanCoinGraphics[(int)currentStatue.UpgradeType];
		for (int i = 0; i < triangleRenderers.Length; i++)
		{
			if (i < currentStatue.Price)
			{
				triangleRenderers[i].gameObject.SetActive(true);
				if (i < currencyItem.Quantity)
				{
					triangleRenderers[i].sprite = fullTriangleGraphic;
				}
				else
				{
					triangleRenderers[i].sprite = emptyTriangleGraphic;
				}
			}
			else
			{
				triangleRenderers[i].gameObject.SetActive(false);
			}
		}
		cachedAnimator.SetBool("show", true);
		for (float f = 0f; f < animationDuration; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
	}

	protected override IEnumerator beforeDisable()
	{
		cachedAnimator.SetBool("show", false);
		for (float f = 0f; f < animationDuration; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
	}

	protected override void guiModeSetup()
	{
		instance = this;
		cachedAnimator = GetComponent<Animator>();
	}

	public void __Buy()
	{
		if (base.ReceiveInput)
		{
			doBuy();
		}
	}

	public void __Exit()
	{
		if (base.ReceiveInput)
		{
			cleanup();
		}
	}

	protected void doBuy()
	{
		cleanup();
		Item obj = upgradeItems[(int)currentStatue.UpgradeType];
		ItemPresentation.PresentItem(obj, 1);
		obj.Quantity++;
		currencyItem.Quantity -= currentStatue.Price;
		currentStatue.StateVariable.BoolValue = true;
	}

	protected void cleanup()
	{
		currentStatue.InMenuForThisStatue = false;
		GUIMode.PopMode(this);
	}
}
