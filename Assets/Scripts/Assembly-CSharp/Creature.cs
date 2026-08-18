using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityStandardAssets.Utility;

[SelectionBase]
[RequireComponent(typeof(Flammable))]
public class Creature : MonoBehaviour, IDamageable
{
	public delegate IEnumerator crntn();

	[Header("Creature")]
	[SerializeField]
	protected string dataRow = "creatureName";

	protected Dat powerLevel;

	protected Dat defaultStartingMaxHP;

	protected Dat maxPoiseDat;

	protected Dat poiseRefillDelay;

	[SerializeField]
	protected float pushBufferDamping = 0.1f;

	[SerializeField]
	protected bool useiFrameAnimParam;

	[SerializeField]
	protected string iFrameParameterName = "AP_iframes";

	[Header("Creature - Effects")]
	[SerializeField]
	[EventRef]
	protected string hurtSFX;

	[SerializeField]
	[EventRef]
	protected string bigHurtSFX;

	[SerializeField]
	[EventRef]
	protected string dieSFX;

	[SerializeField]
	[EventRef]
	protected string inWaterLoopSFX = "event:/main/player/general/water_wade_loop";

	[FormerlySerializedAs("hurtFlasher")]
	[SerializeField]
	protected CreatureMaterialManager materialManager;

	[SerializeField]
	protected CreatureMaterialManager[] extraMaterialManagers;

	[SerializeField]
	protected GameObject iceBaseGameObject;

	[SerializeField]
	protected ParticleSystem healthLossParticleSystem;

	[SerializeField]
	protected ParticleSystem poisonParticleSystem;

	[Header("Creature - Parry")]
	[SerializeField]
	protected bool isParriable;

	[SerializeField]
	protected Transform parryStunEffectParentTransform;

	protected static readonly float groundingCheckDistancePerSecond = 1f;

	protected static readonly float gravity = 60f;

	protected static readonly float kKillFloorAltitude = -50f;

	protected static readonly float kPushMagnitudeSufficientToSuppressNavMeshSnapping = 999f;

	protected static readonly float kPostTeatherNavMeshSupressionDuration = 0.5f;

	protected static readonly float kDefaultSlowInWaterFactor = 0.66f;

	protected static readonly float kPoisonReductionRate = 1f / 15f;

	protected static readonly string kPathToForcefieldPrefab = "Forcefield";

	protected static readonly string kPathToParryStunParticleSystem = "ParryStunEffect";

	protected static readonly string kPathToSnowParticleSystem = "CreatureFrozenSnowParticleSystem";

	protected static readonly string kPathToIceBaseObject = "CreatureFrozenIceBase";

	protected static readonly float verticalVelocityFallThreshold = 8f;

	protected static readonly float fallRaycastLength = 8f;

	protected static readonly float kDamageToReduceFreezeTimeByOneSecond = 20f;

	protected static LayerMask _groundingRaycastLayerMask;

	protected static bool _groundingLayerMaskSetUp;

	protected float _poise;

	public Action OnDestroyCallbacks;

	protected Rigidbody cachedRigidbody;

	protected Animator cachedAnimator;

	protected HitQuiver cachedHitQuiver;

	protected NavMeshQueryFilter navMeshQueryFilter;

	protected float lastDamageTime = float.NegativeInfinity;

	protected float lastBlockTime = float.NegativeInfinity;

	protected float lastPoiseDamageTime = float.NegativeInfinity;

	protected float lastStaminaDepleatTime = float.NegativeInfinity;

	protected float lastStaminaNoChargeTime = float.NegativeInfinity;

	protected float lastStaminaFullTime = float.NegativeInfinity;

	protected Vector3 lastPhysicsForce;

	protected Vector3 pushBuffer;

	protected float timeOfDeath;

	protected float verticalVelocity;

	protected float frozenUntil;

	protected float freezeStartTime;

	protected float poisonCounter;

	protected Vector3 previousPosition;

	protected bool beingTetherDragged;

	protected bool tetherSuppressesNavMeshSnappingOnEnd;

	protected Vector3 tetherDragTarget;

