using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

public class PlaySFXOnEnterAnimatorState : StateMachineBehaviour
{
	[FormerlySerializedAs("fmodEvent")]
	[SerializeField]
	[EventRef]
	protected string onStateEnterEvent;

	[SerializeField]
	[EventRef]
	protected string onStateExitEvent;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!string.IsNullOrEmpty(onStateEnterEvent))
		{
			SFX.PlayAudioClipHere(onStateEnterEvent, animator.transform.position);
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!string.IsNullOrEmpty(onStateExitEvent))
		{
			SFX.PlayAudioClipHere(onStateExitEvent, animator.transform.position);
		}
	}
}
