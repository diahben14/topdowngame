using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[SelectionBase]
[RequireComponent(typeof(HitReceiver))]
public class PlayerCharacter : Creature
{
	public delegate void telegraphCallbackType(float distance);

	public enum StatState
	{
		NORMAL = 0,
		SUPRESSED = 1,
		HERO = 2
	}

	[Header("PlayerCharacter")]
	protected Dat normalTurningDuration;

	protected Dat defaultStartingStamina;

	protected Dat noStaminaDamageAttenuation;

	protected Dat magnetizeDuration;

	protected Dat climbSpeed;

	protected Dat coinDropScaler;

	protected Dat coinDropFalloffRate;

	protected Dat coinDropMin;

	protected Dat coinDropMax;

	protected Dat HPPerLevelUpItem;

	protected Dat SPPerLevelUpItem;

	protected Dat[] mpDefaultStartingAmount = new Dat[3];

	protected Dat[] mpPerLevelUpItem = new Dat[3];

	protected Dat[] mpRechargeRate = new Dat[3];

	protected Dat[] mpRechargeDelayOnUse = new Dat[3];

	protected Dat[] mpRechargeDelayOnDeplete = new Dat[3];

	protected Dat poiseFractionOfMaxHP;

	protected Dat potionRecoverBase;

	[Header("PlayerCharacter - SFX")]
	[SerializeField]
	[EventRef]
	protected string shieldDownSFX;

	[SerializeField]
	[EventRef]
	protected string blockSFX;

	[SerializeField]
	[EventRef]
	protected string blockOutOfStaminaSFX;

	[Header("FX")]
	[SerializeField]
	protected ParticleSystem staminaParticleSystem;

	[SerializeField]
	protected ParticleSystem damageBoostParticleSystem;

	[SerializeField]
	protected ParticleSystem blockParticleSystem;

	[SerializeField]
	protected Animator hyperdashEffectAnimator;

	[SerializeField]
	protected Animator hyperdashTerminationEffectAnimator;

	[SerializeField]
	protected ParticleSystem hyperdashParticleSystem;

	[Header("PlayerCharacter - Debug")]
	[SerializeField]
	protected bool unlimitedAmmo;

	[Header("PlayerCharacter - StateVariables/Gear")]
	[SerializeField]
	protected Item shieldItem;

	[SerializeField]
	protected Item lanternItem;

	[SerializeField]
	protected Light lanternLight;

	[SerializeField]
	protected Item hyperdashItem;

	[SerializeField]
	protected Item moneyLevelItem;

	[SerializeField]
	protected ButtonAssignableItem potionItem;

	[SerializeField]
	protected StateVariable hasUsedMP;

	[SerializeField]
	protected StateVariable hasEverDiedStateVariable;

	[Header("PlayerCharacter - Koban Items")]
	[SerializeField]
	protected Item healthUpgradeItem;

	[SerializeField]
	protected Item staminaUpgradeItem;

	[SerializeField]
	protected Item[] mpUpgradeItem;

	[SerializeField]
	protected Item attackUpgradeItem;

	[SerializeField]
	protected Item damageResistUpgradeItem;

	[SerializeField]
	protected Item potionRecoverUpgradeItem;

	[SerializeField]
	protected Item heroAttackItem;

	[SerializeField]
	protected Item heroDefenseItem;

	[SerializeField]
	protected Item heroFlaskItem;

	[SerializeField]
	protected Item heroHPItem;

	[SerializeField]
	protected Item heroSPItem;

	[SerializeField]
	protected Item heroMPItem;

	[Header("Conduit Travel")]
	[SerializeField]
	protected GameObject[] foxModeGameobjects;

	[SerializeField]
	protected GameObject[] conduitModeGameobjects;

	[SerializeField]
	protected Material[] conduitSharedMaterials;

	[Header("Coin Spawn on Death")]
	[SerializeField]
	protected Item moneyItem;

	[Header("PlayerCharacter - Magic")]
	[SerializeField]
	protected float magicInputBufferTimeout = 2f;

	[Header("PlayerCharacter - Misc")]
	[SerializeField]
	protected TrackingBone headTrackingBone;

	[SerializeField]
	protected Collider hyperdashHitTriggerCollider;

	[Header("Hyperdash")]
	[SerializeField]
	protected LayerMask dashLayermask;

	[SerializeField]
	protected float maxDashDistance = 9f;

	[SerializeField]
	protected float blinkdashMaxDuration = 0.16667f;

	[SerializeField]
	protected float wallDistance = 1f;

	[SerializeField]
	protected float groundBuffer = 0.25f;

	[SerializeField]
	protected float maxAltitudeDifference = 1f;

	[SerializeField]
	protected int dashSteps = 16;

	[SerializeField]
	protected float navCheckRadius = 0.25f;

	[Header("PlayerCharacter - HACK")]
	[SerializeField]
	protected SkinnedMeshRenderer[] zeroStaminaFlashHack;

	protected const float rightStickZeroingThreshold = 0.2f;

	protected const float rightStickFlickThreshold = 0.5f;

	protected const string kCharacterPrefabPath = "_Fox";

	protected const float kMoveStickDeadzone = 0.2f;

	protected const float minHoldDashTimeForSprint = 0.25f;

	protected const float openChestAnimationTransitionDuration = 0.5f;

	protected const float navMeshSamplingEpsilon = 0.01f;

	protected const int kMaxMagicInputs = 64;

	public const int kMagicMeterCount = 3;

	protected static readonly float hopCheckHeight = 0.25f;

	protected static readonly float hopCheckReach = 1f;

	protected static readonly float hopCheckMinDrop = 1f;

	protected static readonly float hopCheckMaxDrop = 2f;

	protected static readonly float hopCheckNavMeshSampleDistance = 0.25f;

	protected static readonly float playerCheckNavMeshSampleDistance = 0.125f;

	protected static readonly float kAttackBoostFactor = 1.5f;

	protected static readonly float kRadiationForMaxRadiationShow = 0.5f;

	protected const float kConduitPlatformEnterExitDistance = 3f;

	protected static Item _antiRadiationItem;

	protected static float _radiation;

	protected static float lastFrameRadiationAccumulation;

	public static telegraphCallbackType telegraphCallback;

	protected bool _staminaEffects;

	protected static float animationPauseStartTime = float.NegativeInfinity;

	protected static PlayerCharacter _instance;

	protected Collider cachedCollider;

	protected float turningVelocity;

	protected ZTarget zTarget;

	protected ZTarget previewZTarget;

	protected bool lowerDistanceIfZTargetGoesNull;

	protected PlayerStateBehaviour[] stateMachineBehaviours;

	protected bool rightStickHasZeroed;

	protected bool hasNotMovedSinceLastDashInputDown;

	protected ItemBehaviour[] itemBehaviours;

	protected bool usedItemLastFrame;

	protected bool dashInputDownLastFrame;

	protected float damageBoostTime;

	protected bool clickStickTargetting;

	protected float dashInputHeldTimer;

	protected bool animatorRequestedInstantFacing;

	protected ParticleSystem.EmissionModule staminaParticleSystemEmission;

	protected ParticleSystem.EmissionModule damageBoostParticleSystemEmission;

	protected ParticleSystem.EmissionModule hyperdashParticleEmitter;

	protected Ladder currentLadder;

	protected float ladderProgressParam;

	protected bool continuousMovementSinceLastNodeSwitch;

	protected float conduitParameter;

	protected float lastNormalizedParamVelocity;

	protected float timeOfLastMagicInput;

	protected float[] lastMPUseTime = new float[3];

	protected float[] lastMPDepleatTime = new float[3];

	protected float lastDashTime;

	protected HitReceiver cachedHitReceiver;

	protected int iFrameLayer;

	protected int playerLayer;

	protected int inConduitLayer;

	protected Vector3 magnetPositionEnd;

	protected Vector3 magnetPositionStart;

	protected Quaternion magnetRotationStart;

	protected Quaternion magnetRotationEnd;

	protected bool magnetizing;

	protected float magnetParameter;

	protected Switch rememberedSwitchToActivate;

	protected Chest rememberedChest;

	protected float staminaLayerWeightVelocity;

	protected static LayerMask hopCheckLayerMask;

	protected float lastWaveTime;

	protected bool playerIsSprinting;

	protected Vector3 hyperdashDestination;

	protected float hyperdashTransitDuration;

	protected float hyperdashFullDuration;

	protected bool doingHyperdash;

	protected bool partialHyperdash;

	protected bool hyperdashTerminationEffectsTriggered;

	protected Vector3 hyperdashStartPos;

	protected float lastHyperdashTime;

	private bool[] itemBehaviourInputDowns = new bool[4];

	public static bool[] itemBehaviourInputUps = new bool[4];

	protected int wadingAnimatorLayerIndex;

	protected float wadingAnimationLayerWeight;

	protected bool _cutsceneHidden;

	protected bool _hyperdashHidden;

	protected float[] mp = new float[3];

	protected bool menuNextFrame;

	protected ItemBehaviour lastUsedItemBehaviour;

	protected static int _conduitAcceleratorCount;

	protected const float kSteep = 2f;

	public static bool DebugInvincible { get; set; }

	public static bool Instanced
	{
		get
		{
			return instance != null;
		}
	}

