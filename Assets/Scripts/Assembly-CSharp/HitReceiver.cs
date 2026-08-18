using System;
using UnityEngine;

public class HitReceiver : MonoBehaviour
{
	public delegate void SuccessInfoToHitterDelegate(Vector3 position);

	public delegate void BlockInfoToHitterDelegate(Vector3 position, bool parry);

	[SerializeField]
	protected string id = "";

	protected Dat damageReduction;

	protected Dat[] damageTypeHurtMultipliers;

	protected Dat defensePower;

	[SerializeField]
	protected bool debug_ignoreAllHits;

	[Header("In-Editor Animatable")]
	[SerializeField]
	protected bool blockEnabled = true;

	[SerializeField]
	protected float blockingArc;

	[SerializeField]
	protected float blockingArcOffset;

	[Header("Import Curves")]
	[SerializeField]
	protected bool useImportCurves;

	[SerializeField]
	protected string blockingArcParameterName = "AP_blockingArc";

	[Header("Responses")]
	[SerializeField]
	protected bool canParry;

	[SerializeField]
	protected Creature creature;

	[SerializeField]
	protected Animator animatorForHitTrigger;

	[SerializeField]
	protected Rigidbody rigidBodyForReceivedForce;

	[SerializeField]
	protected float forceFactorWhenBlocked = 0.5f;

	[Header("Visual Effects")]
	[SerializeField]
	protected GameObject hitEffect;

	[SerializeField]
	protected GameObject blockEffect;

	[Header("Talk back to Hitter")]
	[SerializeField]
	protected bool informHitterOnSuccess = true;

	[SerializeField]
	protected bool informHitterOnBlock = true;

	protected IDamageable damageable;

	protected SmashableObject cachedSmashableObject;

	protected PlayerCharacter cachedPlayerCharacter;

	private float[] powerMatrix = new float[8] { 0.33f, 0.5f, 0.7f, 1f, 1.33f, 1.66f, 2f, 2.5f };

	private float[] powerMatrix_foxDamageOutput = new float[8] { 0.5f, 0.66f, 0.8f, 1f, 1.33f, 1.66f, 2f, 2.5f };

	public void switchDatID(string newID)
	{
		id = newID;
		populateDats();
	}

	protected void Awake()
	{
		populateDats();
		Transform transform = base.transform;
		while (transform != null)
		{
			damageable = (IDamageable)transform.GetComponent(typeof(IDamageable));
			transform = ((damageable != null) ? null : transform.parent);
		}
		cachedPlayerCharacter = GetComponent<PlayerCharacter>();
		cachedSmashableObject = GetComponent<SmashableObject>();
	}

	protected void populateDats()
	{
		string[] names = Enum.GetNames(typeof(HitType));
		if (string.IsNullOrEmpty(id))
		{
			Debug.LogErrorFormat("No string id on {0}", base.name, this);
		}
		damageReduction = new Dat("dr", id);
		damageTypeHurtMultipliers = new Dat[names.Length];
		for (int i = 0; i < names.Length; i++)
		{
			damageTypeHurtMultipliers[i] = new Dat("hurt_" + names[i], id);
		}
		defensePower = new Dat("def_pow", id);
	}

