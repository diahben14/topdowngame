using UnityEngine;

public class CutscenePlayerAnimationTrigger : MonoBehaviour
{
	[SerializeField]
	protected Transform magnetizeTo;

	private void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<PlayerCharacter>() != null)
		{
			PlayerCharacter.CutsceneHeadLook = false;
			PlayerCharacter.AnimationSetTrigger("CUTSCENE_get sword");
			PlayerCharacter.MagnetizeToPositionAndFacing(magnetizeTo.position, magnetizeTo.rotation);
		}
	}
}
