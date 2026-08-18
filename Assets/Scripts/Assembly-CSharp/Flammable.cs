using System;
using UnityEngine;

public class Flammable : MonoBehaviour
{
	[SerializeField]
	protected bool flammable = true;

	[SerializeField]
	protected bool permanentlyOnFire;

	protected static readonly float kFireDamagePerSecond = 1f;

	protected static readonly string kPathToCreatureFireParticleSystem = "CreatureFire";

	protected static readonly float kStopDropRollDistancePerSecondThreshold = 10f;

	protected static readonly float kStopDropRollFireReductionFactor = 1.5f;

	protected float fireRadius;

	private ParticleSystem _fireParticleSystem;

	private ParticleSystem.EmissionModule _fireParticleSystemEmission;

	private float _fireAmount;

	protected float onFireCounter;

	protected HitReceiver cachedHitReceiver;

	protected Creature cachedCreature;

	protected Rigidbody cachedRigidbody;

	protected bool hitReceiverIsCached;

	protected bool creatureIsCached;

	protected bool rigidbodyIsCached;

	protected bool fireParticleSystemEmission
	{
		set
		{
			if (!(_fireParticleSystem == null) || value)
			{
				if (_fireParticleSystem == null)
				{
					GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(kPathToCreatureFireParticleSystem, typeof(GameObject)));
					gameObject.transform.parent = base.transform;
					gameObject.transform.localPosition = Vector3.up;
					_fireParticleSystem = gameObject.GetComponent<ParticleSystem>();
					Bounds bounds = GetComponent<Collider>().bounds;
					float num = (bounds.size.x + bounds.size.y + bounds.size.z) / 3f;
					ParticleSystem.ShapeModule shape = _fireParticleSystem.shape;
					fireRadius = num / 2f;
					shape.radius = fireRadius;
					float num2 = (float)Math.PI * fireRadius * fireRadius;
					ParticleSystem.MinMaxCurve rateOverTime = _fireParticleSystem.emission.rateOverTime;
					float constantMax = rateOverTime.constantMax;
					float constantMin = (rateOverTime.constantMin = num2 * constantMax);
					rateOverTime.constantMin = constantMin;
					shape.radius = num / 2f;
					_fireParticleSystemEmission = _fireParticleSystem.emission;
				}
				_fireParticleSystemEmission.enabled = value;
			}
		}
	}

	public float FireAmount
	{
		get
		{
			return _fireAmount;
		}
		set
		{
			_fireAmount = Mathf.Clamp(value, 0f, 2f);
		}
	}

	public bool OnFire
	{
		get
		{
			if (!flammable || !(FireAmount > 1f))
			{
				return permanentlyOnFire;
			}
			return true;
		}
	}

	protected void OnTriggerStay(Collider c)
	{
		if (OnFire)
		{
			shareFire(c.gameObject);
		}
	}

	protected void shareFire(GameObject other)
	{
		Flammable component = other.GetComponent<Flammable>();
		if (component != null)
		{
			component.FireAmount += Time.deltaTime;
		}
	}

	private void Awake()
	{
		cachedHitReceiver = GetComponent<HitReceiver>();
		cachedCreature = GetComponent<Creature>();
		cachedRigidbody = GetComponent<Rigidbody>();
		hitReceiverIsCached = cachedHitReceiver != null;
		creatureIsCached = cachedCreature != null;
		rigidbodyIsCached = cachedRigidbody != null;
	}

	private void Update()
	{
		if (creatureIsCached && cachedCreature.InWater)
		{
			FireAmount = 0f;
		}
		fireParticleSystemEmission = OnFire;
		if (OnFire)
		{
			onFireCounter += Time.deltaTime;
			if (hitReceiverIsCached && onFireCounter > 1f / kFireDamagePerSecond)
			{
				cachedHitReceiver.ReceiveHit(HitType.NORMAL, 2, -1f, 0, 0, 0f, 0f, true, true, base.transform.position, 0f, base.transform.position, Quaternion.identity, false, null, null, -1);
				onFireCounter = 0f;
				RaycastHit[] array = Physics.SphereCastAll(base.transform.position, fireRadius, Vector3.up, 0.1f, -1);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].collider.GetComponent<Flammable>() != this)
					{
						shareFire(array[i].collider.gameObject);
					}
				}
			}
		}
		if (rigidbodyIsCached)
		{
			bool flag = cachedRigidbody.velocity.magnitude > kStopDropRollDistancePerSecondThreshold;
			if (Time.deltaTime > 0f && flag)
			{
				FireAmount -= Time.deltaTime * kStopDropRollFireReductionFactor;
			}
		}
	}
}
