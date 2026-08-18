public interface IInteractionReceiver
{
	bool Interact(Item i);

	bool CanReceiveInteration();

	bool ShouldShowGenericInteractPrompt();
}
