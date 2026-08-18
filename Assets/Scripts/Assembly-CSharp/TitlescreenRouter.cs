using UnityEngine;

public class TitlescreenRouter : MonoBehaviour
{
	[SerializeField]
	protected string titleScreenScene;

	private void Start()
	{
		if (PlayerCharacter.Instanced)
		{
			PlayerCharacter.RebootCharacter();
		}
		Setup.DoStandardSetup();
		GUIMode.ClearStack();
		GUIMode.PushMode(GUIMode.GetMode<TitleScreen>());
	}
}
