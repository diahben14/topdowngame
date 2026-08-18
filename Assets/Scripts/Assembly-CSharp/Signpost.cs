using System.Collections;
using UnityEngine;

[SelectionBase]
public class Signpost : MonoBehaviour, IInteractionReceiver
{
	[SerializeField]
	protected LanguageLine message;

	[SerializeField]
	protected CameraPositionOverrideTrigger cameraOverrideTrigger;

	public bool Interact(Item i)
	{
		if (i == null)
		{
			StartCoroutine(showSignpostMessage());
			return true;
		}
		return false;
	}

	protected void Awake()
	{
		if ((bool)cameraOverrideTrigger)
		{
			cameraOverrideTrigger.enabled = false;
		}
	}

	protected IEnumerator showSignpostMessage()
	{
		if ((bool)cameraOverrideTrigger)
		{
			cameraOverrideTrigger.enabled = true;
		}
		GenericMessage.ShowMessage(message);
		if ((bool)cameraOverrideTrigger)
		{
			while (GUIMode.PauseTime)
			{
				yield return 0;
			}
			cameraOverrideTrigger.enabled = false;
		}
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return true;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}
}
