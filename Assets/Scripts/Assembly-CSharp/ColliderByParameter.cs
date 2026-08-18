using UnityEngine;

public class ColliderByParameter : BySingleParameterBase
{
	protected Collider[] cachedColliders;

	protected override void setup()
	{
		cachedColliders = GetComponents<Collider>();
		if (cachedColliders == null)
		{
			Debug.LogError("[ColliderByParameter] `" + base.name + "` couldn't find collider.", base.gameObject);
		}
	}

	protected override void setValue(bool boolValue, float floatValue)
	{
		for (int i = 0; i < cachedColliders.Length; i++)
		{
			cachedColliders[i].enabled = boolValue;
		}
	}
}
