using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObsidianDoorway : MonoBehaviour
{
	[SerializeField]
	protected List<Transform> cubes;

	[SerializeField]
	protected float cubesOpenParam;

	[SerializeField]
	protected float cubesScaleParam;

	[SerializeField]
	protected Transform cubeOpenOrigin;

	[SerializeField]
	protected float openStaggerFactor;

	[SerializeField]
	protected float distance = 4f;

	[SerializeField]
	protected Vector3 maxRotation;

	private void Update()
	{
		if (cubeOpenOrigin != null)
		{
			for (int i = 0; i < cubes.Count; i++)
			{
				Vector3 localPosition = cubes[i].localPosition;
				localPosition.z = 0f;
				float num = Vector3.Distance(localPosition, cubeOpenOrigin.localPosition);
				float num2 = Mathf.Clamp01(cubesOpenParam - num * openStaggerFactor);
				cubes[i].localScale = Vector3.one * (1f - num2) * cubesScaleParam;
				Vector3 localPosition2 = cubes[i].localPosition;
				localPosition2.z = num2 * distance;
				cubes[i].localPosition = localPosition2;
				cubes[i].localRotation = Quaternion.Euler(maxRotation * num2);
			}
		}
	}
}
