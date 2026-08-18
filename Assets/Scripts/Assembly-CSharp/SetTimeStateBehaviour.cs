using UnityEngine;

public class SetTimeStateBehaviour : StateMachineBehaviour
{
	[SerializeField]
	protected string timeParamter = "time";

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetFloat(timeParamter, Time.time);
	}
}
