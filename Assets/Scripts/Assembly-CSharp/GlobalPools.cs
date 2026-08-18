using UnityEngine;

public class GlobalPools : MonoBehaviour
{
	public PooledFX InteractionPrompt;

	public PooledFX AlertFX;

	public PooledFX FoxTechbolt;

	public static GlobalPools Instance;

	protected void Awake()
	{
		if ((bool)Instance)
		{
			Debug.LogError("Multiple GlobalPools.");
			Object.Destroy(base.gameObject);
		}
		else
		{
			Instance = this;
		}
	}
}