	public static bool CutsceneHidden
	{
		get
		{
			return instance.cutsceneHidden;
		}
		set
		{
			if (Instanced)
			{
				instance.cutsceneHidden = value;
			}
			else
			{
				Debug.LogError("[Player Character] Can't hide fox -- no instance.");
			}
		}
	}

	public static int HP
	{
		get
		{
			if (!instance)
			{
				return 0;
			}
			return instance.hp;
		}
		set
		{
			if ((bool)instance)
			{
				instance.hp = value;
			}
		}
	}

	public static int Poison
	{
		get
		{
			if (!instance)
			{
				return 0;
			}
			return (int)Mathf.Ceil(instance.poisonCounter);
		}
	}

	protected static bool antiRadiationItemEquipped
	{
		get
		{
			return antiRadiationItem.Quantity > 0;
		}
	}

	protected static Item antiRadiationItem
	{
		get
		{
			if (_antiRadiationItem == null)
			{
				_antiRadiationItem = Inventory.GetItemByName("AntiRadiationItem");
			}
			return _antiRadiationItem;
		}
	}

	protected static float radiation
	{
		get
		{
			return _radiation;
		}
		set
		{
			if (value > _radiation)
			{
				lastFrameRadiationAccumulation = value - _radiation;
			}
			_radiation = Mathf.Min(value, instance.actualMaxHP);
		}
	}

	public static float RadiationShowAmount
	{
		get
		{
			return lastFrameRadiationAccumulation / kRadiationForMaxRadiationShow;
		}
	}

	public static int MaxHP
	{
		get
		{
			if (!instance)
			{
				return 0;
			}
			return instance.maxhp;
		}
	}

	public static bool AbleToAct
	{
		get
		{
			return instance.ableToAct;
		}
	}

	public static float Stamina
	{
		get
		{
			if (!instance)
			{
				return 0f;
			}
			return instance.stamina;
		}
		set
		{
			if (Instanced)
			{
				instance.stamina = Mathf.Max(0f, value);
			}
			else
			{
				showInstanceError("PlayerCharacter.Stamina");
			}
		}
	}

	public static float MaxStamina
	{
		get
		{
			if (!instance)
			{
				return 0f;
			}
			return instance.maxStamina;
		}
	}

	public static bool ShieldUp { get; private set; }

	public static bool IsFrozen
	{
		get
		{
			if (instance != null)
			{
				return instance.Frozen;
			}
			return false;
		}
	}

	public static float LastUnfreezeTime { get; private set; }

	public static float AttackPower
	{
		get
		{
			int num = instance.attackUpgradeItem.Quantity + 1;
			StatState statState_Att = StatState_Att;
			if (instance.damageBoostTime > 0f)
			{
				num++;
			}
			switch (statState_Att)
			{
			case StatState.HERO:
				return num + 1;
			case StatState.SUPRESSED:
				return 1f;
			default:
				return num;
			}
		}
	}

	public static float DefensePower
	{
		get
		{
			int num = instance.damageResistUpgradeItem.Quantity + 1;
			switch (StatState_Def)
			{
			case StatState.HERO:
				return num + 1;
			case StatState.SUPRESSED:
				return 1f;
			default:
				return num;
			}
		}
	}

	public static bool InstanceIsDead
	{
		get
		{
			if (!Instanced)
			{
				return false;
			}
			return instance.IsDead;
		}
	}

	public static bool InstanceInConduit
	{
		get
		{
			if (!Instanced)
			{
				return false;
			}
			return instance.InConduit;
		}
	}

	public static Vector3 LaterialVelocity
	{
		get
		{
			if ((bool)instance)
			{
				return Vector3.Scale(instance.cachedRigidbody.velocity, new Vector3(1f, 0f, 1f));
			}
			return Vector3.zero;
		}
	}

	public static Transform Transform
	{
		get
		{
			if ((bool)instance)
			{
				return instance.transform;
			}
			return null;
		}
	}

	public static Vector3? WorldPositionOrNull
	{
		get
		{
			if ((bool)instance)
			{
				return instance.transform.position;
			}
			return null;
		}
	}

	public static Vector3 WorldPosition
	{
		get
		{
			if ((bool)instance)
			{
				return instance.transform.position;
			}
			showInstanceError("PlayerCharacter.WorldPosition");
			return Vector3.zero;
		}
	}

	public static Vector3? ForwardOrNull
	{
		get
		{
			if ((bool)instance)
			{
				return instance.transform.forward;
			}
			return null;
		}
	}

	public static Vector3 Forward
	{
		get
		{
			if ((bool)instance)
			{
				return instance.transform.forward;
			}
			showInstanceError("PlayerCharacter.ForwardOrNull");
			return Vector3.zero;
		}
	}

	public new static Vector3 Velocity
	{
		get
		{
			if ((bool)instance)
			{
				return instance.cachedRigidbody.velocity;
			}
			return Vector3.zero;
		}
	}

	public static Vector3 DashDirection { get; set; }

	public static Vector3 ProjectedMoveVector { get; private set; }

	public static Vector3? ZTargetWorldPositionOrNull
	{
		get
		{
			if (Instanced)
			{
				if ((bool)instance.zTarget)
				{
					return instance.zTarget.WorldPosition;
				}
				return null;
			}
			return null;
		}
	}

	public static Vector3? PreviewZTargetWorldPositionOrNull
	{
		get
		{
			if (Instanced && !InstanceIsDead)
			{
				if ((bool)instance.previewZTarget)
				{
					return instance.previewZTarget.WorldPosition;
				}
				return null;
			}
			return null;
		}
	}

	public static float? ZTargetHealthFactor
	{
		get
		{
			if (Instanced && instance.zTarget != null && !instance.zTarget.HideHPBar)
			{
				return instance.zTarget.NormalizedHealth;
			}
			return null;
		}
	}

	public static float? ZTargetPoiseFactor
	{
		get
		{
			if (Instanced)
			{
				return instance.zTarget.NormalizedPoise;
			}
			return null;
		}
	}

	public static Vector3 ZTargetVelocity
	{
		get
		{
			if (Instanced && instance.zTarget != null)
			{
				Rigidbody component = instance.zTarget.GetComponent<Rigidbody>();
				if ((bool)component)
				{
					return component.velocity;
				}
			}
			return Vector3.zero;
		}
	}

	public static bool StrafeTargetting { get; set; }

	public static bool CurrentlyFlashedOn
	{
		get
		{
			if ((bool)instance)
			{
				return instance.materialManager.CurrentlyFlashedOn;
			}
			return false;
		}
	}

	public static bool FoxInWater
	{
		get
		{
			if ((bool)instance)
			{
				return instance.InWater;
			}
			return false;
		}
	}

	public static bool CutsceneHeadLook { get; set; }

	public static Vector3 CutsceneHeadLookPosition { get; set; }

	public static bool DoingHyperdash
	{
		get
		{
			return instance.doingHyperdash;
		}
	}

	public static float SlowdownEffect { get; set; }

	public static StatState StatState_HP
	{
		get
		{
			if (instance.heroHPItem.Quantity <= 0)
			{
				if (!StatSuppressionField.IsActive)
				{
					return StatState.NORMAL;
				}
				return StatState.SUPRESSED;
			}
			return StatState.HERO;
		}
	}

	public static StatState StatState_SP
	{
		get
		{
			if (instance.heroSPItem.Quantity <= 0)
			{
				if (!StatSuppressionField.IsActive)
				{
					return StatState.NORMAL;
				}
				return StatState.SUPRESSED;
			}
			return StatState.HERO;
		}
	}

	public static StatState StatState_MP
	{
		get
		{
			if (instance.heroMPItem.Quantity <= 0)
			{
				if (!StatSuppressionField.IsActive)
				{
					return StatState.NORMAL;
				}
				return StatState.SUPRESSED;
			}
			return StatState.HERO;
		}
	}

	public static StatState StatState_Att
	{
		get
		{
			if (instance.heroAttackItem.Quantity <= 0)
			{
				if (!StatSuppressionField.IsActive)
				{
					return StatState.NORMAL;
				}
				return StatState.SUPRESSED;
			}
			return StatState.HERO;
		}
	}

	public static StatState StatState_Def
	{
		get
		{
			if (instance.heroDefenseItem.Quantity <= 0)
			{
				if (!StatSuppressionField.IsActive)
				{
					return StatState.NORMAL;
				}
				return StatState.SUPRESSED;
			}
			return StatState.HERO;
		}
	}

	public static StatState StatState_Flask
	{
		get
		{
			if (instance.heroFlaskItem.Quantity <= 0)
			{
				if (!StatSuppressionField.IsActive)
				{
					return StatState.NORMAL;
				}
				return StatState.SUPRESSED;
			}
			return StatState.HERO;
		}
	}

	public override float DamageFactor
	{
		get
		{
			float num = 1f;
			if (StaminaSettings.AttackRequirements == StaminaSettings.StaminaRequirementMode.WEAK_WHEN_STAMINA_IS_ZERO && !StaminaEffects)
			{
				num *= noStaminaDamageAttenuation.Value;
			}
			return num;
		}
	}

