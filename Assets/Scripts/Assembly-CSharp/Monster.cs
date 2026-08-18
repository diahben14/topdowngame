using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RuntimeStableID))]
public class Monster : Creature
{
	[Header("Monster")]
	[SerializeField]
	protected StateVariable isDeadPermanentlyStateVar;

	[SerializeField]
	protected float secondsBeforeDespawn;

	[SerializeField]
	protected GameObject deathEffect;

	[SerializeField]
	protected bool destroyDeathEffect = true;

	[SerializeField]
	protected Transform effectSpawnPoint;

	[SerializeField]
	protected bool runtimStayDead = true;

	[SerializeField]
	protected LayerMask playerVisibilityRaycast;

	[Header("Monster - Aggro")]
	[SerializeField]
	protected float monsterAggroDistance = 10f;

	[SerializeField]
	protected float monsterMaxSightDistance = 30f;

	[SerializeField]
	protected float monsterAggroAngle = 180f;

	[SerializeField]
	protected bool needLineOfSightToAggro = true;

	[SerializeField]
	protected float closeAggroDistance;

	[SerializeField]
	protected float aggroBroadcastRadius = 15f;

	[SerializeField]
	protected Transform aggroEffectTransform;

	[SerializeField]
	protected float aggroEffectHeight = 2f;

	[SerializeField]
	[EventRef]
	protected string aggroSFX;

	[SerializeField]
	protected int aggroLayer;

	[SerializeField]
	protected AggroTrigger overridingAggroTrigger;

	[SerializeField]
	protected Monster[] broadcastAggroTo;

	[SerializeField]
	protected bool onlyAggroViaTrigger;

	[Header("Monster - Infighting")]
	[SerializeField]
	protected Creature overridingTarget;

	protected const float kSpeedOfAggro = 30f;

	protected static List<int> runtimeDeadMonsters = new List<int>();

	private bool _sleepingDueToPlayerDistance;

	protected NavMeshAgent nma;

	protected RuntimeStableID runtimeStableID;

	protected MonsterStateBehaviour[] stateBehaviours;

	protected Dat animatorSpeedMultiplier;

	protected Dat dropValue;

	protected Dat mpDrop;

	protected bool _monsterAggroed;

	public Action OnAggroChangeCallbacks;

	private bool _paused;

	public int AggroLayer
	{
		get
		{
			return aggroLayer;
		}
	}

	public bool SleepingDueToPlayerDistance
	{
		get
		{
			if (_sleepingDueToPlayerDistance)
			{
				return !neverSleepDueToPlayerDistance;
			}
			return false;
		}
		set
		{
			_sleepingDueToPlayerDistance = value;
		}
	}

	public bool IsAggroed
	{
		get
		{
			return monsterAggroed;
		}
		set
		{
			monsterAggroed = true;
		}
	}

	protected bool monsterAggroed
	{
		get
		{
			return _monsterAggroed;
		}
		set
		{
			if (value && !_monsterAggroed)
			{
				cachedAnimator.SetBool("aggro", value);
				AlertEffect next = GlobalPools.Instance.AlertFX.GetNext<AlertEffect>(base.transform.position, Quaternion.identity);
				if ((bool)next)
				{
					next.SetFollowTransformAndHeight((aggroEffectTransform != null) ? aggroEffectTransform : base.transform, aggroEffectHeight);
					next.StartSequence();
				}
				SFX.PlayAudioClipHere(aggroSFX, base.transform.position);
				_monsterAggroed = true;
				onAggro();
				if (OnAggroChangeCallbacks != null)
				{
					OnAggroChangeCallbacks();
				}
			}
			if (!value && _monsterAggroed)
			{
				_monsterAggroed = false;
				cachedAnimator.SetBool("aggro", value);
				if (OnAggroChangeCallbacks != null)
				{
					OnAggroChangeCallbacks();
				}
			}
		}
	}

	protected bool busy
	{
		get
		{
			for (int i = 0; i < stateBehaviours.Length; i++)
			{
				if (stateBehaviours[i].Busy)
				{
					return true;
				}
			}
			return false;
		}
	}

