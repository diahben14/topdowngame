using System.Collections;
using UnityEngine;

public class SealCombine : GUIMode
{
	[SerializeField]
	protected float animationDuration;

	[SerializeField]
	protected Item[] sealPartItems;

	[SerializeField]
	protected Item sealFullItem;

	protected static SealCombine instance;

	protected Animator cachedAnimator;

	protected override bool pauseTime
	{
		get
		{
			return true;
		}
	}

	public static void Show()
	{
		GUIMode.PushMode(instance);
	}

	protected override void guiModeSetup()
	{
		instance = this;
		cachedAnimator = GetComponent<Animator>();
	}

	protected override IEnumerator afterEnable()
	{
		cachedAnimator.SetBool("show", true);
		for (float f = 0f; f < animationDuration; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
		for (int i = 0; i < sealPartItems.Length; i++)
		{
			sealPartItems[i].Quantity--;
		}
		sealFullItem.Quantity++;
		GUIMode.PopMode(this);
		ItemPresentation.PresentItem(sealFullItem);
	}

	[ContextMenu("test")]
	protected void test()
	{
		Show();
	}
}