	public override int PoisePowerAdjust
	{
		get
		{
			if (StaminaSettings.AttackRequirements == StaminaSettings.StaminaRequirementMode.WEAK_WHEN_STAMINA_IS_ZERO)
			{
				if (!StaminaEffects)
				{
					return -1;
				}
				return 0;
			}
			return 0;
		}
	}

	public override float HitForceFactor
	{
		get
		{
			if (StaminaEffects || StaminaSettings.AttackRequirements != StaminaSettings.StaminaRequirementMode.WEAK_WHEN_STAMINA_IS_ZERO)
			{
				return 1f;
			}
			return noStaminaDamageAttenuation.Value;
		}
	}

	public bool RootMotionHitCancelled { get; set; }

	public override bool IFrameInvincible
	{
		get
		{
			if (base.IFrameInvincible)
			{
				if (StaminaSettings.DodgeRequirements == StaminaSettings.StaminaRequirementMode.WEAK_WHEN_STAMINA_IS_ZERO)
				{
					return Stamina > 0f;
				}
				return true;
			}
			return false;
		}
	}

	public bool StaminaEffects
	{
		get
		{
			return _staminaEffects;
		}
		protected set
		{
			_staminaEffects = value;
		}
	}

	protected static PlayerCharacter instance
	{
		get
		{
			return _instance;
		}
		set
		{
			if (_instance == null || value == null)
			{
				_instance = value;
			}
			else if (_instance != value)
			{
				Debug.LogWarning("Already a registered PlayerCharacter... Killing newcomer.");
				UnityEngine.Object.Destroy(value.gameObject);
			}
		}
	}

	protected static PlayerCharacter onDemandInstance
	{
		get
		{
			if (_instance == null)
			{
				PlayerCharacter component = UnityEngine.Object.Instantiate(Resources.Load("_Fox") as GameObject).GetComponent<PlayerCharacter>();
				UnityEngine.Object.DontDestroyOnLoad(component);
				_instance = component;
				playerDebug("Making new player on demand", _instance);
			}
			return _instance;
		}
	}

	protected override float hurtInvincibilityDuration
	{
		get
		{
			return 0.5f;
		}
	}

	protected override float blockInvincibilityDuration
	{
		get
		{
			return 0.125f;
		}
	}

	protected bool cutsceneHidden
	{
		get
		{
			return _cutsceneHidden;
		}
		set
		{
			_cutsceneHidden = value;
			updatePlayerVisibility();
			cachedCollider.enabled = !value;
			if (value)
			{
				cachedRigidbody.velocity = Vector3.zero;
			}
		}
	}

	protected bool hyperdashHidden
	{
		get
		{
			return _hyperdashHidden;
		}
		set
		{
			_hyperdashHidden = value;
			updatePlayerVisibility();
		}
	}

	protected FacingLockType facingLock
	{
		get
		{
			for (int i = 0; i < stateMachineBehaviours.Length; i++)
			{
				if (stateMachineBehaviours[i].LockingFacing != 0)
				{
					return stateMachineBehaviours[i].LockingFacing;
				}
			}
			return FacingLockType.NORMAL;
		}
	}

	protected bool busy
	{
		get
		{
			for (int i = 0; i < stateMachineBehaviours.Length; i++)
			{
				if (stateMachineBehaviours[i].Busy)
				{
					return true;
				}
			}
			return false;
		}
	}

	protected bool animatorStateAlwaysFlinch
	{
		get
		{
			for (int i = 0; i < stateMachineBehaviours.Length; i++)
			{
				if (stateMachineBehaviours[i].AlwaysFlinchWhenHit)
				{
					return true;
				}
			}
			return false;
		}
	}

	protected bool ableToAct
	{
		get
		{
			if (!busy && !base.Frozen)
			{
				return !magnetizing;
			}
			return false;
		}
	}

	protected bool animatorStateAllowingDodge
	{
		get
		{
			for (int i = 0; i < stateMachineBehaviours.Length; i++)
			{
				if (!stateMachineBehaviours[i].DodgeAllowed)
				{
					return false;
				}
			}
			return true;
		}
	}

	protected bool animatorStateAllowingItemUse
	{
		get
		{
			for (int i = 0; i < stateMachineBehaviours.Length; i++)
			{
				if (!stateMachineBehaviours[i].ItemInputsAllowed)
				{
					return false;
				}
			}
			return true;
		}
	}

	protected GroundingTechnique groundingTechnique
	{
		get
		{
			for (int i = 0; i < stateMachineBehaviours.Length; i++)
			{
				if (stateMachineBehaviours[i].NavMeshSnapping == GroundingTechnique.NONE)
				{
					return GroundingTechnique.NONE;
				}
			}
			for (int j = 0; j < stateMachineBehaviours.Length; j++)
			{
				if (stateMachineBehaviours[j].NavMeshSnapping == GroundingTechnique.GRAVITY)
				{
					return GroundingTechnique.GRAVITY;
				}
			}
			for (int k = 0; k < stateMachineBehaviours.Length; k++)
			{
				if (stateMachineBehaviours[k].NavMeshSnapping == GroundingTechnique.NAVMESH_PERMISSIVE)
				{
					return GroundingTechnique.NAVMESH_PERMISSIVE;
				}
			}
			return GroundingTechnique.NAVMESH;
		}
	}

	protected float TurnSpeedAttenuation
	{
		get
		{
			float num = 1f;
			for (int i = 0; i < stateMachineBehaviours.Length; i++)
			{
				num *= stateMachineBehaviours[i].TurnSpeedAttenuation;
			}
			return num;
		}
	}

	protected override int hp
	{
		get
		{
			return Mathf.Max(0, maxhp - _damage);
		}
		set
		{
			if (value < 0 && (zeroHP || (float)value < (float)maxhp * -0.25f) && (_damage > actualMaxHP / 2 || radiation > 0f))
			{
				if (!base.IsDead)
				{
					StartCoroutine(Die());
				}
			}
			else
			{
				_damage = maxhp - Mathf.Clamp(value, 0, maxhp);
			}
		}
	}

	protected override bool alwaysFlinch
	{
		get
		{
			return animatorStateAlwaysFlinch;
		}
	}

	protected bool zeroHP
	{
		get
		{
			return _damage >= maxhp;
		}
	}

	protected float MaxGhostliness
	{
		get
		{
			return 1f - (float)(heroFlaskItem.Quantity + heroAttackItem.Quantity + heroDefenseItem.Quantity) / 3f;
		}
	}

	protected override int maxhp
	{
		get
		{
			return actualMaxHP - Mathf.RoundToInt(radiation);
		}
	}

	protected int actualMaxHP
	{
		get
		{
			int num = healthUpgradeItem.Quantity;
			switch (StatState_HP)
			{
			case StatState.HERO:
				num++;
				break;
			case StatState.SUPRESSED:
				num = 0;
				break;
			}
			return defaultStartingMaxHP.IntValue + num * HPPerLevelUpItem.IntValue;
		}
	}

	protected override float maxStamina
	{
		get
		{
			int quantity = staminaUpgradeItem.Quantity;
			switch (StatState_SP)
			{
			case StatState.HERO:
				quantity++;
				break;
			case StatState.SUPRESSED:
				quantity = 0;
				break;
			}
			return defaultStartingStamina.Value;
		}
	}

	protected override int maxPoise
	{
		get
		{
			return (int)((float)MaxHP * StaminaSettings.PoiseFractionOfMaxHP);
		}
	}

	protected override float creature_AnimatorSpeed
	{
		get
		{
			return 1f;
		}
	}

	public static int ConduitAcceleratorCount
	{
		get
		{
			return _conduitAcceleratorCount;
		}
		set
		{
			_conduitAcceleratorCount = value;
		}
	}

	protected override float SlowInWaterFactor
	{
		get
		{
			return 0.77f;
		}
	}

	protected override float navMeshMinSampleDistance
	{
		get
		{
			return 2f;
		}
	}

	protected override float navMeshMaxSampleDistance
	{
		get
		{
			return 2f;
		}
	}

	public static void Restore()
	{
		if ((bool)instance)
		{
			radiation = 0f;
			instance.hp = instance.maxhp;
			instance.stamina = instance.maxStamina;
			instance.refillMPToMax();
		}
	}

	public static void ApplyRadiation(float amount, bool ignoreAntiRadiationItem)
	{
		if (!antiRadiationItemEquipped || ignoreAntiRadiationItem)
		{
			radiation += amount * Time.deltaTime;
		}
	}

	public static void GiveHP(int hp)
	{
		instance.hp += hp;
	}

	public static void GiveMP(int mp, int type)
	{
		instance.mp[type] = Mathf.Min(instance.mp[type] + (float)mp, instance.getMaxMP(type));
	}

	public static void RebootCharacter()
	{
		onDemandInstance.rebootCharacter();
		ShieldUp = false;
		DashDirection = Vector3.zero;
		ProjectedMoveVector = Vector3.zero;
		StrafeTargetting = false;
	}

	public static float GetMP(int index)
	{
		if (!instance)
		{
			return 0f;
		}
		return instance.mp[index];
	}

	public static void SetMP(int index, float val)
	{
		if (val < instance.mp[index])
		{
			instance.lastMPUseTime[index] = Time.time;
			instance.hasUsedMP.BoolValue = true;
			if (val <= 0f)
			{
				instance.lastMPDepleatTime[index] = Time.time;
			}
		}
		instance.mp[index] = Mathf.Clamp(val, 0f, instance.getMaxMP(index));
	}

