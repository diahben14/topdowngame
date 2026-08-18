using UnityEngine;

public class CreatureFMODAnimationStateBehaviour : FMODAnimationStateBehaviour
{
	[SerializeField]
	protected bool playOnDryLand = true;

	[SerializeField]
	protected bool playInWater;

	[SerializeField]
	protected float velocityThreshold;

	[SerializeField]
	protected bool mapVelocityToParameter;

	[SerializeField]
	protected float velocityMapMaxiumum;

	[SerializeField]
	protected string velocityMapParameter = "";

	[SerializeField]
	protected float parameterMaxValue = 10f;

	protected Creature cachedCreature;

	protected override bool canPlay
	{
		get
		{
			if (cachedCreature != null)
			{
				return cachedCreature.Velocity.magnitude > velocityThreshold;
			}
			Debug.Log("[CreatureFMODAnimationStateBehaviour] checking velocity threshold but didn't cache a Creature component");
			return false;
		}
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		if (!cachedCreature)
		{
			cachedCreature = animator.GetComponent<Creature>();
		}
	}

	protected override void playEffect(int i, Vector3 pos)
	{
		if ((cachedCreature.InWater && playInWater) || (!cachedCreature.InWater && playOnDryLand))
		{
			if (!mapVelocityToParameter)
			{
				SFX.PlayAudioClipHere(events[i].fmodEvent, pos);
				return;
			}
			float paramValue = Mathf.Clamp01(Mathf.InverseLerp(velocityThreshold, velocityMapMaxiumum, cachedCreature.Velocity.magnitude)) * parameterMaxValue;
			SFX.playAudioClipWithParam(events[i].fmodEvent, pos, velocityMapParameter, paramValue);
		}
	}
}
