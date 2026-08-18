using UnityEngine;

public class SleepPrompt : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	protected string scene;

	[SerializeField]
	protected string id = "in";

	[SerializeField]
	protected StateVariable visited;

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return !visited.BoolValue;
	}

	bool IInteractionReceiver.Interact(Item i)
	{
		visited.BoolValue = true;
		ScenePortal.DepartToScene(this, true, 2f, scene, id, true, 0f);
		return true;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}
}
