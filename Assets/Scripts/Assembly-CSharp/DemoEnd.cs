using System.Collections;
using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoEnd : GUIMode
{
	[SerializeField]
	protected float safetyTimeBeforeCanGoToTitle;

	protected override bool pauseTime
	{
		get
		{
			return true;
		}
	}

	protected override IEnumerator afterEnable()
	{
		Debug.Log("afterEnable");
		for (float f = 0f; f < safetyTimeBeforeCanGoToTitle; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
		Debug.Log("safety done; waiting for input");
		while (!InputManager.ActiveDevice.AnyButton.WasPressed && !Input.anyKeyDown)
		{
			yield return 0;
		}
		Debug.Log("going to title screen");
		SceneManager.LoadScene("TitleScreen");
	}

	protected override IEnumerator beforeDisable()
	{
		yield break;
	}

	protected override void guiModeSetup()
	{
	}
}
