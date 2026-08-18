using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericPrompt : GUIMode
{
	[SerializeField]
	protected MixedTextBuilder headerGlyphs;

	[SerializeField]
	protected GameObject defaultSelection;

	[SerializeField]
	protected float animationDuration;

	protected static GenericPrompt instance;

	protected static Action registeredYesAction;

	protected static Action registeredNoAction;

	protected Animator cachedAnimator;

	protected override bool pauseTime
	{
		get
		{
			return true;
		}
	}

	public static void ShowPrompt(LanguageLine line, Action yesAction, Action noAction)
	{
		registeredYesAction = yesAction;
		registeredNoAction = noAction;
		GUIMode.PushMode(instance);
		instance.headerGlyphs.InputLanguageLine = line;
		EventSystem.current.SetSelectedGameObject(instance.defaultSelection);
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

	public void ButtonYes()
	{
		if (base.ReceiveInput)
		{
			cleanup();
			if (registeredYesAction != null)
			{
				registeredYesAction();
			}
		}
	}

	public void ButtonNo()
	{
		if (base.ReceiveInput)
		{
			cleanup();
			if (registeredNoAction != null)
			{
				registeredNoAction();
			}
		}
	}

	protected void cleanup()
	{
		GUIMode.PopMode(this);
	}
}
