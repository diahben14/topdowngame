using UnityEngine;

public class CameraZoneByParameter : BySingleParameterBase
{
	protected CameraPositionOverrideTrigger cachedZone;

	protected override void setup()
	{
		cachedZone = GetComponent<CameraPositionOverrideTrigger>();
		if (cachedZone == null)
		{
			Debug.LogError("[CameraZoneByParameter] `" + base.name + "` couldn't find camera zone.", base.gameObject);
		}
	}

	protected override void setValue(bool boolValue, float floatValue)
	{
		cachedZone.enabled = boolValue;
	}
}
