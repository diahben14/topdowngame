using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HolySealDoor : ProximityRegistered<HolySealDoor>
{
	[SerializeField]
	protected List<Transform> cubes;

	[SerializeField]
	protected float cubesOpenParam;

	[SerializeField]
	protected Transform cubeOpenOrigin;

	[SerializeField]
	protected float openStaggerFactor;

	[SerializeField]
	protected StateVariable stateVariable;

	protected override bool isCandidateForBest
	{
		get
		{
			return !stateVariable.BoolValue;
		}
	}

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
				cubes[i].localScale = Vector3.one * (1f - num2);
				Vector3 localPosition2 = cubes[i].localPosition;
				localPosition2.z = (0f - num2 * num2) * 2f;
				cubes[i].localPosition = localPosition2;
			}
		}
	}

	public void Open()
	{
		if (!stateVariable.BoolValue)
		{
			stateVariable.BoolValue = true;
		}
		else
		{
			Debug.LogWarning("Tried to open door that was already open");
		}
	}
}
