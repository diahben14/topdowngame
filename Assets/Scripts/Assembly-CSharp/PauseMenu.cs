using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : GUIMode
{
	[SerializeField]
	protected GameObject defaultSelection;

	[SerializeField]
	[EventRef]
	protected string sfx_pause;

	[SerializeField]
	[EventRef]
	protected string sfx_unPause;

	protected override bool pauseTime
	{
		get
		{
			return true;
		}
	}

	public void __button_ReturnToGame()
	{
		SFX.PlayAudioClip(sfx_unPause);
		SFX.PauseAudioBus = false;
		GUIMode.PopMode(this);
	}

	public void __button_QuitGame()
	{
		SFX.PauseAudioBus = false;
		Debug.Log("-- Quitting Video Game --");
		Application.Quit();
	}

	public void __button_ReturnToTitle()
	{
		SFX.PauseAudioBus = false;
		Profile.Save();
		GUIMode.ClearStack();
		SceneManager.LoadScene("TitleScreen");
	}

	protected override IEnumerator afterEnable()
	{
		GUIMode.rebootEventSystem();
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(defaultSelection);
		SFX.PlayAudioClip(sfx_pause);
		SFX.PauseAudioBus = true;
		yield break;
	}
}
