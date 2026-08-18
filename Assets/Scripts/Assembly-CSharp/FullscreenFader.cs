using UnityEngine;

public class FullscreenFader : MonoBehaviour
{
	protected static float _fade;

	public static bool Whiteout { get; set; }

	public static float FullscreenFade
	{
		get
		{
			return _fade;
		}
		set
		{
			_fade = Mathf.Clamp01(value);
		}
	}
}
