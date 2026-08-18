using InControl;

public class GUIActionSet : PlayerActionSet
{
	public PlayerTwoAxisAction Navigate;

	public PlayerAction NavigateLeft;

	public PlayerAction NavigateRight;

	public PlayerAction NavigateUp;

	public PlayerAction NavigateDown;

	public PlayerAction PreviousPage;

	public PlayerAction NextPage;

	public PlayerTwoAxisAction Look;

	public PlayerAction LookLeft;

	public PlayerAction LookRight;

	public PlayerAction LookUp;

	public PlayerAction LookDown;

	public PlayerAction Confirm;

	public PlayerAction Cancel;

	public PlayerAction Pause;

	public GUIActionSet()
	{
		NavigateLeft = CreatePlayerAction("NavigateLeft");
		NavigateRight = CreatePlayerAction("NavigateRight");
		NavigateUp = CreatePlayerAction("NavigateUp");
		NavigateDown = CreatePlayerAction("NavigateDown");
		Navigate = CreateTwoAxisPlayerAction(NavigateLeft, NavigateRight, NavigateDown, NavigateUp);
		PreviousPage = CreatePlayerAction("PreviousPage");
		NextPage = CreatePlayerAction("NextPage");
		LookLeft = CreatePlayerAction("LookLeft");
		LookRight = CreatePlayerAction("LookRight");
		LookUp = CreatePlayerAction("LookUp");
		LookDown = CreatePlayerAction("LookDown");
		Look = CreateTwoAxisPlayerAction(LookLeft, LookRight, LookDown, LookUp);
		Confirm = CreatePlayerAction("Confirm");
		Cancel = CreatePlayerAction("Cancel");
		Pause = CreatePlayerAction("Pause");
	}

	public static GUIActionSet CreateWithDefaultBindings()
	{
		GUIActionSet gUIActionSet = new GUIActionSet();
		gUIActionSet.NavigateLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		gUIActionSet.NavigateRight.AddDefaultBinding(InputControlType.LeftStickRight);
		gUIActionSet.NavigateUp.AddDefaultBinding(InputControlType.LeftStickUp);
		gUIActionSet.NavigateDown.AddDefaultBinding(InputControlType.LeftStickDown);
		gUIActionSet.NavigateLeft.AddDefaultBinding(InputControlType.DPadLeft);
		gUIActionSet.NavigateRight.AddDefaultBinding(InputControlType.DPadRight);
		gUIActionSet.NavigateUp.AddDefaultBinding(InputControlType.DPadUp);
		gUIActionSet.NavigateDown.AddDefaultBinding(InputControlType.DPadDown);
		gUIActionSet.PreviousPage.AddDefaultBinding(InputControlType.LeftStickLeft);
		gUIActionSet.NextPage.AddDefaultBinding(InputControlType.LeftStickRight);
		gUIActionSet.PreviousPage.AddDefaultBinding(InputControlType.DPadLeft);
		gUIActionSet.NextPage.AddDefaultBinding(InputControlType.DPadRight);
		gUIActionSet.PreviousPage.AddDefaultBinding(InputControlType.LeftTrigger);
		gUIActionSet.NextPage.AddDefaultBinding(InputControlType.RightTrigger);
		gUIActionSet.PreviousPage.AddDefaultBinding(InputControlType.LeftBumper);
		gUIActionSet.NextPage.AddDefaultBinding(InputControlType.RightBumper);
		gUIActionSet.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
		gUIActionSet.LookRight.AddDefaultBinding(InputControlType.RightStickRight);
		gUIActionSet.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
		gUIActionSet.LookDown.AddDefaultBinding(InputControlType.RightStickDown);
		gUIActionSet.Confirm.AddDefaultBinding(InputControlType.Action1);
		gUIActionSet.Cancel.AddDefaultBinding(InputControlType.Action2);
		gUIActionSet.Pause.AddDefaultBinding(InputControlType.Start);
		gUIActionSet.Pause.AddDefaultBinding(InputControlType.Options);
		gUIActionSet.Pause.AddDefaultBinding(InputControlType.Plus);
		gUIActionSet.Pause.AddDefaultBinding(InputControlType.Menu);
		gUIActionSet.NavigateLeft.AddDefaultBinding(Key.LeftArrow);
		gUIActionSet.NavigateRight.AddDefaultBinding(Key.RightArrow);
		gUIActionSet.NavigateUp.AddDefaultBinding(Key.UpArrow);
		gUIActionSet.NavigateDown.AddDefaultBinding(Key.DownArrow);
		gUIActionSet.NavigateLeft.AddDefaultBinding(Key.A);
		gUIActionSet.NavigateRight.AddDefaultBinding(Key.D);
		gUIActionSet.NavigateUp.AddDefaultBinding(Key.W);
		gUIActionSet.NavigateDown.AddDefaultBinding(Key.S);
		gUIActionSet.PreviousPage.AddDefaultBinding(Key.LeftArrow);
		gUIActionSet.NextPage.AddDefaultBinding(Key.RightArrow);
		gUIActionSet.PreviousPage.AddDefaultBinding(Key.PageUp);
		gUIActionSet.NextPage.AddDefaultBinding(Key.PageDown);
		gUIActionSet.PreviousPage.AddDefaultBinding(Key.A);
		gUIActionSet.NextPage.AddDefaultBinding(Key.D);
		gUIActionSet.Confirm.AddDefaultBinding(Key.Space);
		gUIActionSet.Confirm.AddDefaultBinding(Key.Return);
		gUIActionSet.Cancel.AddDefaultBinding(Key.Escape);
		gUIActionSet.Pause.AddDefaultBinding(Key.Escape);
		return gUIActionSet;
	}
}
