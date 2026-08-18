using System;
using UnityEngine;

public class PowerSwitch : ToggleObjectByStateVariable, IDamageable
{
	[SerializeField]
	protected bool toggle;

	public bool ReceiveDamage(int damagePoints, int poisePoints, Vector3 physicsForce, float freezeTime, int poisonDamage)
	{
		throw new NotImplementedException();
	}

	void IDamageable.BlockDamage(int sp, Vector3 physicsForce)
	{
		if (toggle)
		{
			stateVar.BoolValue = !stateVar.BoolValue;
		}
		else
		{
			stateVar.BoolValue = true;
		}
	}
}
