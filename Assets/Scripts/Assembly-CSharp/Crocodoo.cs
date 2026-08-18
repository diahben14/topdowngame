public class Crocodoo : NavigatingMonster
{
	protected override void LocomotingBehaviourUpdate()
	{
		base.locomotionTarget = base.playerOrBaitWorldPosition;
		base.facingTarget = base.transform.position + base.desiredDirection;
	}
}
