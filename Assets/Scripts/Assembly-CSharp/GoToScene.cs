using System.Collections;
using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
	[SerializeField]
	protected float delay = 2f;

	[SerializeField]
	protected string sceneName;

	[SerializeField]
	protected bool suppressSkip;

	private IEnumerator Start()
	{
		Setup.DoStandardSetup();
		GUIMode.ClearStack();
		for (float f = 0f; f < delay; f += Time.deltaTime)
		{
			if ((InputManager.ActiveDevice.AnyButton.WasPressed || Input.anyKeyDown) && !suppressSkip)
			{
				break;
			}
			yield return 0;
		}
		SceneManager.LoadScene(sceneName);
	}
}
