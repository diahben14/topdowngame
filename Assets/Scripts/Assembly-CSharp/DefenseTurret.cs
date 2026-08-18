using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class DefenseTurret : Monster
{
	[Header("DefenseTurret")]
	[SerializeField]
	protected float rotationalTrackingSpeed = 450f;

	[SerializeField]
	protected float maxShootDiatance = 20f;

	[SerializeField]
	protected float maxXAngleDeflectionDegrees = 30f;

	[SerializeField]
	protected RaycastProjectile bulletPrefab;

	[SerializeField]
	protected Transform muzzleTransform;

	[SerializeField]
	protected Transform yAxisTransform;

	[SerializeField]
	protected Transform xAxisTransform;

	[SerializeField]
	protected float bulletVelocity;

	[SerializeField]
	protected Collider colliderForBulletsToIgnore;

	[SerializeField]
	protected bool leadByPlayerVelocity = true;

	[SerializeField]
	protected float deaggro_distance = 20f;

	[SerializeField]
	protected float deaggro_noLOSTimeout = 8f;

	[SerializeField]
	protected float stopShootNoLOSTimeout = 3f;

	[SerializeField]
	protected GameObject turretCorpsePrefab;

	[SerializeField]
	[EventRef]
	protected string sfx_rotationLoop;

	protected float turnToFaceVelocity;

	protected float lastCouldSeePlayerTime;

	protected EventInstance rotationLoopInstance;

	protected PARAMETER_ID rotationLoopAngleParamID;

	protected bool doFireSequence;

	protected Quaternion lastYAxisTransformRotation;

	protected static PooledFX bulletPool;

	protected override bool receiveBroadcastAggro
	{
		get
		{
			return false;
		}
	}

	public override bool Tetherable
	{
		get
		{
			return false;
		}
	}

	protected override bool removeZTargetOnDeath
	{
		get
		{
			return false;
		}
	}

	protected void __fire()
	{
		doFireSequence = true;
	}

	protected override bool doNavMeshSnappingAndReturnTrueIfGravityShouldBeApplied()
	{
		return false;
	}

	protected Vector3 getToTarget()
	{
		Vector3 vector = PlayerCharacter.WorldPosition + Vector3.up;
		Vector3 result = vector - muzzleTransform.position;
		if (!leadByPlayerVelocity)
		{
			return result;
		}
		Vector3 laterialVelocity = PlayerCharacter.LaterialVelocity;
		return vector + laterialVelocity * result.magnitude / bulletVelocity - muzzleTransform.position;
	}

	protected override void onAggro()
	{
		lastCouldSeePlayerTime = Time.time;
		rotationLoopInstance.start();
	}

	protected override void monster_Start()
	{
		rotationLoopInstance = SFX.GetInstance(sfx_rotationLoop, base.transform, cachedRigidbody);
		rotationLoopAngleParamID = SFX.GetParamID(rotationLoopInstance, "turret_tracking_angle");
		if (bulletPool == null)
		{
			bulletPool = PooledFX.NewPool("turret bullet pool", bulletPrefab.gameObject, 16, true);
		}
	}

	protected override void monster_OnDestroy()
	{
		rotationLoopInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		rotationLoopInstance.release();
	}

	protected override void BehaviourUpdate()
	{
		if (!base.Frozen && PlayerCharacter.Instanced && base.IsAggroed)
		{
			Vector3 toTarget = getToTarget();
			bool value = toTarget.magnitude < maxShootDiatance && Time.time < lastCouldSeePlayerTime + stopShootNoLOSTimeout;
			cachedAnimator.SetBool("shootOK", value);
			if (toTarget.magnitude > deaggro_distance || (Time.time > lastCouldSeePlayerTime + deaggro_noLOSTimeout && needLineOfSightToAggro))
			{
				base.monsterAggroed = false;
				rotationLoopInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			if (canSeePlayerOrBaitCheck())
			{
				lastCouldSeePlayerTime = Time.time;
			}
		}
	}

	protected void LateUpdate()
	{
		if (!base.Frozen && PlayerCharacter.Instanced && !base.SleepingDueToPlayerDistance)
		{
			Vector3 toTarget = getToTarget();
			float num = cachedAnimator.GetFloat("tracking") * rotationalTrackingSpeed;
			if (num > 0f)
			{
				yAxisTransform.rotation = Mathx.CalculateSmoothTurnToFaceQuaternion(lastYAxisTransformRotation.eulerAngles.y, toTarget, num, ref turnToFaceVelocity);
			}
			else
			{
				yAxisTransform.rotation = lastYAxisTransformRotation;
			}
			lastYAxisTransformRotation = yAxisTransform.rotation;
			float value = Mathf.RoundToInt(yAxisTransform.rotation.eulerAngles.y) % 360;
			rotationLoopInstance.setParameterByID(rotationLoopAngleParamID, value);
			float value2 = Mathx.xAngleBetween(Vector3.forward, toTarget);
			xAxisTransform.localEulerAngles = new Vector3(Mathf.Clamp(value2, 0f - maxXAngleDeflectionDegrees, maxXAngleDeflectionDegrees), 0f, 0f);
			if (doFireSequence)
			{
				fireSequence();
				doFireSequence = false;
			}
		}
	}

	protected void fireSequence()
	{
		if (PlayerCharacter.Instanced)
		{
			RaycastProjectile next = bulletPool.GetNext<RaycastProjectile>(muzzleTransform.position, muzzleTransform.rotation);
			Vector3 velocity = muzzleTransform.forward * bulletVelocity;
			Physics.IgnoreCollision(next.IgnoreCollider, colliderForBulletsToIgnore);
			next.Velocity = velocity;
			Debug.DrawRay(muzzleTransform.position, next.Velocity, Color.blue, 1f);
		}
	}

	protected override void alreadyDeadOnAwakeHandler()
	{
		spawnCorpse();
		Object.Destroy(base.gameObject);
	}

	protected override void monster_onDie()
	{
		spawnCorpse();
	}

	protected void spawnCorpse()
	{
		Object.Instantiate(turretCorpsePrefab, base.transform.position, Quaternion.identity);
	}
}
