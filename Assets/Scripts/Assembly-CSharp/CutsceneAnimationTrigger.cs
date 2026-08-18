using UnityEngine;

public class CutsceneAnimationTrigger : MonoBehaviour
{
	[SerializeField]
	protected Animator animator;

	[SerializeField]
	protected string triggerName = "go";

	private void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<PlayerCharacter>() != null)
		{
			animator.SetTrigger(triggerName);
		}
	}
}
