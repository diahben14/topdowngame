using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStateBehaviour : StateMachineBehaviour
{
	[Tooltip("Busy means other actions can't happen while this is playing. If true, something other than player input must take you out of this state. (exit time, etc.)")]
	[SerializeField]
	protected float busyNormalizedDuration;

	[SerializeField]
	protected float noDodgeNormalizedDuration = 1f;

	[SerializeField]
	protected bool allowItemInputs = true;

	[SerializeField]
	protected float noItemsNormalizedDuration;

	[SerializeField]
	protected FacingLockType lockFacingWhilePlaying;

	[SerializeField]
	protected float turnSpeedAttenuationWhilePlaying = 1f;

	[SerializeField]
	protected GroundingTechnique groundingTechnique;

	[FormerlySerializedAs("rechargeStaminaWhilePlaying")]
	[SerializeField]
	protected bool rechargeStaminaWhilePlayingFast;

	[SerializeField]
	protected bool rechargeStaminaWhilePlayingSlow;

	[SerializeField]
	protected StaminaCostType staminaCostType;

	[SerializeField]
	protected bool applyGlobalDodgeCost;

	[SerializeField]
	protected float staminaCostOnEntry;

	[SerializeField]
	protected float staminaCostPerSecond;

	[SerializeField]
	protected bool instantlyFaceMoveStickDirectionOnEnter;

	[SerializeField]
	protected bool instantlyFaceDesiredLookDirectionOnEnter;

	[SerializeField]
	protected bool instantlyFaceDesiredLookDirectionOnExit;

	[SerializeField]
	protected bool alwaysFlinchWhenHit;

	public string debugName;

	protected PlayerCharacter playerCharacter;

	protected Rigidbody playerRigidbody;

	protected bool playing;

	protected float normalizedTime;

	protected const float cliffTestRaycastDistance = 1.05f;

	protected const int cliffTestLookaheadFrames = 1;

	public bool Busy
	{
		get
		{
			if (playing)
			{
				return normalizedTime < busyNormalizedDuration;
			}
			return false;
		}
	}

	public bool ItemInputsAllowed
	{
		get
		{
			if (playing)
			{
				if (allowItemInputs)
				{
					return normalizedTime >= noItemsNormalizedDuration;
				}
				return false;
			}
			return true;
		}
	}

	public bool AlwaysFlinchWhenHit
	{
		get
		{
			if (playing)
			{
				return alwaysFlinchWhenHit;
			}
			return false;
		}
	}

	public bool DodgeAllowed
	{
		get
		{
			if (playing)
			{
				return normalizedTime >= noDodgeNormalizedDuration;
			}
			return true;
		}
	}

	public FacingLockType LockingFacing
	{
		get
		{
			if (!playing)
			{
				return FacingLockType.NORMAL;
			}
			return lockFacingWhilePlaying;
		}
	}

	public float TurnSpeedAttenuation
	{
		get
		{
			if (!playing || lockFacingWhilePlaying != FacingLockType.SLOW_TURNING)
			{
				return 1f;
			}
			return turnSpeedAttenuationWhilePlaying;
		}
	}

	public GroundingTechnique NavMeshSnapping
	{
		get
		{
			if (playing)
			{
				return groundingTechnique;
			}
			return GroundingTechnique.NAVMESH;
		}
	}

	public bool RechargeStaminaFast
	{
		get
		{
			if (playing)
			{
				return rechargeStaminaWhilePlayingFast;
			}
			return false;
		}
	}

	public bool RechargeStaminaSlow
	{
		get
		{
			if (playing)
			{
				return rechargeStaminaWhilePlayingSlow;
			}
			return false;
		}
	}

	protected bool thisStateCostsStamina
	{
		get
		{
			if (staminaCostType != StaminaCostType.DODGE)
			{
				if (staminaCostType == StaminaCostType.ATTACK)
				{
					return StaminaSettings.AttacksCostStamina;
				}
				return false;
			}
			return true;
		}
	}

	public void Reset()
	{
		playing = false;
		normalizedTime = 0f;
	}

	public void Setup(PlayerCharacter pc, Rigidbody rb)
	{
		playerCharacter = pc;
		playerRigidbody = rb;
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		playerCharacter.RootMotionHitCancelled = false;
		playing = true;
		normalizedTime = stateInfo.normalizedTime;
		if (thisStateCostsStamina)
		{
			if (applyGlobalDodgeCost)
			{
				PlayerCharacter.Stamina -= StaminaSettings.DodgeStaminaCost;
			}
			else
			{
				PlayerCharacter.Stamina -= staminaCostOnEntry;
			}
		}
		if (instantlyFaceMoveStickDirectionOnEnter)
		{
			playerCharacter.InstantlyFaceMoveVector();
		}
		if (instantlyFaceDesiredLookDirectionOnEnter)
		{
			playerCharacter.InstantlyFaceDesiredLookDirection();
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		normalizedTime = stateInfo.normalizedTime;
		if (staminaCostPerSecond > 0f && thisStateCostsStamina)
		{
			PlayerCharacter.Stamina -= staminaCostPerSecond * Time.deltaTime;
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		playing = false;
		playerCharacter.TurnOffStaminaEffectsIfStaminaDepleated();
		if (instantlyFaceDesiredLookDirectionOnExit)
		{
			playerCharacter.InstantlyFaceDesiredLookDirection();
		}
	}
}
