using UnityEngine;

public class SetBoolStateMachineBehaviour : StateMachineBehaviour
{
	[SerializeField]
	protected string boolToSet = "bool";

	[SerializeField]
	protected bool setTriggerOnEnter = true;

	[SerializeField]
	protected bool setTriggerOnExit;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (setTriggerOnEnter)
		{
			animator.SetBool(boolToSet, true);
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (setTriggerOnExit)
		{
			animator.SetBool(boolToSet, true);
		}
	}
}
