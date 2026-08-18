using UnityEngine;

public class VisibleByParameter : BySingleParameterBase
{
	protected Renderer[] cachedRenderers;

	protected Light[] cachedLights;

	protected float[] cachedLightInitialIntensity;

	protected override void setup()
	{
		cachedRenderers = GetComponentsInChildren<Renderer>();
		cachedLights = GetComponentsInChildren<Light>();
		if (cachedRenderers == null && cachedLights == null)
		{
			Debug.LogError("[VisibleByParameter] `" + base.name + "` couldn't find renderers or lights.", base.gameObject);
		}
		cachedLightInitialIntensity = new float[cachedLights.Length];
		for (int i = 0; i < cachedLights.Length; i++)
		{
			cachedLightInitialIntensity[i] = cachedLights[i].intensity;
		}
	}

	protected override void setValue(bool boolValue, float floatValue)
	{
		for (int i = 0; i < cachedRenderers.Length; i++)
		{
			cachedRenderers[i].enabled = boolValue;
		}
		for (int j = 0; j < cachedLights.Length; j++)
		{
			cachedLights[j].enabled = boolValue;
			cachedLights[j].intensity = cachedLightInitialIntensity[j] * floatValue;
		}
	}
}
