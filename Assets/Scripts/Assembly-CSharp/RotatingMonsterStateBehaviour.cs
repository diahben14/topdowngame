using UnityEngine;

public class RotatingMonsterStateBehaviour : StateMachineBehaviour
{
	[SerializeField]
	protected float normalizedTimeToRotate;

	[SerializeField]
	protected float degreeIncrement = 30f;

	protected bool ready;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		ready = true;
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		float @float = animator.GetFloat("right");
		float float2 = animator.GetFloat("forward");
		float num = (float)Mathf.RoundToInt(Geometry.Vector2Angle(new Vector3(@float, 0f, float2).normalized) / degreeIncrement) * degreeIncrement;
		if (ready && stateInfo.normalizedTime > normalizedTimeToRotate)
		{
			animator.transform.Rotate(Vector3.up, 0f - num);
			ready = false;
		}
	}
}
