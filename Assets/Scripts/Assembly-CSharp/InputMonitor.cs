using InControl;
using UnityEngine;
using UnityEngine.UI;

public class InputMonitor : MonoBehaviour
{
	[SerializeField]
	private Image action1;

	[SerializeField]
	private Image action2;

	[SerializeField]
	private Image action3;

	[SerializeField]
	private Image action4;

	[SerializeField]
	private Image R1;

	[SerializeField]
	private Image R2;

	[SerializeField]
	private Image R3;

	[SerializeField]
	private Image L1;

	[SerializeField]
	private Image L2;

	[SerializeField]
	private Image L3;

	[SerializeField]
	private Image DirUp;

	[SerializeField]
	private Image DirDown;

	[SerializeField]
	private Image DirRight;

	[SerializeField]
	private Image DirLeft;

	[SerializeField]
	private Image menu;

	[SerializeField]
	private Image LStick;

	[SerializeField]
	private Image RStick;

	private void Update()
	{
		InputDevice activeDevice = InputManager.ActiveDevice;
		action1.enabled = activeDevice.Action1.IsPressed;
		action2.enabled = activeDevice.Action2.IsPressed;
		action3.enabled = activeDevice.Action3.IsPressed;
		action4.enabled = activeDevice.Action4.IsPressed;
		R1.enabled = activeDevice.RightBumper.IsPressed;
		R2.enabled = activeDevice.RightTrigger.IsPressed;
		R3.enabled = activeDevice.RightStickButton.IsPressed;
		L1.enabled = activeDevice.LeftBumper.IsPressed;
		L2.enabled = activeDevice.LeftTrigger.IsPressed;
		L3.enabled = activeDevice.LeftStickButton.IsPressed;
		menu.enabled = activeDevice.CommandWasPressed;
		DirUp.enabled = activeDevice.DPadUp.IsPressed;
		DirDown.enabled = activeDevice.DPadDown.IsPressed;
		DirRight.enabled = activeDevice.DPadRight.IsPressed;
		DirLeft.enabled = activeDevice.DPadLeft.IsPressed;
		RStick.rectTransform.localPosition = activeDevice.RightStick.Vector * 28f;
		LStick.rectTransform.localPosition = activeDevice.LeftStick.Vector * 28f;
	}
}
