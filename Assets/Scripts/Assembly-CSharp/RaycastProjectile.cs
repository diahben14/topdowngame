using System.Collections;
using FMODUnity;
using UnityEngine;

public class RaycastProjectile : HitTrigger
{
	[Header("---------- Projectile ----------")]
	[SerializeField]
	protected float trackingDegreesPerSecond;

	[SerializeField]
	protected float totalTrackingBudget;

	[SerializeField]
	protected bool leadTrackingByVelocity;

	[SerializeField]
	public Collider IgnoreCollider;

	[SerializeField]
	protected bool m_doHitsOnTriggerStay;

	[Header(" -- Projectile cheating")]
	[SerializeField]
	protected bool cheatWallCollisions;

	[SerializeField]
	protected LayerMask cheatLayermask;

	[Header(" -- Projectile impact behaviour")]
	[SerializeField]
	protected bool despawnOnCollideWithHitReceiver = true;

	[SerializeField]
	protected bool despawnOnNonHRCollision = true;

	[SerializeField]
	protected float delayBeforeDisable = 1f;

	[SerializeField]
	protected bool deflectWhenBlocked;

	[SerializeField]
	[EventRef]
	protected string deflectionSFX;

	[SerializeField]
	protected float deflectionSphereRadius = 0.1f;

	[SerializeField]
	protected bool doBlockedEffectsOnNonHitReceiverCollision;

	[SerializeField]
	protected PooledFX impactPooledFX;

	[Header(" -- Projectile Effects")]
	[SerializeField]
	protected GameObject projectileGraphicsRoot;

	[SerializeField]
	protected GameObject trailGraphicsRoot;

	[SerializeField]
	protected GameObject impactGraphicsRoot;

	[SerializeField]
	protected float kMaxProjectileLifetime = 4f;

	protected Rigidbody cachedRigidbody;

	protected float birthTime;

	protected float _damageFactor = 1f;

	protected Vector3 previousPosition;

	protected LayerMask layermaskFromPhysicsLayer;

	protected Vector3 lastPosition;

	public Vector3 Velocity
	{
		get
		{
			return cachedRigidbody.velocity;
		}
		set
		{
			cachedRigidbody.velocity = value;
		}
	}

	public bool DisableTracking { get; set; }

	public float DamageFactor
	{
		get
		{
			return _damageFactor;
		}
		set
		{
			_damageFactor = value;
		}
	}

	protected override Vector3 OriginOfForce
	{
		get
		{
			return previousPosition - Velocity;
		}
	}

	protected override bool doHitsOnTriggerStay
	{
		get
		{
			return m_doHitsOnTriggerStay;
		}
	}

	protected override void hitTrigger_Awake()
	{
		powerLevel_dat = new Dat("level", id);
		cachedRigidbody = GetComponent<Rigidbody>();
		int layer = base.gameObject.layer;
		for (int i = 0; i < 32; i++)
		{
			if (!Physics.GetIgnoreLayerCollision(layer, i))
			{
				layermaskFromPhysicsLayer = (int)layermaskFromPhysicsLayer | (1 << i);
			}
		}
	}

	protected void OnEnable()
	{
		projectileGraphicsRoot.SetActive(true);
		trailGraphicsRoot.SetActive(true);
		impactGraphicsRoot.SetActive(false);
		IgnoreCollider.enabled = true;
		birthTime = Time.time;
		previousPosition = base.transform.position;
		base.Potent = true;
	}

	protected void FixedUpdate()
	{
		if (Time.time > birthTime + kMaxProjectileLifetime)
		{
			runDisableSequence();
			return;
		}
		Vector3 vector = previousPosition;
		Vector3 direction = base.transform.position - vector;
		RaycastHit hitInfo;
		if (Physics.Raycast(vector, direction, out hitInfo, direction.magnitude, layermaskFromPhysicsLayer, QueryTriggerInteraction.Ignore))
		{
			Debug.DrawLine(vector, vector + Vector3.up, Color.white, 1f);
			Debug.DrawLine(vector, base.transform.position, Color.red, 1f);
			base.transform.position = hitInfo.point;
			doHit(hitInfo.collider);
		}
		else
		{
			Debug.DrawLine(vector, vector + Vector3.up, Color.white, 1f);
			Debug.DrawLine(vector, base.transform.position, Color.green, 1f);
		}
		if (base.Potent && trackingDegreesPerSecond != 0f && PlayerCharacter.Instanced && totalTrackingBudget > 0f && !DisableTracking)
		{
			Vector3 vector2 = PlayerCharacter.WorldPosition + Vector3.up;
			Vector3 vector3 = vector2 - base.transform.position;
			Vector3 vector4 = vector2;
			if (leadTrackingByVelocity && Velocity != Vector3.zero)
			{
				vector4 += PlayerCharacter.LaterialVelocity * vector3.magnitude / Velocity.magnitude;
			}
			Vector3 vector5 = vector4 - base.transform.position;
			Quaternion b = Quaternion.FromToRotation(Velocity, vector5);
			float num = Vector3.Angle(Velocity, vector5);
			float num2 = Mathf.Min(1f, trackingDegreesPerSecond * Time.deltaTime / num);
			Quaternion quaternion = Quaternion.Lerp(Quaternion.identity, b, num2);
			totalTrackingBudget -= num2 * num;
			if (cachedRigidbody.velocity.magnitude > 0f)
			{
				cachedRigidbody.velocity = quaternion * cachedRigidbody.velocity;
				base.transform.rotation = Quaternion.LookRotation(cachedRigidbody.velocity);
				cachedRigidbody.angularVelocity = Vector3.zero;
			}
		}
		previousPosition = base.transform.position;
		if (Velocity.magnitude > 0f)
		{
			base.transform.rotation = Quaternion.LookRotation(Velocity);
		}
	}

