using UnityEngine;

public class Bumblebones : NavigatingMonster
{
	[Header("Bumblebones")]
	[SerializeField]
	protected bool startEmerged;

	[SerializeField]
	protected float attackDistance;

	[SerializeField]
	protected float minimumAttackFacingDotProduct = 0.5f;

	[SerializeField]
	protected float fastDistance = 20f;

	[SerializeField]
	protected float minTimeBetweenAttackTriggers = 1f;

	[SerializeField]
	protected bool isBigVersion;

	[SerializeField]
	protected GameObject deathDetritus;

	[SerializeField]
	protected float deathDetritusForceMagnitudeCap = 2f;

	[SerializeField]
	protected ParticleSystem impactParticleSystem;

	[SerializeField]
	private Rigidbody tossRigidbody;

	[SerializeField]
	private Transform tossOrigin;

	[SerializeField]
	private float tossForwardVelocity;

	[SerializeField]
	private float tossUpVelocity;

	[SerializeField]
	private float tossVelocityRandomness;

	[SerializeField]
	private int howManyToToss = 3;

	[SerializeField]
	private float closeAttackSetDistance = 5f;

	protected float lastAttackTime = float.NegativeInfinity;

	protected override bool SuppressPushbuffer
	{
		get
		{
			return cachedAnimator.GetFloat("AP_suppressPushbuffer") > 0f;
		}
	}

	public override int ModifyReceivedDamage(int dr)
	{
		if (cachedAnimator.GetFloat("AP_suppressPushbuffer") > 0f)
		{
			return Mathf.RoundToInt((float)dr / 2f);
		}
		return dr;
	}

	protected override void monster_Start()
	{
		if (startEmerged)
		{
			cachedAnimator.CrossFade("idle", 0f);
		}
	}

	protected override void LocomotingBehaviourUpdate()
	{
		cachedAnimator.SetBool("aggro", base.monsterAggroed);
		if (base.monsterAggroed)
		{
			Vector3 vector = base.playerOrBaitWorldPosition;
			float magnitude = (vector - base.transform.position).magnitude;
			Vector3 normalized = (vector - base.transform.position).normalized;
			float num = Vector3.Dot(base.transform.forward, normalized);
			base.locomotionTarget = vector;
			base.facingTarget = base.transform.position + nma.desiredVelocity;
			float num2 = Vector3.Distance(base.transform.position, vector);
			if (num2 < attackDistance && !base.busy && num > minimumAttackFacingDotProduct && Time.time > lastAttackTime + minTimeBetweenAttackTriggers)
			{
				if (isBigVersion)
				{
					if (num2 < closeAttackSetDistance)
					{
						Mathx.Choose(delegate
						{
							cachedAnimator.SetTrigger("attack");
							lastAttackTime = Time.time;
						}, delegate
						{
							cachedAnimator.SetTrigger("attack_alt");
							lastAttackTime = Time.time;
						});
					}
					else
					{
						Mathx.Choose(delegate
						{
							cachedAnimator.SetTrigger("attack");
							lastAttackTime = Time.time;
						}, delegate
						{
							cachedAnimator.SetTrigger("toss");
							lastAttackTime = Time.time;
						});
					}
				}
				else
				{
					cachedAnimator.SetTrigger("attack");
					lastAttackTime = Time.time;
				}
			}
			cachedAnimator.SetBool("fast", magnitude > fastDistance);
		}
		else
		{
			base.locomotionTarget = base.transform.position;
			base.facingTarget = base.transform.position + base.transform.forward;
		}
	}

	protected override void monster_onDie()
	{
		foreach (Transform item in Object.Instantiate(deathDetritus, base.transform.position, base.transform.rotation).transform)
		{
			Rigidbody component = item.gameObject.GetComponent<Rigidbody>();
			if ((bool)component)
			{
				Vector3 vector = lastPhysicsForce;
				float magnitude = vector.magnitude;
				magnitude = Mathf.Min(magnitude, deathDetritusForceMagnitudeCap);
				component.AddForce(vector.normalized * magnitude, ForceMode.Impulse);
			}
		}
	}

	public void __impactFX()
	{
		if (impactParticleSystem != null)
		{
			impactParticleSystem.transform.rotation = Quaternion.identity;
			impactParticleSystem.Play();
		}
	}

	public void __toss()
	{
		for (int i = 0; i < howManyToToss; i++)
		{
			Rigidbody rigidbody = Object.Instantiate(tossRigidbody, tossOrigin.position, Random.rotation);
			Vector3 force = base.transform.forward * tossForwardVelocity + base.transform.up * tossUpVelocity;
			force += Random.insideUnitSphere * tossVelocityRandomness;
			rigidbody.AddForce(force, ForceMode.VelocityChange);
		}
	}
}
