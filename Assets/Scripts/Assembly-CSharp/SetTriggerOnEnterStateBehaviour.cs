using UnityEngine;

public class SetTriggerOnEnterStateBehaviour : StateMachineBehaviour
{
	[SerializeField]
	protected string triggerName = "trigger";

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetTrigger(triggerName);
	}
}
