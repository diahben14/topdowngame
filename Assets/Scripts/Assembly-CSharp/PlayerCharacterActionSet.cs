using InControl;

public class PlayerCharacterActionSet : PlayerActionSet
{
	public PlayerTwoAxisAction Move;

	public PlayerAction MoveLeft;

	public PlayerAction MoveRight;

	public PlayerAction MoveUp;

	public PlayerAction MoveDown;

	public PlayerAction Item1;

	public PlayerAction Item2;

	public PlayerAction Item3;

	public PlayerAction Flask;

	public PlayerAction InteractDodge;

	public PlayerAction Shield;

	public PlayerAction Target;

	public PlayerAction ToggleTarget;

	public PlayerTwoAxisAction Look;

	public PlayerAction LookLeft;

	public PlayerAction LookRight;

	public PlayerAction LookUp;

	public PlayerAction LookDown;

	public PlayerTwoAxisAction DPad;

	public PlayerAction DPadLeft;

	public PlayerAction DPadRight;

	public PlayerAction DPadUp;

	public PlayerAction DPadDown;

	public PlayerAction Inventory;

	public PlayerAction Manual;

	public PlayerCharacterActionSet()
	{
		MoveUp = CreatePlayerAction("Move Up");
		MoveDown = CreatePlayerAction("Move Down");
		MoveLeft = CreatePlayerAction("Move Left");
		MoveRight = CreatePlayerAction("Move Right");
		Move = CreateTwoAxisPlayerAction(MoveLeft, MoveRight, MoveDown, MoveUp);
		Item1 = CreatePlayerAction("Use Item 1");
		Item2 = CreatePlayerAction("Use Item 2");
		Item3 = CreatePlayerAction("Use Item 3");
		Flask = CreatePlayerAction("Flask");
		InteractDodge = CreatePlayerAction("Interact / Dodge");
		Shield = CreatePlayerAction("Shield");
		Target = CreatePlayerAction("Focus");
		ToggleTarget = CreatePlayerAction("Toggle Focus");
		LookUp = CreatePlayerAction("Look Up");
		LookDown = CreatePlayerAction("Look Down");
		LookLeft = CreatePlayerAction("Look Left");
		LookRight = CreatePlayerAction("Look Right");
		Look = CreateTwoAxisPlayerAction(LookLeft, LookRight, LookDown, LookUp);
		DPadUp = CreatePlayerAction("D-Pad Up");
		DPadDown = CreatePlayerAction("D-Pad Down");
		DPadLeft = CreatePlayerAction("D-Pad Left");
		DPadRight = CreatePlayerAction("D-Pad Right");
		DPad = CreateTwoAxisPlayerAction(DPadLeft, DPadRight, DPadDown, DPadUp);
		Inventory = CreatePlayerAction("Open Inventory");
		Manual = CreatePlayerAction("Open Manual");
	}

	public static PlayerCharacterActionSet CreateWithDefaultBindings()
	{
		PlayerCharacterActionSet playerCharacterActionSet = new PlayerCharacterActionSet();
		playerCharacterActionSet.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		playerCharacterActionSet.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);
		playerCharacterActionSet.MoveUp.AddDefaultBinding(InputControlType.LeftStickUp);
		playerCharacterActionSet.MoveDown.AddDefaultBinding(InputControlType.LeftStickDown);
		playerCharacterActionSet.Item1.AddDefaultBinding(InputControlType.Action3);
		playerCharacterActionSet.Item2.AddDefaultBinding(InputControlType.Action4);
		playerCharacterActionSet.Item3.AddDefaultBinding(InputControlType.Action2);
		playerCharacterActionSet.Flask.AddDefaultBinding(InputControlType.RightBumper);
		playerCharacterActionSet.InteractDodge.AddDefaultBinding(InputControlType.Action1);
		playerCharacterActionSet.Shield.AddDefaultBinding(InputControlType.RightTrigger);
		playerCharacterActionSet.Target.AddDefaultBinding(InputControlType.LeftTrigger);
		playerCharacterActionSet.ToggleTarget.AddDefaultBinding(InputControlType.RightStickButton);
		playerCharacterActionSet.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
		playerCharacterActionSet.LookRight.AddDefaultBinding(InputControlType.RightStickRight);
		playerCharacterActionSet.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
		playerCharacterActionSet.LookDown.AddDefaultBinding(InputControlType.RightStickDown);
		playerCharacterActionSet.DPadLeft.AddDefaultBinding(InputControlType.DPadLeft);
		playerCharacterActionSet.DPadRight.AddDefaultBinding(InputControlType.DPadRight);
		playerCharacterActionSet.DPadUp.AddDefaultBinding(InputControlType.DPadUp);
		playerCharacterActionSet.DPadDown.AddDefaultBinding(InputControlType.DPadDown);
		playerCharacterActionSet.Inventory.AddDefaultBinding(InputControlType.LeftBumper);
		playerCharacterActionSet.Manual.AddDefaultBinding(InputControlType.Select);
		playerCharacterActionSet.Manual.AddDefaultBinding(InputControlType.Back);
		playerCharacterActionSet.Manual.AddDefaultBinding(InputControlType.TouchPadButton);
		playerCharacterActionSet.Manual.AddDefaultBinding(InputControlType.Minus);
		playerCharacterActionSet.Manual.AddDefaultBinding(InputControlType.View);
		playerCharacterActionSet.MoveLeft.AddDefaultBinding(Key.A);
		playerCharacterActionSet.MoveRight.AddDefaultBinding(Key.D);
		playerCharacterActionSet.MoveUp.AddDefaultBinding(Key.W);
		playerCharacterActionSet.MoveDown.AddDefaultBinding(Key.S);
		playerCharacterActionSet.Item1.AddDefaultBinding(Key.J);
		playerCharacterActionSet.Item2.AddDefaultBinding(Key.K);
		playerCharacterActionSet.Item3.AddDefaultBinding(Key.L);
		playerCharacterActionSet.Flask.AddDefaultBinding(Key.P);
		playerCharacterActionSet.InteractDodge.AddDefaultBinding(Key.Space);
		playerCharacterActionSet.Shield.AddDefaultBinding(Key.Semicolon);
		playerCharacterActionSet.Target.AddDefaultBinding(Key.LeftShift);
		playerCharacterActionSet.ToggleTarget.AddDefaultBinding(Key.CapsLock);
		playerCharacterActionSet.DPadLeft.AddDefaultBinding(Key.LeftArrow);
		playerCharacterActionSet.DPadRight.AddDefaultBinding(Key.RightArrow);
		playerCharacterActionSet.DPadUp.AddDefaultBinding(Key.UpArrow);
		playerCharacterActionSet.DPadDown.AddDefaultBinding(Key.DownArrow);
		playerCharacterActionSet.Inventory.AddDefaultBinding(Key.Tab);
		playerCharacterActionSet.Manual.AddDefaultBinding(Key.F1);
		return playerCharacterActionSet;
	}
}
