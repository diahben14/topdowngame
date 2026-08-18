using UnityEngine;

public class SewerTentacle : NavigatingMonster
{
	[SerializeField]
	protected float attackDistance = 6f;

	protected override void LocomotingBehaviourUpdate()
	{
		if (base.IsAggroed)
		{
			Vector3 vector = base.playerOrBaitWorldPosition - base.transform.position;
			Monster closestMonster = MonsterManager.GetClosestMonster(base.transform.position, this);
			if ((closestMonster.transform.position - base.transform.position).sqrMagnitude < vector.sqrMagnitude)
			{
				base.facingTarget = closestMonster.transform.position;
			}
			else
			{
				base.facingTarget = base.playerOrBaitWorldPosition;
			}
			Vector3 vector2 = base.facingTarget - base.transform.position;
			cachedAnimator.SetBool("playerClose", vector2.magnitude < attackDistance);
		}
	}
}
