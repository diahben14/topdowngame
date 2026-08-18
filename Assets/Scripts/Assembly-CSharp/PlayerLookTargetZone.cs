using UnityEngine;

public class PlayerLookTargetZone : PlayerTriggerZone<PlayerLookTargetZone>
{
	[SerializeField]
	protected Transform lookTarget;

	protected override void onChange()
	{
		if (base.PlayerInZone)
		{
			PlayerCharacter.CutsceneHeadLook = true;
			PlayerCharacter.CutsceneHeadLookPosition = lookTarget.position;
		}
		else
		{
			PlayerCharacter.CutsceneHeadLook = false;
		}
	}
}
