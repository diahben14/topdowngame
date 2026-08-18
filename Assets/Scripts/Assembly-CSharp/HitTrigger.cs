using UnityEngine;

public class HitTrigger : MonoBehaviour
{
	[SerializeField]
	protected string id = "monster or weapon name";

	[SerializeField]
	protected Transform originOfForce;

	[SerializeField]
	protected Animator animatorForAttackBlockedReaction;

	[SerializeField]
	protected bool unblockable;

	[SerializeField]
	protected bool ignoreIFrames;

	[Header("Effects")]
	[SerializeField]
	protected GameObject successfulHitEffectTemplate;

	[SerializeField]
	protected GameObject blockedHitEffectRoot;

	[SerializeField]
	protected Transform effectSpawnTransform;

	[SerializeField]
	private float fastScreenShakeOnStaminafullHit = 0.25f;

	protected Dat hpDamagePayload_dat;

	protected Dat attackPower_dat;

	protected Dat forcePayload_dat;

	protected Dat poiseAttackPowerDelta_dat;

	protected Dat blockAttackPowerDelta_dat;

	protected Dat freezeTimeToAdd_dat;

	protected Dat poisonPayload_dat;

	protected Dat damageType_dat;

	protected Dat powerLevel_dat;

	protected Creature heldCreature;

	protected PlayerCharacter heldPlayerCharacter;

	protected PooledFX impactPool;

	protected virtual Vector3 OriginOfForce
	{
		get
		{
			if (!(originOfForce != null))
			{
				return base.transform.position;
			}
			return originOfForce.position;
		}
	}

	protected virtual bool doHitsOnTriggerStay
	{
		get
		{
			return false;
		}
	}

	public bool Potent { get; protected set; }

	protected virtual void HitWasSuccess(Vector3 impactPosition)
	{
		if ((bool)successfulHitEffectTemplate)
		{
			Quaternion rot = (effectSpawnTransform ? effectSpawnTransform.rotation : Quaternion.identity);
			PooledFX.GetNext<GameObject>(impactPosition, rot, ref impactPool, base.name + "-Impact", successfulHitEffectTemplate, 4, false);
			if ((bool)heldPlayerCharacter && heldPlayerCharacter.StaminaEffects)
			{
				CameraController.AddFastScreenshake(fastScreenShakeOnStaminafullHit);
			}
		}
		if ((bool)heldPlayerCharacter)
		{
			heldPlayerCharacter.RootMotionHitCancelled = true;
			if (StaminaSettings.HitsRestoreStamina)
			{
				heldPlayerCharacter.GiveStaminaFromHit();
			}
		}
	}

	protected virtual void HitWasBlocked(Vector3 impactPosition, bool parry)
	{
		if ((bool)animatorForAttackBlockedReaction)
		{
			animatorForAttackBlockedReaction.SetTrigger("sword bounceback");
			animatorForAttackBlockedReaction.Play("recoil", 0);
			if (parry)
			{
				TimeManager.FreezeFrame();
				CameraController.AddParryEffect();
				animatorForAttackBlockedReaction.Play("parry", 0);
			}
		}
		if ((bool)blockedHitEffectRoot)
		{
			Quaternion rotation = (effectSpawnTransform ? effectSpawnTransform.rotation : base.transform.rotation);
			blockedHitEffectRoot.transform.position = impactPosition;
			blockedHitEffectRoot.transform.rotation = rotation;
			blockedHitEffectRoot.SetActive(true);
		}
	}

	protected void Awake()
	{
		hpDamagePayload_dat = new Dat("dmg_hp", id);
		attackPower_dat = new Dat("att_pow", id);
		forcePayload_dat = new Dat("dmg_force", id);
		poiseAttackPowerDelta_dat = new Dat("pp_pow", id);
		blockAttackPowerDelta_dat = new Dat("block_pow", id);
		freezeTimeToAdd_dat = new Dat("freeze_time", id);
		poisonPayload_dat = new Dat("poison", id);
		damageType_dat = new Dat("dmg_type", id, typeof(HitType));
		powerLevel_dat = new Dat("level", id);
		Potent = true;
		Transform parent = base.transform;
		while (parent != null && heldCreature == null)
		{
			heldCreature = parent.GetComponent<Creature>();
			parent = parent.parent;
		}
		if (heldCreature is PlayerCharacter)
		{
			heldPlayerCharacter = heldCreature as PlayerCharacter;
		}
		hitTrigger_Awake();
		Collider[] components = GetComponents<Collider>();
		if (components.Length == 0)
		{
			Debug.LogError("DamageTrigger has no collider!", this);
			return;
		}
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i].isTrigger)
			{
				return;
			}
		}
		Debug.LogError("DamageTrigger's has no trigger collider!", this);
	}

	protected virtual void hitTrigger_Awake()
	{
	}

	protected void OnTriggerEnter(Collider c)
	{
		doHit(c);
	}

	protected void OnTriggerStay(Collider c)
	{
		if (doHitsOnTriggerStay)
		{
			doHit(c);
		}
	}

	protected void doHit(Collider c)
	{
		if (Time.timeSinceLevelLoad < Time.deltaTime * 2f || !Potent || (heldCreature != null && heldCreature.Frozen))
		{
			return;
		}
		bool flag = false;
		HitReceiver[] components = c.GetComponents<HitReceiver>();
		foreach (HitReceiver hitReceiver in components)
		{
			if (hitReceiver != null)
			{
				flag = true;
				Debug.DrawRay(OriginOfForce, Vector3.up, Color.magenta, 1f);
				Debug.DrawLine(OriginOfForce, hitReceiver.transform.position, Color.magenta, 1f);
				Debug.DrawRay(hitReceiver.transform.position, Vector3.up, Color.white, 1f);
				Vector3 effectSpawnPosition = ((effectSpawnTransform != null) ? effectSpawnTransform.position : base.transform.position);
				Quaternion effectSpawnRotation = ((effectSpawnTransform != null) ? effectSpawnTransform.rotation : Quaternion.identity);
				float num = hpDamagePayload_dat.Value;
				float num2 = forcePayload_dat.Value;
				int num3 = poiseAttackPowerDelta_dat.IntValue;
				int intValue = blockAttackPowerDelta_dat.IntValue;
				if ((bool)heldCreature)
				{
					num *= heldCreature.DamageFactor;
					num2 *= heldCreature.HitForceFactor;
					num3 += heldCreature.PoisePowerAdjust;
				}
				float attackPower = (heldPlayerCharacter ? PlayerCharacter.AttackPower : attackPower_dat.Value);
				hitReceiver.ReceiveHit((HitType)damageType_dat.EnumValue, Mathf.RoundToInt(num), attackPower, num3, intValue, freezeTimeToAdd_dat.Value, poisonPayload_dat.Value, unblockable, ignoreIFrames, OriginOfForce, num2, effectSpawnPosition, effectSpawnRotation, heldPlayerCharacter != null, HitWasSuccess, HitWasBlocked, powerLevel_dat.IntValue);
			}
		}
		if (!flag)
		{
			triggerEnterWithNotHitReceiver(c);
		}
	}

	protected virtual void triggerEnterWithNotHitReceiver(Collider c)
	{
	}
}
