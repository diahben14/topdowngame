using UnityEngine;

public class Waterfall : MonoBehaviour
{
	[Range(0f, 8f)]
	public float waterSpeed;

	[Range(1f, 25f)]
	public float waterGravity = 1f;

	[Range(0f, 1f)]
	public float uOffset;

	[Range(0f, 1f)]
	public float vOffset;
}