	protected float tetherDragSpeed;

	protected float timeSinceLastNavMeshSupressingTetherEnd = float.NegativeInfinity;

	protected ParticleSystem.EmissionModule parryStunEmission;

	protected bool megaflinchAnimationStateExists;

	protected bool wasFrozen;

	protected RigidbodyConstraints initialRigidbodyConstraints;

	protected Vector3 lastPosition;

	protected Quaternion lastRotation;

	protected EventInstance inWaterFMODEventInstance;

	protected PARAMETER_ID inWaterSpeedParamID;

	protected const string kDefaultHurtSFXString = "event:/main/enemy/shared/hit";

	protected const string kDefaultBigHurtSFXString = "event:/main/enemy/shared/hit";

	protected const string kDefaultDieSFXString = "event:/main/enemy/shared/death";

	protected const string kMegaflinchStateName = "megaflinch";

	protected int _damage;

	protected float _stamina;

	protected float lastNavMeshSnapTime;

	protected const float kForceNavMeshSnapDistanceSqr = 0.01f;

	protected const float kMaxNavMeshSnapInterval = 0.333f;

	protected const float kNavMeshSampleMoveDistanceFactor = 2f;

	private Vector3 lastNavHitPosition;

	protected const string kPathToCreatureWaterFringeParticleSystem = "CreatureWaterFringeFX";

	private CreatureWaterFringeFX _waterFringeFX;

	private TextMesh textMesh;

	protected bool iceEffectsSetUp;

	protected ParticleSystem.EmissionModule snowParticleSystemEmission;

	protected ParticleSystem snowParticleSystem;

	protected static LayerMask groundingLayerMask
	{
		get
		{
			if (!_groundingLayerMaskSetUp)
			{
				_groundingRaycastLayerMask = (1 << LayerMask.GetMask("Default")) | (1 << LayerMask.GetMask("DefaultHidden"));
				_groundingLayerMaskSetUp = true;
			}
			return _groundingRaycastLayerMask;
		}
	}

	public virtual int PowerLevel
	{
		get
		{
			return powerLevel.IntValue;
		}
	}

	public bool IsDead { get; protected set; }

	public virtual bool InConduit
	{
		get
		{
			return false;
		}
	}

	public bool Invulnerable { get; set; }

	public int Poise
	{
		get
		{
			if (alwaysFlinch)
			{
				return 0;
			}
			return Mathf.RoundToInt(_poise);
		}
		protected set
		{
			_poise = Mathf.Min(Mathf.Max(0, value), maxPoise);
		}
	}

	protected virtual bool alwaysFlinch
	{
		get
		{
			return false;
		}
	}

	public virtual bool IFrameInvincible
	{
		get
		{
			if (useiFrameAnimParam)
			{
				return !Mathf.Approximately(cachedAnimator.GetFloat(iFrameParameterName), 0f);
			}
			return false;
		}
	}

	public bool MercyInvincible
	{
		get
		{
			return Time.time < lastDamageTime + hurtInvincibilityDuration;
		}
	}

	public bool MercyBlockInvincible
	{
		get
		{
			return Time.time < lastBlockTime + blockInvincibilityDuration;
		}
	}

	protected virtual float hurtInvincibilityDuration
	{
		get
		{
			return 0.2f;
		}
	}

	protected virtual float blockInvincibilityDuration
	{
		get
		{
			return 0.25f;
		}
	}

	public virtual float DamageFactor
	{
		get
		{
			return 1f;
		}
	}

	public virtual float HitForceFactor
	{
		get
		{
			return 1f;
		}
	}

	public virtual int PoisePowerAdjust
	{
		get
		{
			return 0;
		}
	}

	public float NormalizeHP
	{
		get
		{
			return (float)hp / (float)maxhp;
		}
	}

	public float NormalizedPoise
	{
		get
		{
			return (float)Poise / (float)maxPoise;
		}
	}

	public bool Frozen
	{
		get
		{
			return Time.time < frozenUntil;
		}
	}

	public bool IsParryStunned
	{
		get
		{
			if (isParriable)
			{
				return cachedAnimator.GetFloat("AP_parryStun") > 0f;
			}
			return false;
		}
	}