	public static int GetMaxMP(int index)
	{
		if (!instance)
		{
			return 0;
		}
		return instance.getMaxMP(index);
	}

	public static void RefillMP()
	{
		instance.refillMPToMax();
	}

	public static void SpawnAt(Transform transform)
	{
		onDemandInstance.transform.position = transform.position;
		onDemandInstance.transform.rotation = transform.rotation;
		onDemandInstance.WaterTriggerCount = 0;
	}

	public static void SetPosition(Vector3 position)
	{
		instance.transform.position = position;
	}

	public static void SetFacing(Vector3 position)
	{
		Vector3 forward = position - Transform.position;
		forward.Scale(Mathx.flatteningVector);
		instance.transform.rotation = Quaternion.LookRotation(forward);
	}

	public static void MagnetizeToPositionAndFacing(Vector3 position, Quaternion facing)
	{
		instance.magnetizeToPositionAndFacing(position, facing);
	}

	public static void AttemptToActivateSwitch(Switch switchToActivate, Transform playerMagnetTransform)
	{
		instance.attemptToActivateSwitch(switchToActivate, playerMagnetTransform);
	}

	public static void EnterRestState(Vector3 position)
	{
		if ((bool)instance)
		{
			instance.cachedAnimator.SetBool("sitting", true);
		}
		else
		{
			Debug.LogError("EnterRestState but no character instance.");
		}
	}

	public static void EnterRestStateInstant(Vector3 position)
	{
		if ((bool)instance)
		{
			instance.cachedAnimator.SetBool("sitting instant", true);
		}
		else
		{
			Debug.LogError("EnterRestStateInstant but no character instance.");
		}
	}

	public static void ExitRestState()
	{
		if ((bool)instance)
		{
			instance.cachedAnimator.SetBool("sitting", false);
			instance.cachedAnimator.SetBool("sitting instant", false);
		}
		else
		{
			Debug.LogError("EnterRestStateInstant but no character instance.");
		}
	}

	public static void AttemptToUseFromMenu(ButtonAssignableItem buttonAssignableItem)
	{
	}

	public static int GetCoinDropFromValue(float value)
	{
		if ((bool)instance)
		{
			float num = Mathf.Clamp(mapDropValue(value, instance.moneyLevelItem.Quantity), instance.coinDropMin.Value, instance.coinDropMax.Value);
			int num2 = Mathf.FloorToInt(num);
			int num3 = num2;
			float num4 = num - (float)num2;
			if (UnityEngine.Random.value < num4)
			{
				num3++;
			}
			return num3;
		}
		return 0;
	}

	private static float mapDropValue(float value, int level)
	{
		return value / Mathf.Pow(instance.coinDropFalloffRate.Value, level) * instance.coinDropScaler.Value;
	}

	public static void FlaskSwig()
	{
		GiveHP(getFlaskRestoreAmount());
	}

	public static int getFlaskRestoreAmount()
	{
		int quantity = instance.potionRecoverUpgradeItem.Quantity;
		switch (StatState_Flask)
		{
		case StatState.HERO:
			quantity++;
			break;
		case StatState.SUPRESSED:
			quantity = 0;
			break;
		}
		return Mathf.RoundToInt(instance.potionRecoverBase.Value);
	}

	public static void AnimationGotoState(string s)
	{
		Debug.LogFormat("PlayerCharacter.SnapToAnimationState(\"{0}\")", s);
		if (Instanced)
		{
			instance.cachedAnimator.CrossFade(s, 0f);
		}
	}

	public static void AnimationSetTrigger(string s)
	{
		Debug.LogFormat("PlayerCharacter.AnimationSetTrigger(\"{0}\")", s);
		if (Instanced)
		{
			instance.cachedAnimator.SetTrigger(s);
		}
	}

	public static void PlayOpenChestAnimation(Chest chest)
	{
		instance.cachedAnimator.CrossFadeInFixedTime("open chest", 0.5f);
		instance.rememberedChest = chest;
	}

	public static void ClimbOnLadder(Ladder ladder, float param, Vector3 snapPosition, Quaternion snapRotation)
	{
		instance.climbOnLadder(ladder, param, snapPosition, snapRotation);
	}

	public static void AddDamageBoostTime(float time)
	{
		if (Instanced)
		{
			instance.damageBoostTime += time;
		}
	}

	public static void CancelHyperdash()
	{
		instance.doingHyperdash = false;
	}

	public static StatState StatStateFromOfferingItem(OfferingItem item)
	{
		return StatState.NORMAL;
	}

	public override void ResetAnimationStates()
	{
		cachedAnimator.Play("Default", 1);
		for (int i = 0; i < stateMachineBehaviours.Length; i++)
		{
			stateMachineBehaviours[i].Reset();
		}
	}

	public void GiveStaminaFromHit()
	{
		base.stamina += 50f;
	}

	public void TurnOffStaminaEffectsIfStaminaDepleated()
	{
		if (Stamina <= 0f)
		{
			StaminaEffects = false;
		}
	}

	public void __switchThrowSuccessEvent()
	{
		rememberedSwitchToActivate.Throw();
	}

	public void InstantlyFaceMoveVector()
	{
		base.transform.rotation = Geometry.Vector2Quaternion(ProjectedMoveVector);
	}

	public void InstantlyFaceDesiredLookDirection()
	{
		animatorRequestedInstantFacing = true;
	}

	public static void DoAnimationPause()
	{
		animationPauseStartTime = Time.time;
	}

	public void __CutsceneAttachSwordToHand()
	{
		GameObject gameObject = GameObject.Find("_Sword Pickup");
		GameObject gameObject2 = GameObject.Find("hand.R");
		Debug.LogFormat("Attaching sword to hand... {0} -> {1}", gameObject.name, gameObject2.name);
		gameObject.transform.parent = gameObject2.transform;
	}

	protected static void showInstanceError(string functionName)
	{
		Debug.LogErrorFormat(functionName + " called but there's no player instance. Use this OrNull version or check against PlayerCharacter.Instanced");
	}

	protected static void playerDebug(string s, params UnityEngine.Object[] p)
	{
		Debug.LogFormat("[Fox] {0}", s, p);
	}

	protected void updatePlayerVisibility()
	{
		bool active = !cutsceneHidden && !hyperdashHidden;
		for (int i = 0; i < foxModeGameobjects.Length; i++)
		{
			foxModeGameobjects[i].SetActive(active);
		}
	}

	protected Vector3 projectOntoYPlane(Vector2 input)
	{
		Vector3 mainCameraRight = CameraController.MainCameraRight;
		mainCameraRight.y = 0f;
		mainCameraRight.Normalize();
		return Vector3.Cross(mainCameraRight, Vector3.up) * input.y + mainCameraRight * input.x;
	}

	protected override void onFlinch()
	{
		if ((bool)rememberedChest)
		{
			rememberedChest.InterruptOpening();
		}
		currentLadder = null;
		cachedAnimator.SetBool("climbing", false);
	}

	protected int getMaxMP(int index)
	{
		int num = mpUpgradeItem[index].Quantity;
		switch (StatState_MP)
		{
		case StatState.HERO:
			num++;
			break;
		case StatState.SUPRESSED:
			num = 0;
			break;
		}
		return mpDefaultStartingAmount[index].IntValue + num * mpPerLevelUpItem[index].IntValue;
	}

	protected override void ReceivedDamageEffects(bool brokePoise, int damagePoints)
	{
		if (damagePoints > 0)
		{
			if (brokePoise)
			{
				TimeManager.FreezeFrame();
				CameraController.AddDamageEffect(1f);
				CameraController.AddFastScreenshake(0.5f);
			}
			else
			{
				CameraController.AddDamageEffect(0.25f);
				CameraController.AddFastScreenshake(0.25f);
			}
		}
		for (int i = 0; i < itemBehaviours.Length; i++)
		{
			itemBehaviours[i].AnimationWasInterrupted();
		}
		magnetizing = false;
	}

	protected override void BlockDamageEffects()
	{
		blockParticleSystem.gameObject.SetActive(false);
		blockParticleSystem.gameObject.SetActive(true);
		CameraController.AddFastScreenshake(1f);
		if (base.stamina == 0f)
		{
			SFX.PlayAudioClipAtFox(blockOutOfStaminaSFX);
		}
		else
		{
			SFX.PlayAudioClipAtFox(blockSFX);
		}
	}

	protected override int modifyDamage(int damage)
	{
		if (Stamina <= 0f)
		{
			return Mathf.RoundToInt((float)damage * StaminaSettings.DamageFactorAtZeroSP);
		}
		return damage;
	}

	protected override int modifyPoiseDamage(int poiseDamage)
	{
		float num = 1f;
		if (Stamina <= 0f && StaminaSettings.ZeroSPPoiseDmgFactor > 1f)
		{
			num = StaminaSettings.ZeroSPPoiseDmgFactor;
		}
		if (StaminaSettings.AlwaysMegaflinchWhenRunning && playerIsSprinting)
		{
			return int.MaxValue;
		}
		return Mathf.RoundToInt((float)poiseDamage * num);
	}

