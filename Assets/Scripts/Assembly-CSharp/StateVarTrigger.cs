using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StateVarTrigger : PlayerTriggerZone<StateVarTrigger>
{
	[SerializeField]
	protected StateVariable stateVariable;

	[SerializeField]
	protected bool onEnterSetTo = true;

	protected override void onChange()
	{
		if (base.PlayerInZone && Time.time > 0f)
		{
			stateVariable.BoolValue = onEnterSetTo;
		}
	}
}
