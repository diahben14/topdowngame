using UnityEngine;

public class MaterialByParameter : BySingleParameterBase
{
	protected Renderer cachedRenderer;

	protected Color cachedInitialEmissionColour;

	[SerializeField]
	protected bool modulateTintColour = true;

	[SerializeField]
	protected bool modulateEmission;

	protected override void setup()
	{
		cachedRenderer = GetComponent<Renderer>();
		if (cachedRenderer == null)
		{
			Debug.LogError("[MaterialByParameter] `" + base.name + "` couldn't find MeshRenderer.", base.gameObject);
		}
		if (modulateEmission)
		{
			cachedInitialEmissionColour = cachedRenderer.materials[0].GetColor("_EmissionColor");
		}
	}

	protected override void setValue(bool boolValue, float floatValue)
	{
		if (modulateTintColour)
		{
			cachedRenderer.materials[0].SetColor("_TintColor", Color.white * Mathf.Max(0f, floatValue));
		}
		if (modulateEmission)
		{
			cachedRenderer.materials[0].SetColor("_EmissionColor", cachedInitialEmissionColour * Mathf.Max(0f, floatValue));
		}
	}
}
