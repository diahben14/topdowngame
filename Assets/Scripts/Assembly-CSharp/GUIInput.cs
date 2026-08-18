using InControl;
using UnityEngine;

public class GUIInput : MonoBehaviour
{
	protected static GUIInput instance;

	protected GUIActionSet actionSet;

	[SerializeField]
	protected InControlInputModule inputModule;

	public static GUIActionSet Actions
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
		actionSet = GUIActionSet.CreateWithDefaultBindings();
		inputModule.SubmitAction = actionSet.Confirm;
		inputModule.CancelAction = actionSet.Cancel;
		inputModule.MoveAction = actionSet.Navigate;
	}

	protected void OnDestroy()
	{
		actionSet.Destroy();
	}
}
