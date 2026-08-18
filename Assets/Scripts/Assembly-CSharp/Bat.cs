using UnityEngine;

public class Bat : NavigatingMonster
{
	[Header("Bat")]
	[SerializeField]
	protected float flyTowardsDistance = 8f;

	[SerializeField]
	protected float maxAcceleration = 10f;

	[SerializeField]
	protected float maxVelocity = 14f;

	[SerializeField]
	protected float averageAttackCooldown = 3f;

	[SerializeField]
	protected float attackDistance = 5f;

	protected float nextAttackTime;

	protected Vector3 smoothedVelocity;

	protected bool orbitLeft = true;

	protected override bool doNavMeshSnappingAndReturnTrueIfGravityShouldBeApplied()
	{
		return false;
	}

	protected override void LocomotingBehaviourUpdate()
	{
		if (PlayerCharacter.Instanced && base.IsAggroed)
		{
			Vector3 zero = Vector3.zero;
			Vector3 vector = base.playerOrBaitWorldPosition - base.transform.position;
			float @float = cachedAnimator.GetFloat("AP_fly");
			zero = ((!(vector.magnitude > flyTowardsDistance)) ? Vector3.Cross(vector.normalized, Vector3.up).normalized : vector.normalized);
			base.facingTarget = base.playerOrBaitWorldPosition;
			if (@float > 0f)
			{
				smoothedVelocity += zero * maxAcceleration * Time.deltaTime * @float;
				smoothedVelocity = Vector3.ClampMagnitude(smoothedVelocity, maxVelocity * @float);
				cachedRigidbody.velocity = smoothedVelocity;
			}
			else
			{
				smoothedVelocity = Vector3.zero;
			}
			if (vector.magnitude < attackDistance && Time.time > nextAttackTime)
			{
				nextAttackTime = Time.time + Mathx.Poisson(averageAttackCooldown);
				cachedAnimator.SetTrigger("attackOK");
				orbitLeft = !orbitLeft;
			}
		}
	}
}
