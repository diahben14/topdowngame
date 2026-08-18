using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class BlitRTexToDisplay : MonoBehaviour
{
	[SerializeField]
	protected Shader shader;

	protected Material material;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
	}
}
