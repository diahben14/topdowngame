using System.Collections;
using FMODUnity;
using UnityEngine;

public class Hedgehog : NavigatingMonster
{
	[Header("Hedgehog")]
	[SerializeField]
	protected float maxWanderDistance = 6f;

	[SerializeField]
	protected float minTimeBetweenWanders = 6f;

	[SerializeField]
	protected float maxTimeBetweenWanders = 12f;

	[SerializeField]
	protected float aggroDistance = 12f;

	[SerializeField]
	protected float maxAltitudeDeltaForShoot = 2f;

	[SerializeField]
	protected Rigidbody bulletPrefab;

	[SerializeField]
	protected float bulletVelocity = 30f;

	[SerializeField]
	protected float meleeAttackCooldown = 1f;

	[SerializeField]
	protected float missileAttackCooldown = 3f;

	[SerializeField]
	protected int spinePairsPerVolley = 4;

	[SerializeField]
	protected float spineSpread = 0.1f;

	[SerializeField]
	protected float meleeDistance = 4f;

	[SerializeField]
	protected bool guardian;

	[SerializeField]
	protected bool keepDistanceByAdam;

	protected Vector3 homePosition;

	protected float nextWanderTime;

	protected float lastAttackTime;

	protected float lastSawPlayerTime;

	protected bool goingHomeBecauseNoPath;

	[SerializeField]
	[EventRef]
	protected string shootSFX;

	protected static PooledFX bulletPool;

	protected override void monster_Start()
	{
		homePosition = base.transform.position;
		if (bulletPool == null)
		{
			bulletPool = PooledFX.NewPool("hedgehog bullet pool", bulletPrefab.gameObject, 32, true);
		}
	}

	protected override void LocomotingBehaviourUpdate()
	{
		if (PlayerCharacter.Instanced)
		{
			Vector3 vector = PlayerCharacter.WorldPosition - base.transform.position;
			float num = Mathf.Abs(vector.y);
			float magnitude = vector.magnitude;
			float num2 = Vector3.Dot(vector.normalized, base.transform.forward);
			bool flag = canSeePlayerOrBaitCheck();
			if (base.monsterAggroed)
			{
				if (base.busy)
				{
					return;
				}
				if (flag || (!guardian && base.hasPath && !goingHomeBecauseNoPath))
				{
					lastSawPlayerTime = Time.time;
					if (magnitude < meleeDistance)
					{
						if (num2 > 0.8f && Time.time > lastAttackTime + meleeAttackCooldown)
						{
							lastAttackTime = Time.time;
							cachedAnimator.SetTrigger("melee");
						}
						else
						{
							base.locomotionTarget = base.transform.position;
							base.facingTarget = base.playerOrBaitWorldPosition;
						}
					}
					else if (keepDistanceByAdam)
					{
						if (magnitude < 16f)
						{
							Vector3 vector2 = base.transform.position - vector.normalized * 8f;
							base.locomotionTarget = vector2;
							base.facingTarget = base.playerOrBaitWorldPosition;
						}
						else if (Time.time > lastAttackTime + missileAttackCooldown && num2 > 0.75f && num < maxAltitudeDeltaForShoot)
						{
							lastAttackTime = Time.time;
							cachedAnimator.SetTrigger("shoot spine");
						}
						else
						{
							base.locomotionTarget = base.playerOrBaitWorldPosition;
							base.facingTarget = base.playerOrBaitWorldPosition;
						}
					}
					else
					{
						base.locomotionTarget = base.playerOrBaitWorldPosition;
						base.facingTarget = base.playerOrBaitWorldPosition;
						if (Time.time > lastAttackTime + missileAttackCooldown && num2 > 0.95f && num < maxAltitudeDeltaForShoot)
						{
							lastAttackTime = Time.time;
							cachedAnimator.SetTrigger("shoot spine");
						}
					}
					return;
				}
				if (!base.hasPath)
				{
					goingHomeBecauseNoPath = true;
				}
				if (Time.time > lastSawPlayerTime + 4f)
				{
					float num3 = Vector3.Distance(base.transform.position, homePosition);
					base.locomotionTarget = homePosition;
					if (num3 > 2f)
					{
						base.facingTarget = homePosition;
					}
					else
					{
						base.facingTarget = base.transform.position + base.transform.forward;
					}
				}
				else
				{
					base.locomotionTarget = base.transform.position;
					base.facingTarget = base.transform.position + base.transform.forward;
				}
				return;
			}
		}
		if (Time.time > nextWanderTime)
		{
			Vector3 vector3 = Random.onUnitSphere * maxWanderDistance;
			vector3.y = 0f;
			base.locomotionTarget = homePosition + vector3;
			if (vector3.magnitude > 0f)
			{
				base.facingTarget = homePosition + vector3;
			}
			else
			{
				base.facingTarget = homePosition + base.transform.forward;
			}
			nextWanderTime = Time.time + Random.Range(minTimeBetweenWanders, maxTimeBetweenWanders);
		}
	}

	public IEnumerator ShootSpine()
	{
		for (int i = 0; i < spinePairsPerVolley; i++)
		{
			float num = bulletVelocity * (1f - spineSpread * (float)(i - 1));
			SFX.PlayAudioClipHere(shootSFX, base.transform.position);
			for (int j = 0; j < ((i == 0) ? 1 : 2); j++)
			{
				Quaternion rotation = base.transform.rotation * Quaternion.Euler(0f, i * 5 * ((j == 0) ? 1 : (-1)), 0f);
				Rigidbody next = bulletPool.GetNext<Rigidbody>(base.transform.position + Vector3.up, rotation);
				next.velocity = next.transform.forward * num;
				Physics.IgnoreCollision(GetComponent<Collider>(), next.GetComponent<Collider>());
			}
			yield return new WaitForSeconds(0.033f);
		}
	}
}
