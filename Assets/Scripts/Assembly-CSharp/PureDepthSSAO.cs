using UnityEngine;

[ExecuteInEditMode]
public class PureDepthSSAO : MonoBehaviour
{
	public Material mat;

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, mat);
	}
}
