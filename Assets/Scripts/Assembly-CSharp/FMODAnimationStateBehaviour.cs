using UnityEngine;

public class FMODAnimationStateBehaviour : StateMachineBehaviour
{
	[SerializeField]
	protected bool playAtAnimatorsPosition = true;

	[SerializeField]
	protected FMODAnimationStateBehaviourEntry[] events = new FMODAnimationStateBehaviourEntry[1]
	{
		new FMODAnimationStateBehaviourEntry()
	};

	protected float lastNormalizedTime;

	protected virtual bool canPlay
	{
		get
		{
			return true;
		}
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		lastNormalizedTime = getNormalizedTime(stateInfo);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		float normalizedTime = getNormalizedTime(stateInfo);
		if (canPlay)
		{
			for (int i = 0; i < events.Length; i++)
			{
				float normalizedTime2 = events[i].normalizedTime;
				if (stateInfo.speedMultiplier > 0f)
				{
					if (normalizedTime2 >= lastNormalizedTime && normalizedTime2 <= normalizedTime)
					{
						playEffect(i, animator.transform.position);
					}
				}
				else if (stateInfo.speedMultiplier < 0f && normalizedTime2 <= lastNormalizedTime && normalizedTime2 >= normalizedTime)
				{
					playEffect(i, animator.transform.position);
				}
			}
		}
		lastNormalizedTime = normalizedTime;
	}

	protected float getNormalizedTime(AnimatorStateInfo stateInfo)
	{
		float num = 0f;
		if (stateInfo.loop)
		{
			return Mathf.Repeat(stateInfo.normalizedTime, 1f);
		}
		if (stateInfo.normalizedTime > 1f)
		{
			return 1f;
		}
		return stateInfo.normalizedTime;
	}

	protected virtual void playEffect(int i, Vector3 pos)
	{
		if (playAtAnimatorsPosition)
		{
			SFX.PlayAudioClipHere(events[i].fmodEvent, pos);
		}
		else
		{
			SFX.PlayAudioClip(events[i].fmodEvent);
		}
	}
}
