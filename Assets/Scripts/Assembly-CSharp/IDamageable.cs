using UnityEngine;

public interface IDamageable
{
	bool ReceiveDamage(int damagePoints, int poisePoints, Vector3 physicsForce, float freezeTime, int poisonDamage);

	void BlockDamage(int staminaPoints, Vector3 physicsForce);
}
