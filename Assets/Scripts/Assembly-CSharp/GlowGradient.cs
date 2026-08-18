using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GlowGradient : MonoBehaviour
{
	private void Start()
	{
		MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[3]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0f, -1f, 0f)
		};
		mesh.triangles = new int[3] { 0, 1, 2 };
		mesh.colors = new Color[3]
		{
			Color.white,
			Color.black,
			Color.black
		};
		component.mesh = mesh;
	}
}
