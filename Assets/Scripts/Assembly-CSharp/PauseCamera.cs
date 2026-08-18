using UnityEngine;

public class PauseCamera : MonoBehaviour
{
	[SerializeField]
	protected Camera[] cameras;

	protected static PauseCamera instance;

	public static void Match(Camera c)
	{
		for (int i = 0; i < instance.cameras.Length; i++)
		{
			RenderTexture targetTexture = instance.cameras[i].targetTexture;
			instance.cameras[i].CopyFrom(c);
			instance.cameras[i].targetTexture = targetTexture;
			instance.cameras[i].clearFlags = CameraClearFlags.Color;
			instance.cameras[i].backgroundColor = Color.black;
		}
	}

	public static void Render()
	{
		for (int i = 0; i < instance.cameras.Length; i++)
		{
			instance.cameras[i].Render();
		}
	}

	protected void Start()
	{
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