	public void ReceiveHit(HitType hitType, int damagePoints, float attackPower, int poisePowerAdjust, int blockPowerAdjust, float frozenTimeToAdd, float poisonToAdd, bool unblockable, bool ignoreIFrames, Vector3 hitPosition, float forceFactor, Vector3 effectSpawnPosition, Quaternion effectSpawnRotation, bool isPlayerCharacterMelee, SuccessInfoToHitterDelegate successDelegate, BlockInfoToHitterDelegate blockDelegate, int powerLevel)
	{
		if (debug_ignoreAllHits || (creature != null && creature.IsDead))
		{
			return;
		}
		float num = damageTypeHurtMultipliers[(int)hitType].Value;
		if (num.IsApproxZero())
		{
			return;
		}
		bool flag = false;
		if (num.IsApprox(-1f))
		{
			flag = true;
			num = 0f;
		}
		if ((creature != null && (creature.MercyInvincible || creature.Invulnerable || (creature.IFrameInvincible && !ignoreIFrames) || (powerLevel != -1 && powerLevel <= creature.PowerLevel))) || ((bool)cachedSmashableObject && cachedSmashableObject.MercyInvincible))
		{
			return;
		}
		Vector3 a = hitPosition - base.transform.position;
		Vector3 normalized = Vector3.Scale(a, Mathx.flatteningVector).normalized;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = (bool)creature && creature.SuppressBlocking;
		float num2 = (useImportCurves ? (animatorForHitTrigger.GetFloat(blockingArcParameterName) * blockingArc) : blockingArc);
		if (num2 > 0f && blockEnabled && !flag4)
		{
			if (num2 >= 360f)
			{
				flag2 = true;
			}
			else
			{
				Debug.DrawLine(base.transform.position, hitPosition, Color.blue);
				Vector3 to = Vector3.Scale(Quaternion.Euler(0f, blockingArcOffset, 0f) * base.transform.forward, Mathx.flatteningVector);
				if (Vector3.Angle(normalized, to) < num2 / 2f)
				{
					flag2 = true;
					if (canParry && num2 < 100f)
					{
						flag3 = true;
					}
				}
			}
		}
		float num3 = defensePower.Value;
		if ((bool)cachedPlayerCharacter)
		{
			num3 = PlayerCharacter.DefensePower;
		}
		if ((bool)creature)
		{
			num3 = creature.ModifyDefensePower(num3);
		}
		float num4 = 1f;
		float num5 = 1f;
		float num6 = 1f;
		float num7 = 0f;
		num7 = ((!(attackPower > -1f) || !(num3 > -1f)) ? 0f : (attackPower - num3));
		num4 = mapPowerDeltaToModifier(num7, isPlayerCharacterMelee);
		num5 = mapPowerDeltaToModifier(num7 + (float)poisePowerAdjust, isPlayerCharacterMelee);
		num6 = mapPowerDeltaToModifier(num7 + (float)blockPowerAdjust, isPlayerCharacterMelee);
		int num8 = 0;
		if (damagePoints > 0)
		{
			num8 = Mathf.RoundToInt((float)Mathf.Max(damagePoints, 1) * num * num4);
		}
		int num9 = Mathf.RoundToInt((float)Mathf.Max(damagePoints, 0) * num * num5);
		int staminaPoints = Mathf.RoundToInt((float)Mathf.Max(damagePoints, 0) * num * num6);
		if (flag3)
		{
			num8 = 0;
			num9 = 0;
		}
		if (frozenTimeToAdd > 0f && id == "forcefield")
		{
			num8 = 1000;
		}
		if ((bool)creature)
		{
			num8 = creature.ModifyReceivedDamage(num8);
			num9 = creature.ModifyReceivedDamage(num9);
		}
		Vector3 vector = -a.normalized * forceFactor;
		if ((flag2 || flag) && !unblockable)
		{
			if (flag3)
			{
				vector = Vector3.zero;
			}
			else
			{
				vector *= forceFactorWhenBlocked;
			}
			if (damageable != null)
			{
				damageable.BlockDamage(staminaPoints, vector);
			}
			if (informHitterOnBlock && blockDelegate != null)
			{
				blockDelegate(effectSpawnPosition, flag3);
			}
			if ((bool)blockEffect)
			{
				UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(blockEffect, base.transform.position, Quaternion.identity), 1f);
			}
		}
		else
		{
			vector = -normalized * forceFactor;
			if (damageable != null)
			{
				bool flag5 = damageable.ReceiveDamage(num8, num9, vector, frozenTimeToAdd * num, (int)poisonToAdd);
				if (isPlayerCharacterMelee && creature != null)
				{
					if (flag5)
					{
						TimeManager.FreezeFrame();
					}
					else
					{
						TimeManager.FreezeFrame();
					}
				}
				if (creature != null)
				{
					HitQuiver componentInChildren = GetComponentInChildren<HitQuiver>();
					if (componentInChildren != null)
					{
						if (!creature.Frozen)
						{
							componentInChildren.Quiver();
						}
						else if (frozenTimeToAdd == 0f)
						{
							componentInChildren.VibrationPulse();
						}
					}
				}
			}
			if ((bool)hitEffect)
			{
				UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(hitEffect, base.transform.position, Quaternion.identity), 1f);
			}
			if ((bool)animatorForHitTrigger && !creature)
			{
				Debug.Log("creatureless hit trigger receiving `hit' crossfade.");
				animatorForHitTrigger.CrossFade("hit", 0f);
			}
			if (hitType == HitType.FIRE)
			{
				Flammable component = GetComponent<Flammable>();
				if (component != null)
				{
					component.FireAmount += (float)damagePoints * num;
				}
			}
			if (informHitterOnSuccess && successDelegate != null)
			{
				successDelegate(effectSpawnPosition);
			}
		}
		if (vector.magnitude > 0f && (bool)rigidBodyForReceivedForce)
		{
			rigidBodyForReceivedForce.AddForce(vector, ForceMode.Impulse);
		}
	}

	private float mapPowerDeltaToModifier(float powerDelta, bool isFoxAttack)
	{
		int num = Mathf.RoundToInt(Mathf.Clamp(powerDelta, -3f, 4f) + 3f);
		if (isFoxAttack)
		{
			return powerMatrix_foxDamageOutput[num];
		}
		return powerMatrix[num];
	}

	public bool isImmuneToOrAlwaysBlocksDamageType(HitType type)
	{
		float value = damageTypeHurtMultipliers[(int)type].Value;
		if (value.IsApproxZero())
		{
			return true;
		}
		if (value.IsApprox(-1f))
		{
			return true;
		}
		return false;
	}
}