	public override void DoTetherHit()
	{
		if (Tetherable)
		{
			GetComponent<HitReceiver>().ReceiveHit(HitType.WAND, 0, -1f, 0, 0, 0f, 0f, true, false, base.transform.position, 0f, base.transform.position, Quaternion.identity, false, null, null, -1);
		}
	}

	protected override void onUnfreeze()
	{
		LastUnfreezeTime = Time.time;
		Flinch();
	}

	protected override IEnumerator Die()
	{
		base.IsDead = true;
		timeOfDeath = Time.time;
		clearFreezeTime();
		if (string.IsNullOrEmpty(dieSFX))
		{
			SFX.PlayAudioClipHere("event:/main/enemy/shared/death", base.transform.position);
		}
		else
		{
			SFX.PlayAudioClipHere(dieSFX, base.transform.position);
		}
		headTrackingBone.TrackingOn = false;
		zTarget = null;
		clickStickTargetting = false;
		if (rememberedChest != null)
		{
			rememberedChest.InterruptOpening();
		}
		MusicManager.StopImmediate();
		SFX.PlayAudioClip(hurtSFX);
		cachedAnimator.SetTrigger("death");
		cachedAnimator.SetBool("shield up", false);
		yield return new WaitForFixedUpdate();
		TimeManager.CutsceneTimeScale = 0f;
		for (float f = 0f; f < 0.75f; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
		TimeManager.CutsceneTimeScale = 1f;
		GUIMode.ClearStack();
		CoinSpawner.SpawnCoins(SmallMoneyItem.PlayerQuantity, base.transform.position, 5f, 30f, false, true);
		Inventory.OnDeath();
		Profile.SetBloodstain(SceneManager.GetActiveScene().name, base.transform.position);
		if (SceneManager.GetSceneByName("DEMO Tunic Knight").isLoaded)
		{
			Debug.Log("[DEMO HACK] Going to logo scene!");
			yield return new WaitForSeconds(0.1f);
			UnityEngine.Object.Destroy(base.gameObject);
			SceneManager.LoadScene("tunic logobuild");
			yield break;
		}
		yield return new WaitForSeconds(2f);
		for (float f = 0f; f < 1f; f += TimeManager.UnscaledDeltaTime)
		{
			CameraController.CutsceneFade = f;
			yield return 0;
		}
		CameraController.CutsceneFade = 1f;
		yield return 0;
		if (!hasEverDiedStateVariable.BoolValue && !Profile.GetDemoMode())
		{
			hasEverDiedStateVariable.BoolValue = true;
			RebootCharacter();
			PlayerCharacterSpawn.GoTo("Resurrection", "start");
		}
		else
		{
			Campfire.RespawnAtLastCampfire();
		}
	}

	protected void rebootCharacter()
	{
		playerDebug("Rebooting character");
		UnityEngine.Object.Destroy(base.gameObject);
		_instance = null;
	}

	protected void refillMPToMax()
	{
		for (int i = 0; i < 3; i++)
		{
			mp[i] = getMaxMP(i);
		}
	}

	protected ItemBehaviour getItemBehaviourFromItem(ButtonAssignableItem item)
	{
		for (int i = 0; i < itemBehaviours.Length; i++)
		{
			if (itemBehaviours[i].IsItemBehaviourForItem(item))
			{
				return itemBehaviours[i];
			}
		}
		return null;
	}

	private void magnetizeToPositionAndFacing(Vector3 position, Quaternion facing)
	{
		magnetPositionStart = base.transform.position;
		magnetPositionEnd = position;
		magnetRotationStart = base.transform.rotation;
		magnetRotationEnd = facing;
		magnetizing = true;
		magnetParameter = 0f;
	}

	private void attemptToActivateSwitch(Switch switchToActivate, Transform playerMagnetTransform)
	{
		rememberedSwitchToActivate = switchToActivate;
		MagnetizeToPositionAndFacing(playerMagnetTransform.position, playerMagnetTransform.rotation);
		switch (switchToActivate.Type)
		{
		case SwitchType.STONE:
			cachedAnimator.SetTrigger("lift switch");
			break;
		case SwitchType.STONE_BIG:
			cachedAnimator.SetTrigger("lift switch big");
			break;
		case SwitchType.ROPE:
			cachedAnimator.SetTrigger("lift switch");
			break;
		case SwitchType.EXTENDING_BRIDGE:
			cachedAnimator.SetTrigger("lift switch");
			break;
		}
	}

	protected void climbOnLadder(Ladder ladder, float param, Vector3 snapPosition, Quaternion snapRotation)
	{
		currentLadder = ladder;
		if (param == 1f)
		{
			cachedAnimator.SetTrigger("climb top");
		}
		cachedAnimator.SetBool("climbing", true);
		base.transform.position = snapPosition;
		base.transform.rotation = snapRotation;
	}

	protected void ButtonAssignableItemUsed(ButtonAssignableItem buttonAssignableItem, int actionButtonUp, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		ItemBehaviour itemBehaviourFromItem = getItemBehaviourFromItem(buttonAssignableItem);
		if (itemBehaviourFromItem != null)
		{
			itemBehaviourFromItem.StartCoroutine(itemBehaviourFromItem.OnActionButtonDown(buttonAssignableItem, actionButtonUp, itemsUsedThisFrame));
			if (itemBehaviourFromItem.ShouldUsageMakeItemRootVisible)
			{
				lastUsedItemBehaviour = itemBehaviourFromItem;
			}
			else
			{
				lastUsedItemBehaviour = null;
			}
		}
		else
		{
			InteractionTrigger best = ProximityRegistered<InteractionTrigger>.GetBest();
			if ((bool)best)
			{
				best.Interact(buttonAssignableItem);
			}
		}
	}

	protected void doHyperdash(Vector3 start, Vector3 direction, float distance, float transitDuration, float fullDuration)
	{
		dashInputHeldTimer = 0.25f;
		hyperdashStartPos = base.transform.position;
		hyperdashDestination = hyperdashStartPos + direction.normalized * distance;
		hyperdashTransitDuration = transitDuration;
		hyperdashFullDuration = fullDuration;
		lastHyperdashTime = Time.time;
		partialHyperdash = transitDuration < fullDuration;
		doingHyperdash = true;
		hyperdashTerminationEffectsTriggered = false;
		hyperdashEffectAnimator.transform.position = hyperdashStartPos;
		hyperdashEffectAnimator.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
		if (partialHyperdash)
		{
			hyperdashParticleSystem.Emit(15);
		}
		else
		{
			hyperdashParticleSystem.Emit(30);
		}
	}

	protected Vector3 hyperdash_findEndingLocation(Vector3 start, Vector3 dashDirection)
	{
		Vector3 result = start;
		Vector3 vector = start;
		float num = maxDashDistance / (float)dashSteps;
		for (int i = 0; i < dashSteps; i++)
		{
			Vector3 vector2 = vector + dashDirection * num;
			RaycastHit hit;
			if (hyperdash_doRaycast(vector, dashDirection, out hit, num))
			{
				float num2 = maxAltitudeDifference + groundBuffer;
				if (hyperdash_doRaycast(vector + Vector3.up * num2, dashDirection, out hit, num))
				{
					return result;
				}
				if (hyperdash_doRaycast(vector2 + Vector3.up * num2, Vector3.down, out hit, num2))
				{
					if (hyperdash_doNavSample(hit.point).HasValue)
					{
						result = (vector = hit.point + Vector3.up * groundBuffer);
					}
					else
					{
						vector = hit.point + Vector3.up * groundBuffer;
					}
				}
				else
				{
					vector = vector2;
				}
			}
			else if (hyperdash_doRaycast(vector2, Vector3.down, out hit, maxAltitudeDifference + groundBuffer))
			{
				Vector3? vector3 = hyperdash_doNavSample(hit.point);
				if (vector3.HasValue)
				{
					result = (vector = vector3 ?? (Vector3.zero + Vector3.up * groundBuffer));
				}
				else
				{
					vector = vector2;
				}
			}
			else
			{
				vector = vector2;
			}
		}
		return result;
	}

	protected bool hyperdash_doRaycast(Vector3 start, Vector3 dir, out RaycastHit hit, float distance)
	{
		bool num = Physics.Raycast(start, dir, out hit, distance, dashLayermask, QueryTriggerInteraction.Ignore);
		if (num)
		{
			Debug.DrawRay(start, dir * distance, new Color(1f, 1f, 0f, 0.2f), 1f);
			Debug.DrawRay(start, dir * hit.distance, new Color(1f, 1f, 0f, 1f), 1f);
			return num;
		}
		Debug.DrawRay(start, dir * distance, new Color(0.5f, 0.9f, 1f, 1f), 1f);
		return num;
	}

	protected Vector3? hyperdash_doNavSample(Vector3 point)
	{
		float num = navCheckRadius;
		NavMeshHit hit;
		if (NavMesh.SamplePosition(point, out hit, num, -1))
		{
			Debug.DrawLine(point, hit.position, new Color(1f, 0f, 0f), 1f);
			return hit.position;
		}
		Debug.DrawLine(point, point + Vector3.up * num, Color.red, 1f);
		Debug.DrawLine(point, point + Vector3.right * num, Color.red, 1f);
		Debug.DrawLine(point, point + Vector3.down * num, Color.red, 1f);
		Debug.DrawLine(point, point + Vector3.left * num, Color.red, 1f);
		Debug.DrawLine(point, point + Vector3.forward * num, Color.red, 1f);
		Debug.DrawLine(point, point + Vector3.back * num, Color.red, 1f);
		return null;
	}

	protected override void creature_datSetup()
	{
		normalTurningDuration = new Dat("fox_normalTurningDuration");
		defaultStartingStamina = new Dat("fox_defaultStartingStamina");
		noStaminaDamageAttenuation = new Dat("fox_noStaminaDamageAttenuation");
		magnetizeDuration = new Dat("fox_magnetizeDuration");
		climbSpeed = new Dat("fox_climbSpeed");
		HPPerLevelUpItem = new Dat("fox_HPPerUpgrade");
		coinDropScaler = new Dat("coinDropScaler");
		coinDropFalloffRate = new Dat("coinDropFalloffRate");
		coinDropMin = new Dat("coinDropMin");
		coinDropMax = new Dat("coinDropMax");
		poiseFractionOfMaxHP = new Dat("fox_poiseFractionOfMaxHP");
		for (int i = 0; i < 3; i++)
		{
			string id = "mp" + i;
			mpDefaultStartingAmount[i] = new Dat(id, "mp_startingAmount");
			mpPerLevelUpItem[i] = new Dat(id, "mp_MPPerUpgrade");
			mpRechargeRate[i] = new Dat(id, "mp_rechargeRate");
			mpRechargeDelayOnUse[i] = new Dat(id, "mp_rechargeDelayOnUse");
			mpRechargeDelayOnDeplete[i] = new Dat(id, "mp_rechargeDelayOnDeplete");
		}
		potionRecoverBase = new Dat("hpRestorePerPotionSwig");
		instance = this;
		radiation = 0f;
	}

	protected override void creature_Awake()
	{
		hopCheckLayerMask = LayerMask.GetMask("Default");
		inConduitLayer = LayerMask.NameToLayer("inConduit");
		staminaParticleSystemEmission = staminaParticleSystem.emission;
		damageBoostParticleSystemEmission = damageBoostParticleSystem.emission;
		Setup.DoStandardSetup();
		base.stamina = maxStamina;
		StaminaEffects = true;
		refillMPToMax();
		Profile.LoadPlayerMetersFromSaveDataIfNecessary();
		hyperdashEffectAnimator.transform.parent = null;
		UnityEngine.Object.DontDestroyOnLoad(hyperdashEffectAnimator.gameObject);
		cachedHitReceiver = GetComponent<HitReceiver>();
		cachedCollider = GetComponent<Collider>();
		iFrameLayer = LayerMask.NameToLayer("iFrames");
		playerLayer = LayerMask.NameToLayer("Player");
		hyperdashParticleEmitter = hyperdashParticleSystem.emission;
		wadingAnimatorLayerIndex = cachedAnimator.GetLayerIndex("Wading");
		CutsceneHeadLook = false;
		RuntimeQualityManager.AdjustableThruShadowMaterial = materialManager.gameObject.GetComponent<SkinnedMeshRenderer>().materials[1];
	}

	protected void Start()
	{
		stateMachineBehaviours = cachedAnimator.GetBehaviours<PlayerStateBehaviour>();
		PlayerStateBehaviour[] array = stateMachineBehaviours;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Setup(this, cachedRigidbody);
		}
		itemBehaviours = GetComponents<ItemBehaviour>();
		SceneManager.sceneLoaded += onLevelWasLoaded;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	protected void Update()
	{
		materialManager.FlickerEnabled = base.MercyInvincible;
		materialManager.PulseEnabled = hp < maxhp / 2 && !InstanceIsDead;
		materialManager.PulseRateParameter = Mathf.InverseLerp(maxhp / 2 - 1, 1f, hp);
		materialManager.Ghostliness = (StatSuppressionField.IsActive ? MaxGhostliness : 0f);
	}

	protected void onLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		if (itemBehaviours != null)
		{
			for (int i = 0; i < itemBehaviours.Length; i++)
			{
				itemBehaviours[i].Cancel();
			}
		}
	}

