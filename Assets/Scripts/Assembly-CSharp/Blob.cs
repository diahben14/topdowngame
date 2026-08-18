using UnityEngine;

public class Blob : NavigatingMonster
{
	[Header("Blob")]
	[SerializeField]
	protected float attackDistance = 2f;

	[SerializeField]
	protected float followDistance = 8f;

	[SerializeField]
	protected float wanderDistance = 5f;

	[SerializeField]
	protected float minWanderDelay = 1f;

	[SerializeField]
	protected float maxWanderDelay = 4f;

	[SerializeField]
	protected float chaseWhenNotSeenTime = 3f;

	protected float nextWanderTime;

	protected Vector3 startingPosition;

	protected float lastSeenTime;

	protected bool hasEverSeenPlayer;

	protected override void monster_Start()
	{
		startingPosition = base.transform.position;
		nextWanderTime = Time.time + Random.Range(minWanderDelay, maxWanderDelay);
	}

	protected override void LocomotingBehaviourUpdate()
	{
		if (base.busy || !PlayerCharacter.Instanced)
		{
			return;
		}
		Vector3 vector = PlayerCharacter.WorldPosition - base.transform.position;
		if (canSeePlayerOrBaitCheck() && vector.magnitude < followDistance)
		{
			hasEverSeenPlayer = true;
			lastSeenTime = Time.time;
		}
		if (Time.time < lastSeenTime + chaseWhenNotSeenTime && hasEverSeenPlayer)
		{
			base.locomotionTarget = base.playerOrBaitWorldPosition;
			base.facingTarget = base.transform.position + nma.desiredVelocity.normalized;
			cachedAnimator.SetBool("seek", true);
			if (vector.magnitude < attackDistance)
			{
				cachedAnimator.SetTrigger("attack");
			}
			return;
		}
		base.monsterAggroed = false;
		if (nextWanderTime < Time.time)
		{
			Vector3 onUnitSphere = Random.onUnitSphere;
			onUnitSphere.y = 0f;
			onUnitSphere = onUnitSphere.normalized * wanderDistance;
			Vector3 vector3 = (base.locomotionTarget = startingPosition + onUnitSphere);
			base.facingTarget = vector3;
			cachedAnimator.SetBool("seek", true);
			nextWanderTime = Time.time + Random.Range(minWanderDelay, maxWanderDelay);
		}
		if ((base.locomotionTarget - base.transform.position).sqrMagnitude < 1f)
		{
			cachedAnimator.SetBool("seek", false);
		}
	}
}
