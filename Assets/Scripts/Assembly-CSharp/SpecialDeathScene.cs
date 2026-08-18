using UnityEngine;

public class SpecialDeathScene : MonoBehaviour
{
	protected static SpecialDeathScene instance;

	public static bool IsActive
	{
		get
		{
			if (instance != null)
			{
				return instance.gameObject.activeInHierarchy;
			}
			return false;
		}
	}

	protected void Awake()
	{
		instance = this;
	}

	protected void OnDestroy()
	{
		instance = null;
	}
}
