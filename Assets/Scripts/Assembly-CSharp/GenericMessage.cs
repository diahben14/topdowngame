using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericMessage : GUIMode
{
	[SerializeField]
	protected MixedTextBuilder headerGlyphs;

	[SerializeField]
	protected GameObject okButton;

	[SerializeField]
	protected float animationDuration = 0.67f;

	protected Animator cachedAnimator;

	protected static GenericMessage instance;

	protected override bool pauseTime
	{
		get
		{
			return true;
		}
	}

	public static void ShowMessage(LanguageLine line)
	{
		GUIMode.PushMode(instance);
		if (line != null)
		{
			instance.headerGlyphs.InputLanguageLine = line;
		}
		else
		{
			instance.headerGlyphs.InputLanguageLine = null;
		}
		EventSystem.current.SetSelectedGameObject(instance.okButton);
	}

	protected override IEnumerator afterEnable()
	{
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

	public void ButtonOK()
	{
		if (base.ReceiveInput)
		{
			cleanup();
		}
	}

	protected void cleanup()
	{
		GUIMode.PopMode(this);
	}
}
