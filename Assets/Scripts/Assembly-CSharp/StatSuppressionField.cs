using UnityEngine;

public class StatSuppressionField : MonoBehaviour
{
	protected static StatSuppressionField instance;

	public static bool IsActive
	{
		get
		{
			return instance != null;
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
