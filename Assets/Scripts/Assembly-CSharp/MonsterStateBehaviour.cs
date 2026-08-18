using UnityEngine;

public class MonsterStateBehaviour : StateMachineBehaviour
{
	[SerializeField]
	protected bool busyWhilePlaying;

	[SerializeField]
	protected bool suppressNavParamUpdatesWhenPlaying;

	protected bool playing;

	public bool Busy
	{
		get
		{
			if (busyWhilePlaying)
			{
				return playing;
			}
			return false;
		}
	}

	public bool SuppressNavWhenPlaying
	{
		get
		{
			if (suppressNavParamUpdatesWhenPlaying)
			{
				return playing;
			}
			return false;
		}
	}

	public void Reset()
	{
		playing = false;
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		playing = true;
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		playing = false;
	}
}
