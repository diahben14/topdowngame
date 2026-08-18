using UnityEngine;

public class StatefulCameraZone : MonoBehaviour
{
	[SerializeField]
	protected StateVariable enablingStateVariable;

	[SerializeField]
	protected bool activeWhenFalse;

	private void Awake()
	{
		if ((bool)enablingStateVariable)
		{
			enablingStateVariable.SubscribeToChanges(onStateChange);
		}
		updateActiveness();
	}

	private void OnDestroy()
	{
		if ((bool)enablingStateVariable)
		{
			enablingStateVariable.UnSubscribeToChanges(onStateChange);
		}
	}

	private void onStateChange()
	{
		updateActiveness();
	}

	private void updateActiveness()
	{
		if ((bool)enablingStateVariable)
		{
			GetComponent<CameraPositionOverrideTrigger>().enabled = enablingStateVariable.BoolValue != activeWhenFalse;
		}
	}
}
