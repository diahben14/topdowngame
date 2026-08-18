using UnityEngine;

public class CameraOverrideBehaviour : MonoBehaviour
{
	[SerializeField]
	protected bool on;

	[SerializeField]
	protected CameraBehaviourDataLayer data;

	protected bool previousOn;

	protected void FixedUpdate()
	{
		if (on && !previousOn)
		{
			CameraController.AddCameraBehaviourDataLayer(data);
		}
		if (!on && previousOn)
		{
			CameraController.RemoveCameraBehaviourDataLayer(data);
		}
		previousOn = on;
	}
}
