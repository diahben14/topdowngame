using UnityEngine;

public class AnimationParameterPasson : MonoBehaviour
{
	[SerializeField]
	protected Animator sourceAnimator;

	[SerializeField]
	protected Animator destAnimator;

	[SerializeField]
	protected string sourceParameterName = "AP_parameter";

	[SerializeField]
	protected string destParameterName = "AP_parameter";

	private void LateUpdate()
	{
		destAnimator.SetBool(destParameterName, sourceAnimator.GetFloat(sourceParameterName) > 0f);
	}
}