	protected override void creature_FixedUpdate()
	{
		if (!CameraController.Instanced || CutsceneHidden)
		{
			return;
		}
		if (DebugInvincible)
		{
			hp = maxhp;
		}
		if (GUIInput.Actions != null && GUIInput.Actions.Pause.WasPressed)
		{
			menuNextFrame = false;
			GUIMode.PushMode(GUIMode.GetMode<PauseMenu>());
		}
		if (base.IsDead)
		{
			staminaParticleSystemEmission.enabled = false;
			return;
		}
		Vector2 input = Vector2.zero;
		Vector2 input2 = Vector2.zero;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		if (!InventoryDisplay.InventoryOpen)
		{
			input = PlayerInput.Actions.Move.Vector;
			if (input.sqrMagnitude > 1f)
			{
				input.Normalize();
			}
			input2 = PlayerInput.Actions.Look.Vector;
			if (CameraController.Flip)
			{
				input = new Vector2(0f - input.x, input.y);
				input2 = new Vector2(0f - input2.x, input2.y);
			}
			flag = PlayerInput.Actions.InteractDodge.WasPressed;
			bool isPressed = PlayerInput.Actions.InteractDodge.IsPressed;
			flag2 = PlayerInput.Actions.Target.Value > 0.75f;
			flag3 = PlayerInput.Actions.Target.Value > 0f && !flag2;
			flag4 = PlayerInput.Actions.Shield.IsPressed;
			flag5 = PlayerInput.Actions.ToggleTarget.WasPressed;
			if (isPressed)
			{
				dashInputHeldTimer += Time.deltaTime;
			}
			else
			{
				dashInputHeldTimer = 0f;
			}
		}
		if (flag5)
		{
			clickStickTargetting = !clickStickTargetting;
		}
		itemBehaviourInputDowns[0] = PlayerInput.Actions.Item1.WasPressed;
		itemBehaviourInputDowns[1] = PlayerInput.Actions.Item2.WasPressed;
		itemBehaviourInputDowns[2] = PlayerInput.Actions.Item3.WasPressed;
		itemBehaviourInputDowns[3] = PlayerInput.Actions.Flask.WasPressed;
		itemBehaviourInputUps[0] = PlayerInput.Actions.Item1.WasReleased;
		itemBehaviourInputUps[1] = PlayerInput.Actions.Item2.WasReleased;
		itemBehaviourInputUps[2] = PlayerInput.Actions.Item3.WasReleased;
		itemBehaviourInputUps[3] = PlayerInput.Actions.Flask.WasReleased;
		if (flag)
		{
			hasNotMovedSinceLastDashInputDown = true;
		}
		if (input.magnitude < 0.2f)
		{
			input = Vector3.zero;
		}
		else
		{
			hasNotMovedSinceLastDashInputDown = false;
		}
		ProjectedMoveVector = projectOntoYPlane(input);
		Vector3 vector = projectOntoYPlane(input2);
		Vector3 vector2 = base.transform.InverseTransformDirection(ProjectedMoveVector);
		if (input2.magnitude < 0.2f)
		{
			rightStickHasZeroed = true;
		}
		if (PlayerInput.Actions.Manual.WasPressed && Profile.AnyPageEverViewed())
		{
			PageDisplay.Show();
		}
		if (currentLadder != null)
		{
			pushBuffer = Vector3.zero;
			if (cachedAnimator.GetCurrentAnimatorStateInfo(0).IsName("climbing"))
			{
				ladderProgressParam = currentLadder.GetParamFromAltitude(base.transform.position.y);
				float y = input.y;
				float num = Vector3.Dot(ProjectedMoveVector, currentLadder.transform.forward);
				float num2 = Mathf.Clamp(y + num, -1f, 1f);
				float value = num2 * climbSpeed.Value;
				cachedAnimator.SetFloat("climbSpeed", value);
				if (!base.Frozen)
				{
					ladderProgressParam += currentLadder.Normalize(num2 * Time.deltaTime * climbSpeed.Value);
					Vector3 translation = currentLadder.GetPositionFromParam(ladderProgressParam) - base.transform.position;
					base.transform.Translate(translation, Space.World);
					SetFacing(base.transform.position + currentLadder.transform.forward);
				}
				if (num2 > 0f && currentLadder.AtTop(ladderProgressParam))
				{
					cachedAnimator.SetTrigger("climb top");
					SetFacing(base.transform.position + currentLadder.transform.forward);
					currentLadder = null;
					cachedAnimator.SetBool("climbing", false);
				}
				if (num2 < 0f && currentLadder.AtBottom(ladderProgressParam))
				{
					SetFacing(base.transform.position + -currentLadder.transform.forward);
					currentLadder = null;
					cachedAnimator.SetBool("climbing", false);
				}
			}
		}
		cachedAnimator.SetBool("upper body ok to act", !busy);
		ButtonAssignableItem[] array = new ButtonAssignableItem[4];
		if (!base.Frozen && !magnetizing && !InventoryDisplay.InventoryOpen)
		{
			if (!usedItemLastFrame)
			{
				for (int i = 0; i < itemBehaviourInputDowns.Length; i++)
				{
					if (!itemBehaviourInputDowns[i] || !animatorStateAllowingItemUse)
					{
						continue;
					}
					ButtonAssignableItem buttonAssignableItem = null;
					buttonAssignableItem = (array[i] = ((i != 3) ? Inventory.ButtonAssignableItemFromButtonNumber(i) : potionItem));
					if ((bool)buttonAssignableItem)
					{
						buttonAssignableItem.name.ToLower().Contains("sword");
						usedItemLastFrame = true;
						dashInputHeldTimer = 0f;
						if (telegraphCallback != null && buttonAssignableItem.Telegraphs)
						{
							telegraphCallback(buttonAssignableItem.TelegraphRange);
						}
					}
				}
			}
			else
			{
				usedItemLastFrame = false;
			}
			if (flag && !dashInputDownLastFrame)
			{
				dashInputDownLastFrame = true;
				InteractionTrigger best = ProximityRegistered<InteractionTrigger>.GetBest();
				if ((bool)best && !busy)
				{
					best.Interact(null);
				}
				else if (animatorStateAllowingDodge)
				{
					for (int j = 0; j < itemBehaviours.Length; j++)
					{
						itemBehaviours[j].Cancel();
					}
					if (ProjectedMoveVector.sqrMagnitude > 0f && !base.InWater && (Stamina > 0f || StaminaSettings.DodgeRequirements != StaminaSettings.StaminaRequirementMode.NOT_ALLOWED_WHEN_STAMINA_IS_ZERO))
					{
						lastDashTime = Time.time;
						DashDirection = ProjectedMoveVector.normalized;
						if (Stamina == 0f && StaminaSettings.DodgeRequirements == StaminaSettings.StaminaRequirementMode.WEAK_WHEN_STAMINA_IS_ZERO)
						{
							cachedAnimator.SetTrigger("forward roll garbage");
						}
						else
						{
							cachedAnimator.SetTrigger("forward roll");
						}
					}
				}
			}
			else
			{
				dashInputDownLastFrame = false;
			}
		}
		float num3 = normalTurningDuration.Value;
		Vector3 direction = ((vector.sqrMagnitude > 0f) ? vector : ProjectedMoveVector);
		previewZTarget = ZTarget.GetTargetFromPositionAndDirection(base.transform.position, direction, CameraController.MaxZTargetDistance);
		if (headTrackingBone != null)
		{
			if (CutsceneHeadLook)
			{
				headTrackingBone.TrackingOn = true;
				headTrackingBone.TrackingTarget = CutsceneHeadLookPosition;
			}
			else if (zTarget != null && !busy)
			{
				headTrackingBone.TrackingOn = true;
				headTrackingBone.TrackingTarget = zTarget.WorldPosition;
			}
			else if (previewZTarget != null && !busy)
			{
				headTrackingBone.TrackingOn = true;
				headTrackingBone.TrackingTarget = previewZTarget.WorldPosition;
			}
			else
			{
				headTrackingBone.TrackingOn = false;
			}
		}
		bool flag6 = PlayerTriggerZone<DarkZone>.PlayerInAnyZone && lanternItem.Quantity == 0;
		if (flag6)
		{
			previewZTarget = null;
		}
		float num5;
		if ((flag2 || clickStickTargetting) && !flag6)
		{
			if (flag2 && clickStickTargetting)
			{
				zTarget = null;
				lowerDistanceIfZTargetGoesNull = false;
				clickStickTargetting = false;
			}
			if (zTarget == null)
			{
				float num4 = (lowerDistanceIfZTargetGoesNull ? 0.5f : 1f);
				zTarget = ZTarget.GetTargetFromPositionAndDirection(base.transform.position, direction, CameraController.MaxZTargetDistance * num4);
				if (zTarget != null)
				{
					lowerDistanceIfZTargetGoesNull = true;
				}
			}
			if (zTarget != null && !zTarget.IsActive)
			{
				zTarget = null;
				clickStickTargetting = false;
			}
			if (input2.magnitude > 0.5f && rightStickHasZeroed && zTarget != null)
			{
				zTarget = ZTarget.GetTargetFromPositionAndDirection(zTarget.WorldPosition, input2.normalized, CameraController.MaxZTargetDistance, zTarget, true);
				rightStickHasZeroed = false;
			}
			Vector3 vector3 = base.transform.position + base.transform.forward;
			if (zTarget != null)
			{
				if ((zTarget.WorldPosition - base.transform.position).magnitude < CameraController.MaxZTargetDistance)
				{
					vector3 = zTarget.WorldPosition;
				}
				else
				{
					zTarget = null;
					lowerDistanceIfZTargetGoesNull = false;
					clickStickTargetting = false;
				}
				StrafeTargetting = false;
			}
			else
			{
				clickStickTargetting = false;
				StrafeTargetting = true;
			}
			num5 = Geometry.Vector2Angle(vector3 - base.transform.position);
			if (facingLock == FacingLockType.LOCK_ON || animatorRequestedInstantFacing)
			{
				num3 = 0f;
			}
		}
		else
		{
			zTarget = null;
			lowerDistanceIfZTargetGoesNull = false;
			if (flag3 || (flag2 && flag6))
			{
				StrafeTargetting = true;
				num5 = Geometry.Vector2Angle(base.transform.forward);
			}
			else
			{
				StrafeTargetting = false;
				num5 = ((!(ProjectedMoveVector.magnitude > 0f)) ? Geometry.Vector2Angle(base.transform.forward) : Geometry.Vector2Angle(ProjectedMoveVector));
			}
			if (animatorRequestedInstantFacing)
			{
				num3 = 0f;
			}
		}
		if (facingLock == FacingLockType.NO_TURNING && !animatorRequestedInstantFacing)
		{
			num5 = Geometry.Vector2Angle(base.transform.forward);
		}
		num3 = ((TurnSpeedAttenuation != 0f && !base.Frozen) ? (num3 / TurnSpeedAttenuation) : float.PositiveInfinity);
		float current = Geometry.Vector2Angle(base.transform.forward);
		float a = ((num3 != 0f) ? Mathf.SmoothDampAngle(current, num5, ref turningVelocity, num3) : num5);
		Quaternion rotation = Geometry.Angle2Quaternion(a);
		base.transform.rotation = rotation;
		animatorRequestedInstantFacing = false;
		for (int k = 0; k < array.Length; k++)
		{
			if (array[k] != null)
			{
				bool flag10 = itemBehaviourInputUps[k];
				ButtonAssignableItemUsed(array[k], k, array);
			}
		}
		lastFrameRadiationAccumulation *= Mathf.Pow(0.01f, Time.deltaTime);
		float num6 = ProjectedMoveVector.magnitude * cachedAnimator.GetFloat("AP_maxMoveSpeedFactor");
		if (!base.Frozen)
		{
			cachedAnimator.SetFloat("move forward", vector2.z * num6, 0.1f, Time.deltaTime);
			cachedAnimator.SetFloat("move right", vector2.x * num6, 0.1f, Time.deltaTime);
			playerIsSprinting = dashInputHeldTimer > 0.25f && (!flag2 || clickStickTargetting) && ProjectedMoveVector.magnitude > 0f && !base.InWater && !usedItemLastFrame;
			cachedAnimator.SetBool("sprint", playerIsSprinting);
		}
		wadingAnimationLayerWeight = Mathf.Clamp01(wadingAnimationLayerWeight + Time.deltaTime * (float)(base.InWater ? 1 : (-1)));
		cachedAnimator.SetLayerWeight(wadingAnimatorLayerIndex, wadingAnimationLayerWeight);
		staminaParticleSystemEmission.enabled = Stamina <= 0f;
		float layerWeight = cachedAnimator.GetLayerWeight(2);
		int num7 = ((!StaminaEffects) ? 1 : 0);
		float weight = Mathf.SmoothDamp(layerWeight, num7, ref staminaLayerWeightVelocity, 0.125f, float.PositiveInfinity);
		cachedAnimator.SetLayerWeight(2, weight);
		if (flag4 && base.stamina > 0f && shieldItem.Quantity > 0 && !base.IsDead)
		{
			cachedAnimator.SetBool("shield up", true);
			ShieldUp = true;
			if (StaminaSettings.ShieldUpResetsRefillDelay)
			{
				lastStaminaNoChargeTime = Time.time;
			}
		}
		else
		{
			cachedAnimator.SetBool("shield up", false);
			if (ShieldUp && !base.IsDead && base.stamina > 0f && !busy)
			{
				SFX.PlayAudioClipAtFox(shieldDownSFX);
			}
			ShieldUp = false;
		}
		bool flag7 = false;
		bool flag8 = false;
		for (int l = 0; l < stateMachineBehaviours.Length; l++)
		{
			flag7 |= stateMachineBehaviours[l].RechargeStaminaFast;
			flag8 |= stateMachineBehaviours[l].RechargeStaminaSlow;
		}
		if (ShieldUp)
		{
			flag7 = false;
			flag8 = true;
		}
		if (StaminaSettings.StaminaRechargeSpeed_Slow == 0f && !flag7 && flag8)
		{
			flag8 = false;
		}
		if (flag7 || flag8)
		{
			if (Time.time > lastStaminaDepleatTime + StaminaSettings.DepleatedRechargeDelay && Time.time > lastStaminaNoChargeTime + StaminaSettings.RechargeDelay)
			{
				float num8 = (flag8 ? StaminaSettings.StaminaRechargeSpeed_Slow : StaminaSettings.StaminaRechargeSpeed_Fast);
				base.stamina += Time.deltaTime * num8;
				StaminaEffects = true;
			}
		}
		else
		{
			lastStaminaNoChargeTime = Time.time;
		}
		if (base.stamina == maxStamina)
		{
			lastStaminaFullTime = Time.time;
		}
		if (Time.deltaTime > 0f)
		{
			for (int m = 0; m < 3; m++)
			{
				if (lastMPUseTime[m] + mpRechargeDelayOnUse[m].Value < Time.time && lastMPDepleatTime[m] + mpRechargeDelayOnDeplete[m].Value < Time.time)
				{
					SetMP(m, GetMP(m) + Time.deltaTime * mpRechargeRate[m].Value);
				}
			}
		}
		bool flag9 = false;
		if (lanternItem.Quantity > 0 && PlayerTriggerZone<DarkZone>.PlayerInAnyZone)
		{
			flag9 = true;
			lanternLight.intensity = DarkZone.LanternIntensity;
			lanternLight.range = DarkZone.LanternRange;
			lanternLight.bounceIntensity = DarkZone.LanternBounce;
			lanternLight.shadows = (DarkZone.LanternShadows ? LightShadows.Hard : LightShadows.None);
		}
		lanternLight.enabled = flag9;
		SlowdownEffect = cachedAnimator.GetFloat("AP_slowdown");
		damageBoostTime = Mathf.Max(0f, damageBoostTime - Time.deltaTime);
		damageBoostParticleSystemEmission.enabled = damageBoostTime > 0f;
	}