	public int WaterTriggerCount { get; set; }

	public bool InWater
	{
		get
		{
			return WaterTriggerCount > 0;
		}
	}

	public float WaterWorldAltitude { get; set; }

	public Vector3 Velocity
	{
		get
		{
			return cachedRigidbody.velocity;
		}
	}

	public virtual bool Tetherable
	{
		get
		{
			return true;
		}
	}

	public virtual bool TetherToMeInstead
	{
		get
		{
			return false;
		}
	}

	protected virtual float navMeshMinSampleDistance
	{
		get
		{
			return 0.125f;
		}
	}

	protected virtual float navMeshMaxSampleDistance
	{
		get
		{
			return 2f;
		}
	}

	protected virtual int hp
	{
		get
		{
			return maxhp - _damage;
		}
		set
		{
			_damage = maxhp - Mathf.Clamp(value, 0, maxhp);
			if (_damage == maxhp && !IsDead)
			{
				IsDead = true;
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
				StartCoroutine(Die());
			}
		}
	}

	protected float stamina
	{
		get
		{
			return _stamina;
		}
		set
		{
			if (_stamina > value)
			{
				lastStaminaNoChargeTime = Time.time;
			}
			_stamina = Mathf.Clamp(value, 0f, maxStamina);
			if (_stamina == 0f)
			{
				lastStaminaDepleatTime = Time.time;
			}
		}
	}

	protected virtual int maxhp
	{
		get
		{
			return defaultStartingMaxHP.IntValue;
		}
	}

	protected virtual float maxStamina { get; set; }

	protected virtual int maxPoise
	{
		get
		{
			return maxPoiseDat.IntValue;
		}
	}

	protected float poiseDamagePointsToMegaFlinch
	{
		get
		{
			return (float)maxPoise * StaminaSettings.MegaFlinchThresholdFactor;
		}
	}

	public virtual bool SuppressBlocking
	{
		get
		{
			return false;
		}
	}

	protected virtual bool SuppressPushbuffer
	{
		get
		{
			return false;
		}
	}

	protected virtual float SlowInWaterFactor
	{
		get
		{
			return kDefaultSlowInWaterFactor;
		}
	}

	protected virtual bool isPhysicsBased
	{
		get
		{
			return false;
		}
	}

	protected virtual bool suppressNavMeshSnapping
	{
		get
		{
			if (!(pushBuffer.magnitude > kPushMagnitudeSufficientToSuppressNavMeshSnapping))
			{
				return Time.time - timeSinceLastNavMeshSupressingTetherEnd < kPostTeatherNavMeshSupressionDuration;
			}
			return true;
		}
	}

	protected virtual float creature_AnimatorSpeed
	{
		get
		{
			return 1f;
		}
	}

	public virtual bool Paused { get; set; }

