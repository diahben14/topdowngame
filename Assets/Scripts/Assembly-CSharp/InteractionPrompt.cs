using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
	[SerializeField]
	protected Animator animator;

	[SerializeField]
	protected RectTransform canvasRectTransform;

	[SerializeField]
	protected RectTransform promptTransform;

	[SerializeField]
	protected float altitude;

	[SerializeField]
	protected float showHideSpeed = 2.5f;

	protected bool showing;

	protected float showingParameter;

	protected InteractionTrigger _interactionTriggerToShowPromptFor;

	protected InteractionTrigger _lastInteractionTriggerToShowPromptFor;

	protected void Update()
	{
		InteractionTrigger best = ProximityRegistered<InteractionTrigger>.GetBest();
		if (best != null && best.AnyInteractionReceiverShowsGenericInteractPrompt && PlayerCharacter.AbleToAct && !PlayerCharacter.InstanceIsDead && !GUIMode.PauseTime && !PlayerCharacter.InstanceInConduit && !InventoryDisplay.InventoryOpen)
		{
			if (showing && _lastInteractionTriggerToShowPromptFor != best)
			{
				showingParameter = 0f;
			}
			else
			{
				showing = true;
			}
			_lastInteractionTriggerToShowPromptFor = best;
		}
		else
		{
			showing = false;
		}
		showingParameter = Mathf.Clamp01(showingParameter + (float)(showing ? 1 : (-2)) * showHideSpeed * TimeManager.UnscaledDeltaTime);
		animator.Play("state", 0, showingParameter);
		animator.speed = 0f;
		InteractionTrigger interactionTrigger = ((best != null) ? best : _lastInteractionTriggerToShowPromptFor);
		if (interactionTrigger != null)
		{
			Vector2 anchoredPosition = Vector2.Scale(CameraController.WorldToViewportPoint(interactionTrigger.AnchorPosition + Vector3.up * altitude), canvasRectTransform.sizeDelta);
			promptTransform.anchoredPosition = anchoredPosition;
		}
	}
}
