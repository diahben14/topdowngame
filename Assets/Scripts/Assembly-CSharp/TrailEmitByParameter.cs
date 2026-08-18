using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;

public class TrailEmitByParameter : BySingleParameterBase
{
	protected TrailRenderer_Base trail;

	protected TrailRenderer unityTrail;

	protected override void setup()
	{
		unityTrail = GetComponent<TrailRenderer>();
		if (unityTrail == null)
		{
			trail = GetComponent<TrailRenderer_Base>();
			if (trail == null)
			{
				Debug.LogError("[TrailEmitByParameter] `" + base.name + "` couldn't find trail renderer, either a Unity one or a PigeonCoop one.", base.gameObject);
			}
		}
	}

	protected override void setValue(bool boolValue, float floatValue)
	{
		if (unityTrail != null)
		{
			unityTrail.emitting = boolValue;
		}
		else if (trail != null)
		{
			trail.Emit = boolValue;
		}
	}
}
