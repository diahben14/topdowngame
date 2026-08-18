using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DrawCameraMask : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	protected Shader shader;

	protected static DrawCameraMask instance;

	[SerializeField]
	public Material material;

	public static float MaskFade { get; set; }

	private void Start()
	{
		instance = this;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (material == null)
		{
			material = new Material(shader);
			material.hideFlags = HideFlags.DontSave;
		}
		material.SetFloat("_Fade", MaskFade);
		material.SetTexture("_Mask", CameraController.MaskTexture);
		Graphics.Blit(source, destination, material);
	}
}
