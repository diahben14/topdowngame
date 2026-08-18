using UnityEngine;

public class Skuladin : NavigatingMonster
{
	[Header("Skuladin")]
	[SerializeField]
	protected float attackCooldownTime = 2f;

	[SerializeField]
	protected float runDistnace = 9f;

	[SerializeField]
	protected float cantSeeDelayBeforeGoingHome = 4f;

	protected float lastAttackTime;

	protected Vector3 startPosition;

	protected bool goingHome;

	protected float lastSeenTime;

	protected override void monster_Start()
	{
		startPosition = base.transform.position;
		if (PlayerCharacter.Instanced)
		{
			base.locomotionTarget = base.playerOrBaitWorldPosition;
		}
	}

	protected override void onAggro()
	{
		lastSeenTime = Time.time;
	}

	protected override void LocomotingBehaviourUpdate()
	{
		if (!PlayerCharacter.Instanced)
		{
			return;
		}
		if (canSeePlayerOrBaitCheck())
		{
			cachedAnimator.SetBool("los", true);
			lastSeenTime = Time.time;
		}
		else
		{
			cachedAnimator.SetBool("los", false);
		}
		cachedAnimator.SetBool("chaos", Random.value > 0.5f);
		if (Time.time <= lastSeenTime + cantSeeDelayBeforeGoingHome)
		{
			base.locomotionTarget = base.playerOrBaitWorldPosition;
		}
		else
		{
			base.locomotionTarget = startPosition;
			if (Vector3.Distance(startPosition, base.transform.position) < 1f)
			{
				base.monsterAggroed = false;
			}
		}
		base.facingTarget = base.transform.position + base.desiredDirection;
	}
}
