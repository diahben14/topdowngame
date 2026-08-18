public class LadderEnd : ProximityRegistered<LadderEnd>, IInteractionReceiver
{
	public Ladder ParentLadder;

	private bool okToGrab
	{
		get
		{
			return ParentLadder.OkToGrabEnd(this);
		}
	}

	bool IInteractionReceiver.Interact(Item i)
	{
		ParentLadder.ClimbOn(this);
		return true;
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return okToGrab;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return okToGrab;
	}
}
