using UnityEngine;

public class ParticleEmitByParameter : BySingleParameterBase
{
	protected ParticleSystem.EmissionModule emitter;

	protected override void setup()
	{
		ParticleSystem component = GetComponent<ParticleSystem>();
		if (component != null)
		{
			emitter = component.emission;
		}
		if (component == null)
		{
			Debug.LogError("[ParticleEmitByParameter] `" + base.name + "` couldn't find particle system/emitter.", base.gameObject);
		}
	}

	protected override void setValue(bool boolValue, float floatValue)
	{
		emitter.enabled = boolValue;
	}
}
