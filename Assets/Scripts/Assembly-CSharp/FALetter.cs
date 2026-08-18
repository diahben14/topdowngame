using System.Collections;
using InControl;
using UnityEngine;

public class FALetter : GUIMode
{
	[Header("FA Letter")]
	[SerializeField]
	protected Item itemToGet;

	protected Animator cachedAnimator;

	protected override bool pauseTime
	{
		get
		{
			return true;
		}
	}

	protected override void guiModeSetup()
	{
		cachedAnimator = GetComponent<Animator>();
	}

	protected override IEnumerator afterEnable()
	{
		cachedAnimator.SetBool("show", true);
		for (float f = 0f; f < 2f; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
		while (!InputManager.ActiveDevice.AnyButton)
		{
			yield return 0;
		}
		GUIMode.PopMode(this);
		itemToGet.Quantity++;
		ItemPresentation.PresentItem(itemToGet);
	}

	protected override IEnumerator beforeDisable()
	{
		cachedAnimator.SetBool("show", false);
		yield return new WaitForSeconds(1f);
	}
}