	protected void runDisableSequence()
	{
		StartCoroutine(disableSequence());
	}

	protected IEnumerator disableSequence()
	{
		IgnoreCollider.enabled = false;
		Velocity = Vector3.zero;
		if (projectileGraphicsRoot != null)
		{
			projectileGraphicsRoot.SetActive(false);
		}
		yield return new WaitForSeconds(delayBeforeDisable);
		base.gameObject.SetActive(false);
	}

	protected void runImpactFXSequence(Vector3 pos)
	{
		if (impactGraphicsRoot != null)
		{
			impactGraphicsRoot.transform.position = pos;
			impactGraphicsRoot.SetActive(true);
		}
		if (impactPooledFX != null)
		{
			impactPooledFX.GetNext(pos);
		}
	}

	protected override void HitWasSuccess(Vector3 impactPosition)
	{
		Vector3 effectSpawnPoint = getEffectSpawnPoint();
		runImpactFXSequence(effectSpawnPoint);
		if (despawnOnCollideWithHitReceiver)
		{
			runDisableSequence();
			base.Potent = false;
		}
	}

	protected override void HitWasBlocked(Vector3 impactPosition, bool parry)
	{
		if (deflectWhenBlocked)
		{
			base.transform.position = previousPosition - Velocity * Time.deltaTime;
			Velocity *= -1f;
			if (!parry)
			{
				float magnitude = Velocity.magnitude;
				Velocity += Random.insideUnitSphere * Velocity.magnitude * deflectionSphereRadius;
				Velocity = Vector3.Scale(Velocity.normalized, new Vector3(1f, 0f, 1f)) * magnitude;
				base.transform.rotation = Quaternion.LookRotation(Velocity);
			}
			SFX.PlayAudioClip(deflectionSFX);
		}
		else
		{
			Vector3 effectSpawnPoint = getEffectSpawnPoint();
			runImpactFXSequence(effectSpawnPoint);
			if (despawnOnCollideWithHitReceiver)
			{
				runDisableSequence();
				base.Potent = false;
			}
		}
	}

	protected override void triggerEnterWithNotHitReceiver(Collider c)
	{
		if (base.Potent && despawnOnNonHRCollision && (!cheatWallCollisions || !ableToCheatPastCollision()))
		{
			if (doBlockedEffectsOnNonHitReceiverCollision)
			{
				Vector3 effectSpawnPoint = getEffectSpawnPoint();
				runImpactFXSequence(effectSpawnPoint);
			}
			Velocity = Vector3.zero;
			Collider[] components = GetComponents<Collider>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].enabled = false;
			}
			runDisableSequence();
			base.Potent = false;
		}
	}

	protected Vector3 getEffectSpawnPoint()
	{
		Vector3 vector = base.transform.position - lastPosition;
		RaycastHit hitInfo;
		if (Physics.Raycast(lastPosition - vector, cachedRigidbody.velocity, out hitInfo, vector.magnitude * 2f, -1, QueryTriggerInteraction.Ignore))
		{
			return hitInfo.point;
		}
		if (!effectSpawnTransform)
		{
			return base.transform.position;
		}
		return effectSpawnTransform.position;
	}

	protected bool ableToCheatPastCollision()
	{
		Vector3 vector = -CameraController.MainCameraForward;
		Vector3 origin = base.transform.position - Velocity.normalized * Time.deltaTime + vector;
		Ray ray = new Ray(origin, Velocity.normalized);
		float num = Velocity.magnitude * Time.deltaTime * 2f;
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(ray, out hitInfo, num, cheatLayermask);
		Debug.DrawLine(ray.origin, ray.origin + ray.direction * num, flag ? Color.red : Color.green, 4f, true);
		return !flag;
	}

	private void LateUpdate()
	{
		lastPosition = base.transform.position;
	}
}