	protected Vector2 worldToViewportPoint(Vector3 pos)
	{
		return CameraController.WorldToViewportPoint(pos);
	}

	protected float mapDotToVelocityFactor(float d)
	{
		return (float)Math.Tanh(2f * (d + 0.5f)) / 2f + 0.5f;
	}

	protected float DotWithPossiblePenalty(Vector2 a, Vector2 b, bool penalize)
	{
		return Vector2.Dot(a, b) - (penalize ? 1.9f : 0f);
	}

	protected override void creature_OnAnimatorMove()
	{
		if (cutsceneHidden)
		{
			return;
		}
		if (itemBehaviours != null)
		{
			for (int i = 0; i < itemBehaviours.Length; i++)
			{
				itemBehaviours[i].Visible = false;
			}
			if (currentLadder == null && !InConduit && !doingHyperdash)
			{
				for (int j = 0; j < itemBehaviours.Length; j++)
				{
					itemBehaviours[j].Visible = itemBehaviours[j] == lastUsedItemBehaviour;
				}
			}
		}
		if (magnetizing)
		{
			float num = Time.fixedDeltaTime / 0.125f;
			magnetParameter += num;
			Debug.LogFormat("[Magnetizing] dt:{0}  fdt:{1}  step: {2}  magnetParameter: {3}", Time.deltaTime, Time.fixedDeltaTime, num, magnetParameter);
			if (magnetParameter > 1f - num)
			{
				magnetParameter = 1f;
				magnetizing = false;
			}
			base.transform.position = Vector3.Lerp(magnetPositionStart, magnetPositionEnd, magnetParameter);
			base.transform.rotation = Quaternion.Slerp(magnetRotationStart, magnetRotationEnd, magnetParameter);
			return;
		}
		if (Time.deltaTime > 0f)
		{
			if (cachedAnimator.GetCurrentAnimatorStateInfo(0).IsName("roll") || cachedAnimator.GetCurrentAnimatorStateInfo(0).IsName("roll garbage") || cachedAnimator.GetCurrentAnimatorStateInfo(0).IsName("Roll into thrust"))
			{
				cachedRigidbody.velocity += cachedAnimator.deltaPosition.magnitude * DashDirection / Time.deltaTime;
				if (zTarget != null)
				{
					float normalizedTime = cachedAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
					float t = Mathf.Max(0f, Mathf.Pow(2f * normalizedTime - 1f, 3f));
					Quaternion b = Geometry.Vector2Quaternion(zTarget.WorldPosition - base.transform.position);
					Quaternion rotation = Quaternion.Slerp(base.transform.rotation, b, t);
					cachedRigidbody.transform.rotation = rotation;
				}
				else
				{
					cachedRigidbody.transform.rotation *= cachedAnimator.deltaRotation;
				}
			}
			else if (doingHyperdash)
			{
				float num2 = (Time.time - lastHyperdashTime) / hyperdashTransitDuration;
				float num3 = (Time.time - lastHyperdashTime) / hyperdashFullDuration;
				float num4 = hyperdashTransitDuration / hyperdashFullDuration;
				if (num2 < 1f)
				{
					float t2 = num2;
					hyperdashEffectAnimator.SetBool("dashing", true);
					hyperdashHidden = true;
					num2 += Time.deltaTime / hyperdashTransitDuration;
					base.transform.position = Vector3.Lerp(hyperdashStartPos, hyperdashDestination, t2);
				}
				else
				{
					if (!hyperdashTerminationEffectsTriggered)
					{
						hyperdashTerminationEffectsTriggered = true;
						hyperdashEffectAnimator.SetBool("dashing", false);
						base.transform.position = hyperdashDestination;
						if (partialHyperdash)
						{
							hyperdashTerminationEffectAnimator.SetTrigger("go");
						}
					}
					if (num3 > 1f)
					{
						cachedAnimator.SetBool("hyperdashing", false);
						hyperdashEffectAnimator.SetBool("dashing", false);
						doingHyperdash = false;
					}
				}
			}
			else if (!RootMotionHitCancelled)
			{
				cachedRigidbody.velocity += cachedAnimator.deltaPosition / Time.deltaTime;
				cachedRigidbody.transform.rotation *= cachedAnimator.deltaRotation;
			}
			cachedAnimator.SetBool("hyperdashing", doingHyperdash);
			hyperdashHidden = doingHyperdash;
		}
		Vector3 vector = cachedAnimator.GetFloat("AP_inLocomotionState") * ProjectedMoveVector.normalized * ProjectedMoveVector.sqrMagnitude * 7f * cachedAnimator.GetFloat("AP_maxMoveSpeedFactor");
		if (!base.Frozen)
		{
			cachedRigidbody.velocity += vector;
		}
		if (doingHyperdash && StaminaEffects)
		{
			base.gameObject.layer = inConduitLayer;
		}
		else if (IFrameInvincible)
		{
			base.gameObject.layer = iFrameLayer;
		}
		else
		{
			base.gameObject.layer = playerLayer;
		}
	}

