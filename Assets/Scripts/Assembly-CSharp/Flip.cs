using UnityEngine;

[ExecuteInEditMode]
public class Flip : MonoBehaviour
{
	[SerializeField]
	[HideInInspector]
	protected Material material;

	private void Awake()
	{
		material = new Material(Shader.Find("Hidden/FlipImageEffectShader"));
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (CameraController.Flip)
		{
			Graphics.Blit(source, destination, material);
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}
}