	protected bool waterFringeParticleEmission
	{
		set
		{
			if (_waterFringeFX == null && !value)
			{
				return;
			}
			if (_waterFringeFX == null)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("CreatureWaterFringeFX", typeof(GameObject)));
				gameObject.gameObject.name = "WaterFringeFX-" + base.name;
				_waterFringeFX = gameObject.GetComponent<CreatureWaterFringeFX>();
				_waterFringeFX.Creature = this;
				WaterSplashSize component = _waterFringeFX.Creature.GetComponent<WaterSplashSize>();
				if (component != null)
				{
					_waterFringeFX.transform.localScale = Vector3.one * component.FringeSize;
				}
			}
			if (!value)
			{
				_waterFringeFX.Kill();
				_waterFringeFX = null;
			}
		}
	}

	protected bool freezeEffects
	{
		set
		{
			if (!iceEffectsSetUp)
			{
				setUpIceEffects();
			}
			snowParticleSystemEmission.enabled = value;
			if (iceBaseGameObject != null)
			{
				iceBaseGameObject.SetActive(value);
			}
		}
	}

	protected virtual float UnfreezeBlastRadiusFactor
	{
		get
		{
			return 1f;
		}
	}

	public void AddPush(Vector3 push)
	{
		if (!IsParryStunned)
		{
			pushBuffer += Vector3.Scale(push, Mathx.flatteningVector) / cachedRigidbody.mass;
		}
	}

	public void RefillPoise()
	{
		Poise = maxPoise;
	}

	public virtual void ResetAnimationStates()
	{
	}

	public void AddFreezeTime(float t)
	{
		if (!Frozen)
		{
			freezeStartTime = Time.time;
			frozenUntil = Time.time + t;
			if (!iceEffectsSetUp)
			{
				setUpIceEffects();
			}
		}
	}

	public void clearFreezeTime()
	{
		frozenUntil = 0f;
	}

	public void AddPoison(float poisonToAdd)
	{
		poisonCounter += Mathf.Min(poisonToAdd, hp);
	}

	public void ClearPoison()
	{
		poisonCounter = 0f;
	}

	public virtual void DoTetherHit()
	{
		if (Tetherable)
		{
			GetComponent<HitReceiver>().ReceiveHit(HitType.WAND, 0, -1f, 9999, 0, 0f, 0f, true, false, base.transform.position, 0f, base.transform.position, Quaternion.identity, false, null, null, -1);
		}
	}

	public virtual void DoTetherDrag(Vector3 pullToPosition, float speed, Vector3 forceVector, bool suppressNavMeshSnappingAtEnd)
	{
		if (Tetherable)
		{
			base.transform.rotation = Quaternion.LookRotation((pullToPosition - base.transform.position).Flatten(), Vector3.up);
			beingTetherDragged = true;
			tetherDragTarget = pullToPosition;
			tetherDragSpeed = speed;
			tetherSuppressesNavMeshSnappingOnEnd = suppressNavMeshSnappingAtEnd;
		}
	}

	protected bool megaFlinchCheck(int poiseDamagePoints)
	{
		return (float)poiseDamagePoints > poiseDamagePointsToMegaFlinch;
	}

	protected virtual IEnumerator Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	protected virtual void ReceivedDamageEffects(bool brokePoise, int damagePoints)
	{
	}

	protected virtual void BlockDamageEffects()
	{
	}

	[ContextMenu("Test Damage")]
	protected void TestDamage()
	{
		GetComponent<HitReceiver>().ReceiveHit(HitType.NORMAL, 0, -1f, 100, 0, 0f, 0f, true, true, base.transform.position, 1f, base.transform.position, Quaternion.identity, false, null, null, -1);
	}

	bool IDamageable.ReceiveDamage(int damagePoints, int poisePoints, Vector3 physicsForce, float freezeTime, int poisonDamage)
	{
		if (!MercyInvincible && !IsDead)
		{
			if (IsParryStunned)
			{
				damagePoints *= 2;
			}
			damagePoints = modifyDamage(damagePoints);
			poisePoints = modifyPoiseDamage(poisePoints);
			if (damagePoints > 0)
			{
				lastDamageTime = Time.time;
				int count = Mathf.Clamp(damagePoints, 0, hp);
				lastPhysicsForce = physicsForce;
				hp -= damagePoints;
				if ((bool)healthLossParticleSystem)
				{
					healthLossParticleSystem.Emit(count);
				}
				if (Frozen)
				{
					frozenUntil -= (float)damagePoints / kDamageToReduceFreezeTimeByOneSecond;
				}
			}
			AddPush(physicsForce);
			if (poisePoints > 0)
			{
				Poise -= poisePoints;
				lastPoiseDamageTime = Time.time;
			}
			bool result = false;
			bool flag = false;
			if (Poise <= 0 && !Frozen)
			{
				result = true;
				if (!IsParryStunned && !IsDead)
				{
					if (megaFlinchCheck(poisePoints))
					{
						flag = true;
						Flinch(true);
					}
					else
					{
						Flinch();
					}
				}
			}
			if (freezeTime > 0f)
			{
				AddFreezeTime(freezeTime);
				if (iceBaseGameObject != null)
				{
					iceBaseGameObject.transform.rotation = Quaternion.LookRotation(-physicsForce.Flatten(), Vector3.up);
				}
			}
			if (poisonDamage > 0)
			{
				AddPoison(poisonDamage);
			}
			ReceivedDamageEffects(flag, damagePoints);
			if (flag)
			{
				if (string.IsNullOrEmpty(bigHurtSFX))
				{
					SFX.PlayAudioClipHere("event:/main/enemy/shared/hit", base.transform.position);
				}
				else
				{
					SFX.PlayAudioClipHere(bigHurtSFX, base.transform.position);
				}
			}
			else if (damagePoints > 0 || poisePoints > 0)
			{
				if (string.IsNullOrEmpty(hurtSFX))
				{
					SFX.PlayAudioClipHere("event:/main/enemy/shared/hit", base.transform.position);
				}
				else
				{
					SFX.PlayAudioClipHere(hurtSFX, base.transform.position);
				}
			}
			return result;
		}
		return false;
	}

	void IDamageable.BlockDamage(int sp, Vector3 physicsForce)
	{
		if (!MercyBlockInvincible)
		{
			int num = modifyBlockDamage(sp);
			if ((float)num < stamina)
			{
				stamina -= num;
				AddPush(physicsForce);
			}
			else if (stamina > 0f)
			{
				stamina = 0f;
				cachedAnimator.SetTrigger("stagger");
				AddPush(physicsForce);
			}
			else
			{
				((IDamageable)this).ReceiveDamage(sp, sp, physicsForce, 0f, 0);
			}
			lastBlockTime = Time.time;
		}
		BlockDamageEffects();
	}

	public void Flinch(bool megaflinch)
	{
		ResetAnimationStates();
		RefillPoise();
		cachedAnimator.Play("hit", 0, 0f);
		if (megaflinch && megaflinchAnimationStateExists && StaminaSettings.MegaFlinches)
		{
			cachedAnimator.Play("megaflinch", 0, 0f);
		}
		onFlinch();
	}

	public void Flinch()
	{
		Flinch(false);
	}

	protected virtual void onFlinch()
	{
	}

	protected virtual int modifyBlockDamage(int damagePoints)
	{
		return damagePoints;
	}

	protected virtual int modifyDamage(int damage)
	{
		return damage;
	}

	protected virtual int modifyPoiseDamage(int poiseDamage)
	{
		return poiseDamage;
	}

	public virtual float ModifyDefensePower(float dr)
	{
		return dr;
	}

	public virtual int ModifyReceivedDamage(int dmg)
	{
		return dmg;
	}

	protected virtual bool doNavMeshSnappingAndReturnTrueIfGravityShouldBeApplied()
	{
		if ((base.transform.position - lastNavHitPosition).sqrMagnitude > 0.01f || Time.time > lastNavMeshSnapTime + 0.333f)
		{
			Vector3? snapToNavMeshVector = getSnapToNavMeshVector();
			if (!snapToNavMeshVector.HasValue)
			{
				return true;
			}
			Vector3 position = base.transform.position + (snapToNavMeshVector ?? Vector3.zero);
			base.transform.position = position;
			lastNavMeshSnapTime = Time.time;
			return false;
		}
		return false;
	}

	protected Vector3? getSnapToNavMeshVector()
	{
		float value = (lastNavHitPosition - base.transform.position).magnitude * 2f;
		value = Mathf.Clamp(value, navMeshMinSampleDistance, navMeshMaxSampleDistance);
		FPSCounter.LogEvent(value);
		NavMeshHit hit;
		if (NavMesh.SamplePosition(base.transform.position, out hit, value, navMeshQueryFilter))
		{
			Debug.DrawLine(base.transform.position, hit.position, Color.white);
			lastNavHitPosition = hit.position;
			return hit.position - base.transform.position;
		}
		Debug.DrawRay(base.transform.position, Vector3.down * value, Color.blue);
		return null;
	}

	protected void Awake()
	{
		powerLevel = new Dat("level", dataRow);
		defaultStartingMaxHP = new Dat("hp", dataRow);
		maxPoiseDat = new Dat("pp", dataRow);
		poiseRefillDelay = new Dat("pp_refillDelay", dataRow);
		if (isParriable)
		{
			ParticleSystem component = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load(kPathToParryStunParticleSystem, typeof(GameObject)))).GetComponent<ParticleSystem>();
			parryStunEmission = component.emission;
			if (parryStunEffectParentTransform != null)
			{
				component.transform.parent = parryStunEffectParentTransform;
				component.transform.position = parryStunEffectParentTransform.position + Vector3.up;
			}
			else
			{
				component.transform.parent = base.transform;
				component.transform.position = Vector3.zero;
			}
			parryStunEmission.enabled = false;
		}
		creature_datSetup();
		hp = maxhp;
		cachedAnimator = GetComponent<Animator>();
		cachedRigidbody = GetComponent<Rigidbody>();
		cachedHitQuiver = GetComponentInChildren<HitQuiver>();
		navMeshQueryFilter.areaMask = -1;
		NavMeshAgent component2 = GetComponent<NavMeshAgent>();
		if ((bool)component2)
		{
			navMeshQueryFilter.agentTypeID = component2.agentTypeID;
		}
		creature_Awake();
		initialRigidbodyConstraints = cachedRigidbody.constraints;
		megaflinchAnimationStateExists = cachedAnimator.HasState(0, Animator.StringToHash("megaflinch"));
		cachedRigidbody.useGravity = false;
	}

	protected virtual void creature_Awake()
	{
	}

	protected virtual void creature_datSetup()
	{
	}

	protected virtual void onUnfreeze()
	{
		if (!IsDead)
		{
			Flinch();
		}
	}

	protected void FixedUpdate()
	{
		if ((bool)materialManager)
		{
			CreatureMaterialManager.IceState icy = (Frozen ? CreatureMaterialManager.IceState.ICE : CreatureMaterialManager.IceState.NO_ICE);
			materialManager.Icy = icy;
			materialManager.FlickerEnabled = MercyInvincible;
			for (int i = 0; i < extraMaterialManagers.Length; i++)
			{
				extraMaterialManagers[i].Icy = icy;
				extraMaterialManagers[i].FlickerEnabled = MercyInvincible;
			}
		}
		if ((bool)cachedHitQuiver)
		{
			cachedHitQuiver.SteadVibration = Frozen && Time.time > frozenUntil - 1f;
		}
		if (wasFrozen && !Frozen)
		{
			if (!IsDead)
			{
				unfreezeBlast();
			}
			onUnfreeze();
		}
		wasFrozen = Frozen;
		float num = 1f;
		if (Paused)
		{
			num = 0f;
		}
		if (Frozen)
		{
			float num2 = Mathf.Clamp01((Time.time - freezeStartTime) / 1.5f);
			num2 = 1f - Mathf.Pow(1f - num2, 2f);
			num = Mathf.InverseLerp(0.5f, 0f, num2);
		}
		freezeEffects = Frozen;
		cachedAnimator.speed = creature_AnimatorSpeed * num;
		if (isParriable)
		{
			parryStunEmission.enabled = IsParryStunned;
		}
		if (!IsDead)
		{
			if (Time.time > lastPoiseDamageTime + poiseRefillDelay.Value && Time.deltaTime > 0f)
			{
				_poise = Mathf.Min(maxPoise, _poise + (float)maxPoise * Time.deltaTime);
			}
			if (base.transform.position.y < kKillFloorAltitude)
			{
				Debug.LogFormat("Creature {0} hit the killfloor.", base.name);
				hp -= hp;
				hp--;
			}
			if (poisonCounter > 0f)
			{
				float num3 = Mathf.Floor(poisonCounter);
				poisonCounter -= kPoisonReductionRate * Time.deltaTime;
				if (num3 > Mathf.Floor(poisonCounter))
				{
					hp--;
				}
				if (poisonParticleSystem != null)
				{
					ParticleSystem.EmissionModule emission = poisonParticleSystem.emission;
					emission.enabled = true;
				}
			}
			else if (poisonParticleSystem != null)
			{
				ParticleSystem.EmissionModule emission2 = poisonParticleSystem.emission;
				emission2.enabled = false;
			}
			creature_FixedUpdate();
		}
		if (Frozen)
		{
			cachedRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
		else
		{
			cachedRigidbody.constraints = initialRigidbodyConstraints;
		}
		lastPosition = base.transform.position;
		lastRotation = base.transform.rotation;
	}

	protected virtual void creature_FixedUpdate()
	{
	}

	protected virtual void onFall()
	{
	}

	protected void OnAnimatorMove()
	{
		if (base.gameObject == null)
		{
			return;
		}
		if (!isPhysicsBased && !beingTetherDragged && !InConduit)
		{
			bool flag = false;
			flag = suppressNavMeshSnapping || doNavMeshSnappingAndReturnTrueIfGravityShouldBeApplied();
			Vector3 zero = Vector3.zero;
			if (flag)
			{
				verticalVelocity += gravity * Time.deltaTime;
				float num = verticalVelocity * Time.deltaTime;
				float num2 = 1f;
				RaycastHit hitInfo;
				if (!Physics.Raycast(base.transform.position + Vector3.up * num2, Vector3.down, out hitInfo, num + num2, groundingLayerMask, QueryTriggerInteraction.Ignore))
				{
					zero += Vector3.down * verticalVelocity;
					bool flag2 = Physics.Raycast(base.transform.position, Vector3.down, fallRaycastLength, groundingLayerMask, QueryTriggerInteraction.Ignore);
					if (verticalVelocity > verticalVelocityFallThreshold && !flag2)
					{
						onFall();
					}
				}
				else if (Time.deltaTime > 0f)
				{
					float num3 = hitInfo.distance - num2;
					zero += Vector3.down * num3 / Time.deltaTime;
					verticalVelocity = 0f;
				}
			}
			if (Frozen || SuppressPushbuffer)
			{
				zero = Vector3.zero;
			}
			else if (pushBufferDamping > 0f)
			{
				if (IsDead)
				{
					zero += pushBuffer / 8f;
				}
				else
				{
					zero += pushBuffer;
				}
			}
			pushBuffer *= Mathf.Pow(pushBufferDamping, Time.deltaTime);
			cachedRigidbody.velocity = zero;
			cachedRigidbody.angularVelocity = Vector3.zero;
		}
		if (!beingTetherDragged)
		{
			creature_OnAnimatorMove();
		}
		else if (Time.deltaTime > 0f)
		{
			verticalVelocity = 0f;
			Vector3 vector = tetherDragTarget - base.transform.position;
			Vector3 velocity = vector.normalized * tetherDragSpeed;
			if (velocity.magnitude * Time.deltaTime > vector.magnitude)
			{
				cachedRigidbody.velocity = vector / Time.deltaTime;
				beingTetherDragged = false;
				if (tetherSuppressesNavMeshSnappingOnEnd)
				{
					timeSinceLastNavMeshSupressingTetherEnd = Time.time;
				}
			}
			else
			{
				cachedRigidbody.velocity = velocity;
			}
		}
		if (InWater)
		{
			cachedRigidbody.velocity *= SlowInWaterFactor;
			if (!inWaterFMODEventInstance.isValid())
			{
				inWaterFMODEventInstance = SFX.GetInstance(inWaterLoopSFX, base.transform, cachedRigidbody);
				inWaterSpeedParamID = SFX.GetParamID(inWaterFMODEventInstance, "fox_wade_speed");
			}
			if (!SFX.EventIsPlaying(inWaterFMODEventInstance))
			{
				inWaterFMODEventInstance.start();
			}
			inWaterFMODEventInstance.setParameterByID(inWaterSpeedParamID, Velocity.magnitude);
			waterFringeParticleEmission = true;
			_waterFringeFX.transform.position = base.transform.position + Vector3.up * (WaterWorldAltitude - base.transform.position.y);
		}
		else
		{
			if (inWaterFMODEventInstance.isValid())
			{
				inWaterFMODEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			waterFringeParticleEmission = false;
		}
		previousPosition = base.transform.position;
	}

	protected virtual void creature_OnAnimatorMove()
	{
	}

	protected virtual void OnDestroy()
	{
		if (OnDestroyCallbacks != null)
		{
			OnDestroyCallbacks();
		}
		if ((bool)_waterFringeFX)
		{
			_waterFringeFX.Kill();
		}
		inWaterFMODEventInstance.release();
	}

	protected void OnDrawGizmos()
	{
	}

	protected void debugLabel(string msg)
	{
		if (!Profile.GetDemoMode())
		{
			if (textMesh == null)
			{
				textMesh = createDebugTextMesh(Color.yellow);
			}
			textMesh.text = msg;
		}
	}

	protected void debugEvent(string msg)
	{
		if (!Profile.GetDemoMode())
		{
			TextMesh obj = createDebugTextMesh(Color.red);
			obj.text = msg;
			obj.transform.localPosition = Vector3.up * 4f;
			obj.gameObject.AddComponent<MoveUp>();
			UnityEngine.Object.Destroy(obj.gameObject, 1f);
		}
	}

	protected TextMesh createDebugTextMesh(Color c)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = -Vector3.up;
		TextMesh obj = gameObject.AddComponent<TextMesh>();
		obj.alignment = TextAlignment.Center;
		obj.color = c;
		obj.characterSize = 0.75f;
		obj.anchor = TextAnchor.UpperCenter;
		gameObject.AddComponent<FaceMainCamera>();
		return obj;
	}

	protected void setUpIceEffects()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(kPathToSnowParticleSystem, typeof(GameObject)));
		snowParticleSystem = gameObject.GetComponent<ParticleSystem>();
		snowParticleSystemEmission = snowParticleSystem.emission;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		SkinnedMeshRenderer componentInChildren = GetComponentInChildren<SkinnedMeshRenderer>();
		if (componentInChildren != null)
		{
			Bounds localBounds = componentInChildren.localBounds;
			gameObject.transform.localPosition = localBounds.center;
			ParticleSystem.ShapeModule shape = snowParticleSystem.shape;
			shape.scale = localBounds.size;
			float a = localBounds.size.x * localBounds.size.y * localBounds.size.z;
			a = Mathf.Min(a, 100f);
			snowParticleSystemEmission.rateOverTimeMultiplier = a;
			if (iceBaseGameObject == null)
			{
				UnityEngine.Object original = Resources.Load(kPathToIceBaseObject, typeof(GameObject));
				iceBaseGameObject = (GameObject)UnityEngine.Object.Instantiate(original);
				iceBaseGameObject.transform.parent = base.transform;
				iceBaseGameObject.transform.localPosition = Vector3.zero;
				float num = Mathf.Min(localBounds.size.x, localBounds.size.y, localBounds.size.z);
				iceBaseGameObject.transform.localScale = Vector3.one * num;
				HitQuiver componentInChildren2 = GetComponentInChildren<HitQuiver>();
				if ((bool)componentInChildren2)
				{
					iceBaseGameObject.transform.parent = componentInChildren2.transform;
				}
			}
		}
		else
		{
			Debug.LogWarning(string.Format("`{0}` doesn't have a SMR to get size from.", base.name));
		}
		iceEffectsSetUp = true;
	}

	protected void unfreezeBlast()
	{
		CapsuleCollider component = GetComponent<CapsuleCollider>();
		float radius = 6f * UnfreezeBlastRadiusFactor;
		RaycastHit[] array = Physics.SphereCastAll(base.transform.position, radius, Vector3.up, 0.1f, -1);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].collider != component)
			{
				HitReceiver component2 = array[i].collider.GetComponent<HitReceiver>();
				if (component2 != null)
				{
					component2.ReceiveHit(HitType.NORMAL, 0, -1f, 0, 0, 0f, 0f, true, false, base.transform.position, 25f * UnfreezeBlastRadiusFactor, base.transform.position, Quaternion.identity, false, null, null, -1);
				}
			}
		}
		GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("UnfreezeBlast") as GameObject);
		obj.transform.position = base.transform.position;
		obj.transform.localScale = Vector3.one * UnfreezeBlastRadiusFactor;
		UnityEngine.Object.Destroy(obj.gameObject, 3f);
	}
}