	protected override void onFall()
	{
		if (!base.IsDead)
		{
			cachedAnimator.SetBool("falling", true);
		}
	}

	protected override bool doNavMeshSnappingAndReturnTrueIfGravityShouldBeApplied()
	{
		if (groundingTechnique == GroundingTechnique.NONE)
		{
			return false;
		}
		if (PlayerTriggerZone<PlayerOffNavTrigger>.PlayerInAnyZone)
		{
			return true;
		}
		Vector3? snapToNavMeshVector = getSnapToNavMeshVector();
		bool flag = !snapToNavMeshVector.HasValue;
		Vector3 translation = snapToNavMeshVector ?? Vector3.zero;
		switch (groundingTechnique)
		{
		case GroundingTechnique.NAVMESH:
			if (flag)
			{
				return true;
			}
			base.transform.Translate(translation, Space.World);
			cachedAnimator.SetBool("falling", false);
			return false;
		case GroundingTechnique.NAVMESH_PERMISSIVE:
		{
			Vector3 vector = base.transform.position + Vector3.up * hopCheckHeight;
			Vector3 normalized = cachedRigidbody.velocity.normalized;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			if (!Physics.Raycast(vector, normalized, hopCheckReach, hopCheckLayerMask))
			{
				flag2 = true;
				RaycastHit hitInfo;
				if (Physics.Raycast(vector + normalized * hopCheckReach, Vector3.down, out hitInfo, hopCheckMaxDrop, hopCheckLayerMask) && hitInfo.distance > hopCheckHeight)
				{
					flag3 = true;
					NavMeshHit hit;
					if (NavMesh.SamplePosition(hitInfo.point, out hit, hopCheckNavMeshSampleDistance, -1))
					{
						flag4 = true;
						Debug.DrawRay(hit.position, Vector3.left, Color.green);
						Debug.DrawRay(hit.position, Vector3.right, Color.green);
						Debug.DrawRay(hit.position, Vector3.forward, Color.green);
						Debug.DrawRay(hit.position, Vector3.back, Color.green);
						Vector3 direction = hit.position - vector;
						if (Physics.Raycast(vector, direction, direction.magnitude, hopCheckLayerMask))
						{
							flag5 = true;
						}
						NavMeshHit hit2;
						if (!NavMesh.SamplePosition(base.transform.position, out hit2, playerCheckNavMeshSampleDistance, -1))
						{
							flag6 = true;
						}
					}
				}
			}
			if (flag2 && flag3 && flag4 && (flag5 || flag6))
			{
				return true;
			}
			if (flag)
			{
				return true;
			}
			base.transform.Translate(translation, Space.World);
			return false;
		}
		case GroundingTechnique.GRAVITY:
			return true;
		default:
			return true;
		}
	}
}
