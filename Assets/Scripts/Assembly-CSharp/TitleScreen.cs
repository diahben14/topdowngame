using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleScreen : GUIMode
{
	[SerializeField]
	private GameObject newGameButton;

	[SerializeField]
	private GameObject continueButton;

	[SerializeField]
	private GameObject demoStartButton;

	[SerializeField]
	protected float dismissalDuration;

	protected bool lockout;

	protected Animator cachedAnimator;

	protected override void guiModeSetup()
	{
		cachedAnimator = GetComponent<Animator>();
	}

	protected override void beforeEnable()
	{
		CameraController.CutsceneFade = 1f;
	}

	protected override IEnumerator afterEnable()
	{
		lockout = true;
		continueButton.SetActive(false);
		EventSystem.current.SetSelectedGameObject(demoStartButton);
		EventSystem.current.SetSelectedGameObject(demoStartButton);
		for (float f = 0f; f < 1f; f += Time.deltaTime)
		{
			CameraController.CutsceneFade = 1f - f;
			yield return 0;
		}
		lockout = false;
	}

	public void Continue()
	{
	}

	public void NewGame()
	{
		StartCoroutine(ifNotLockedOutCloseMenuAnd(delegate
		{
			AnnotationManager.DesignerMode = false;
			Profile.Reset();
			Profile.MarkGameAsStarted();
			Profile.GoToLastSpawn();
		}));
	}

	public void NewGame_DesignerMode()
	{
		StartCoroutine(ifNotLockedOutCloseMenuAnd(delegate
		{
			AnnotationManager.DesignerMode = true;
			Profile.Reset();
			Profile.MarkGameAsStarted();
			Profile.GoToLastSpawn();
		}));
	}

	public void NewDemoGame()
	{
		StartCoroutine(ifNotLockedOutCloseMenuAnd(delegate
		{
			DemoTimer.ResetTimer();
			Profile.Reset();
			Profile.MarkGameAsStarted();
			Profile.SetUpDemoState();
			Profile.GoToLastSpawn();
		}));
	}

	public void Quit()
	{
		Application.Quit();
	}

	protected IEnumerator ifNotLockedOutCloseMenuAnd(Action function)
	{
		if (lockout)
		{
			yield break;
		}
		lockout = true;
		cachedAnimator.SetTrigger("dismiss");
		if (dismissalDuration > 0f)
		{
			for (float f = 0f; f < 1f; f += Time.deltaTime / dismissalDuration)
			{
				CameraController.CutsceneFade = f;
				yield return 0;
			}
		}
		function();
	}
}
