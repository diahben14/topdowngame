using UnityEngine;

public class SetBoolOnEnterStateBehaviour : StateMachineBehaviour
{
	[SerializeField]
	protected string boolName = "trigger";

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Debug.LogWarning("This is old tech -- use SetBoolStateMachineBehaviour instead");
		animator.SetBool(boolName, true);
	}
}
