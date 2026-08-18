using UnityEngine;

[RequireComponent(typeof(ToggleObjectAnimation))]
public class ToggleObjectByStateVariable : MonoBehaviour
{
	[SerializeField]
	protected StateVariable stateVar;

	protected ToggleObjectAnimation cachedToggleObjectAnimation;

	private void Start()
	{
		cachedToggleObjectAnimation = GetComponent<ToggleObjectAnimation>();
		if ((bool)stateVar)
		{
			stateVar.SubscribeToChanges(onStateChange);
			cachedToggleObjectAnimation.SetToggleInstant(stateVar.BoolValue);
		}
	}

	private void OnDestroy()
	{
		if (Application.isPlaying && (bool)stateVar)
		{
			stateVar.UnSubscribeToChanges(onStateChange);
		}
	}

	protected void onStateChange()
	{
		cachedToggleObjectAnimation.SetToggle(stateVar.BoolValue);
	}
}