	protected Vector3 playerOrBaitWorldPosition
	{
		get
		{
			if ((bool)overridingTarget)
			{
				return overridingTarget.transform.position;
			}
			if (PlayerCharacter.Instanced)
			{
				if (Bait.LatestBate != null)
				{
					return Bait.LatestBate.transform.position;
				}
				return PlayerCharacter.WorldPosition;
			}
			Debug.LogError("Tried to get player position but didn't check PlayerCharacter.Instanced");
			return Vector3.zero;
		}
	}

	protected virtual Vector3 losRaycastOrigin
	{
		get
		{
			return base.transform.position + Vector3.up;
		}
	}

	protected virtual bool receiveBroadcastAggro
	{
		get
		{
			return !onlyAggroViaTrigger;
		}
	}

	protected virtual bool removeZTargetOnDeath
	{
		get
		{
			return true;
		}
	}

	protected virtual bool neverSleepDueToPlayerDistance
	{
		get
		{
			return false;
		}
	}

	public override bool Paused
	{
		get
		{
			if (!Campfire.SleepSequenceRunning)
			{
				return _paused;
			}
			return true;
		}
		set
		{
			_paused = value;
		}
	}

	protected override float creature_AnimatorSpeed
	{
		get
		{
			return animatorSpeedMultiplier.Value;
		}
	}

	public static void ClearRuntimeDeadMonsters()
	{
		runtimeDeadMonsters.Clear();
	}

	public void ReceiveBroadcastAggro()
	{
		ReceiveBroadcastAggro(0f);
	}

	public void ReceiveBroadcastAggro(float distance)
	{
		if (receiveBroadcastAggro)
		{
			StartCoroutine(aggroAfterDelay(distance / 30f));
		}
	}

