using UnityEngine;

public class WizardHitTrigger : MonoBehaviour
{
	[SerializeField]
	protected string id = "wizard_aoe";

	[Header("Effects")]
	[SerializeField]
	protected GameObject successfulHitEffect;

	[SerializeField]
	protected GameObject blockedHitEffect;

	[SerializeField]
	protected Transform effectSpawnTransform;

	protected Dat hpDamagePayload;

	protected Dat attackPower;

	protected Dat forcePayload;

	protected Dat poiseDamageDelta;

	protected Dat freezeTimeToAdd;

	protected Dat poisonPayload;

	protected Dat damageType;

	protected Dat powerLevel;

	protected Creature heldCreature;

	protected PlayerCharacter heldPlayerCharacter;

	protected bool potent { get; set; }

	protected void Awake()
	{
		hpDamagePayload = new Dat("dmg_hp", id);
		attackPower = new Dat("att_pow", id);
		forcePayload = new Dat("dmg_force", id);
		poiseDamageDelta = new Dat("pp_pow", id);
		freezeTimeToAdd = new Dat("freeze_time", id);
		poisonPayload = new Dat("poison", id);
		damageType = new Dat("dmg_type", id, typeof(HitType));
		powerLevel = new Dat("level", id);
		potent = true;
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
		if (!potent || (heldCreature != null && heldCreature.Frozen))
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
				Vector3 effectSpawnPosition = ((effectSpawnTransform != null) ? effectSpawnTransform.position : base.transform.position);
				Quaternion effectSpawnRotation = ((effectSpawnTransform != null) ? effectSpawnTransform.rotation : Quaternion.identity);
				float num = hpDamagePayload.Value;
				float num2 = forcePayload.Value;
				if ((bool)heldCreature)
				{
					num *= heldCreature.DamageFactor;
					num2 *= heldCreature.HitForceFactor;
				}
				Monster component = hitReceiver.GetComponent<Monster>();
				if (component != null)
				{
					component.Heal(num);
				}
				else
				{
					hitReceiver.ReceiveHit((HitType)damageType.EnumValue, Mathf.RoundToInt(num), attackPower.Value, poiseDamageDelta.IntValue, 0, freezeTimeToAdd.Value, poisonPayload.Value, true, false, base.transform.position, num2, effectSpawnPosition, effectSpawnRotation, false, null, null, powerLevel.IntValue);
				}
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
