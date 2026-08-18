using UnityEngine;

[ExecuteInEditMode]
public class WorldRTexCamera : MonoBehaviour
{
	protected void OnEnable()
	{
		RuntimeQualityManager.RegisterWorldRTexCamera(GetComponent<Camera>());
	}

	protected void OnDestroy()
	{
		RuntimeQualityManager.DeregisterWorldRTexCamera(GetComponent<Camera>());
	}
}