	protected IEnumerator aggroAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		monsterAggroed = true;
	}

	public void ReceiveAggroTriggerAggro()
	{
		monsterAggroed = true;
	}

	internal void Heal(float healAmount)
	{
		hp += (int)healAmount;
	}

	protected bool canSeePlayerOrBaitCheck()
	{
		if (PlayerCharacter.Instanced)
		{
			Vector3 vector = playerOrBaitWorldPosition - losRaycastOrigin;
			if (vector.magnitude > monsterMaxSightDistance)
			{
				return false;
			}
			RaycastHit hitInfo;
			if (!Physics.Raycast(losRaycastOrigin, vector + Vector3.up, out hitInfo, vector.magnitude, playerVisibilityRaycast))
			{
				Debug.DrawRay(losRaycastOrigin, vector + Vector3.up, Color.green);
				return true;
			}
			if (hitInfo.collider.gameObject.tag == "Player")
			{
				return true;
			}
		}
		return false;
	}

	protected virtual void BehaviourUpdate()
	{
	}

	protected virtual void monster_Start()
	{
	}

	protected virtual void onAggro()
	{
	}

	protected override IEnumerator Die()
	{
		cachedAnimator.SetTrigger("death");
		cachedAnimator.CrossFade("death", 0f, 0);
		if (runtimStayDead && runtimeStableID.HasValidID)
		{
			runtimeDeadMonsters.Add(runtimeStableID.ID);
		}
		if (isDeadPermanentlyStateVar != null)
		{
			isDeadPermanentlyStateVar.BoolValue = true;
		}
		ZTarget component = GetComponent<ZTarget>();
		if (component != null && removeZTargetOnDeath)
		{
			UnityEngine.Object.Destroy(component);
		}
		yield return 0;
		CoinSpawner.SpawnCoins(PlayerCharacter.GetCoinDropFromValue(dropValue.Value), base.transform.position);
		yield return new WaitForSeconds(secondsBeforeDespawn);
		Collider component2 = GetComponent<Collider>();
		if ((bool)component2)
		{
			component2.enabled = false;
		}
		if ((bool)deathEffect)
		{
			Quaternion rotation = Quaternion.LookRotation(pushBuffer.normalized);
			GameObject obj = UnityEngine.Object.Instantiate(deathEffect, effectSpawnPoint ? effectSpawnPoint.position : base.transform.position, rotation);
			if (destroyDeathEffect)
			{
				UnityEngine.Object.Destroy(obj, 3f);
			}
		}
		if (mpDrop.Value > 0f)
		{
			MPPickup.Drop(mpDrop.Value, base.transform.position);
		}
		monster_onDie();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected override void ReceivedDamageEffects(bool brokePoise, int damagePoints)
	{
		monsterAggroed = true;
		if (!base.Frozen)
		{
			MonsterManager.BroadcastAggro(base.transform.position, aggroBroadcastRadius, aggroLayer);
			shareAggro();
		}
		monster_receviedDamageEffects();
	}

	protected virtual void monster_receviedDamageEffects()
	{
	}

	protected virtual void monster_onDie()
	{
	}

	protected override void BlockDamageEffects()
	{
		monsterAggroed = true;
		MonsterManager.BroadcastAggro(base.transform.position, aggroBroadcastRadius, aggroLayer);
		shareAggro();
	}

	public override void ResetAnimationStates()
	{
		for (int i = 0; i < stateBehaviours.Length; i++)
		{
			stateBehaviours[i].Reset();
		}
	}

	protected override void creature_datSetup()
	{
		animatorSpeedMultiplier = new Dat("animatorSpeed", dataRow);
		dropValue = new Dat("dropValue", dataRow);
		mpDrop = new Dat("mp_drop", dataRow);
	}

	protected override void creature_Awake()
	{
		float num2 = (maxStamina = float.PositiveInfinity);
		base.stamina = num2;
		stateBehaviours = cachedAnimator.GetBehaviours<MonsterStateBehaviour>();
		bool flag = false;
		if (isDeadPermanentlyStateVar != null && isDeadPermanentlyStateVar.BoolValue)
		{
			flag = true;
		}
		if (runtimStayDead)
		{
			runtimeStableID = GetComponent<RuntimeStableID>();
			if ((bool)runtimeStableID)
			{
				if (runtimeStableID.HasValidID && runtimeDeadMonsters.Contains(runtimeStableID.ID))
				{
					flag = true;
				}
			}
			else
			{
				Debug.LogError("No runtime stable id component on " + base.name);
			}
		}
		if (flag)
		{
			alreadyDeadOnAwakeHandler();
		}
		if ((bool)overridingAggroTrigger)
		{
			overridingAggroTrigger.Register(this);
		}
	}

	protected virtual void alreadyDeadOnAwakeHandler()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected void Start()
	{
		monster_Start();
		MonsterManager.RegisterMonster(this);
	}

	protected override void OnDestroy()
	{
		base.IsDead = true;
		base.OnDestroy();
		MonsterManager.DeregisterMonster(this);
		monster_OnDestroy();
	}

	protected virtual void monster_OnDestroy()
	{
	}

	protected override void creature_FixedUpdate()
	{
		if (SleepingDueToPlayerDistance || base.IsDead || base.Frozen)
		{
			return;
		}
		if (PlayerCharacter.Instanced && !monsterAggroed && !PlayerCharacter.InstanceInConduit && (!needLineOfSightToAggro || canSeePlayerOrBaitCheck()) && !onlyAggroViaTrigger)
		{
			if (Vector3.Distance(base.transform.position, playerOrBaitWorldPosition) < monsterAggroDistance && Vector3.Angle(base.transform.forward, playerOrBaitWorldPosition - base.transform.position) < monsterAggroAngle)
			{
				monsterAggroed = true;
				MonsterManager.BroadcastAggro(base.transform.position, aggroBroadcastRadius, aggroLayer);
				shareAggro();
			}
			if (Vector3.Distance(base.transform.position, playerOrBaitWorldPosition) < closeAggroDistance)
			{
				monsterAggroed = true;
				MonsterManager.BroadcastAggro(base.transform.position, aggroBroadcastRadius, aggroLayer);
				shareAggro();
			}
		}
		BehaviourUpdate();
	}

	protected void shareAggro()
	{
		for (int i = 0; i < broadcastAggroTo.Length; i++)
		{
			if (broadcastAggroTo[i] != null)
			{
				broadcastAggroTo[i].ReceiveBroadcastAggro();
			}
		}
	}

	protected void __flash()
	{
		materialManager.AttackFlash();
		for (int i = 0; i < extraMaterialManagers.Length; i++)
		{
			extraMaterialManagers[i].AttackFlash();
		}
	}
}
