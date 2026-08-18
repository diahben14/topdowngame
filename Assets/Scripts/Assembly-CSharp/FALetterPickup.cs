using UnityEngine;

public class FALetterPickup : Pickup
{
	[Header("FA Letter Pickup")]
	[SerializeField]
	protected StateVariable pickupVariable;

	protected override bool alreadyPickedUp()
	{
		return pickupVariable.BoolValue;
	}

	protected override void onGetIt()
	{
		pickupVariable.BoolValue = true;
		GUIMode.PushMode(GUIMode.GetMode<FALetter>());
	}
}
