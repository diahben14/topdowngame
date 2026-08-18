using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	protected static PlayerInput instance;

	protected PlayerCharacterActionSet actionSet;

	public static PlayerCharacterActionSet Actions
	{
		get
		{
			if (!(instance == null))
			{
				return instance.actionSet;
			}
			return null;
		}
	}

	protected void Start()
	{
		instance = this;
		actionSet = PlayerCharacterActionSet.CreateWithDefaultBindings();
	}

	protected void OnDestroy()
	{
		actionSet.Destroy();
	}
}
