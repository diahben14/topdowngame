using System.Collections;
using UnityEngine;

public class TurretTrap : MonoBehaviour
{
	[SerializeField]
	protected Transform muzzleTransform;

	[SerializeField]
	protected float maxDetectionRange;

	[SerializeField]
	protected float maxDetectionDeflectionRadius = 0.15f;

	[SerializeField]
	protected LayerMask layerMask;

	[SerializeField]
	protected int bulletsPerSalvo;

	[SerializeField]
	protected float delayBetweenBullets = 0.2f;

	[SerializeField]
	protected float maxDeflectionRadius = 0.15f;

	[SerializeField]
	protected ParticleSystem muzzleFlashPS;

	[SerializeField]
	protected RaycastProjectile bulletPrefab;

	[SerializeField]
	protected float bulletVelocity;

	[SerializeField]
	protected float kickbackForce;

	[SerializeField]
	protected float cooldownDuration;

	[SerializeField]
	protected Vector3 kickbackForceOriginOffset;

	[SerializeField]
	protected Collider colliderForBulletsToIgnore;

	protected Rigidbody cachedRigidbody;

	protected float lastSalvoTime;

	protected void Awake()
	{
		cachedRigidbody = GetComponent<Rigidbody>();
		lastSalvoTime = float.NegativeInfinity;
	}

	protected void Update()
	{
		if (Time.time > lastSalvoTime + cooldownDuration)
		{
			Vector3 direction = muzzleTransform.forward + (Vector3)(Random.insideUnitCircle * maxDetectionDeflectionRadius);
			RaycastHit hitInfo;
			if (Physics.Raycast(muzzleTransform.position, direction, out hitInfo, maxDetectionRange, layerMask) && hitInfo.collider.gameObject.GetComponent<Creature>() != null)
			{
				shoot();
			}
		}
	}

	protected void shoot()
	{
		StartCoroutine(shootCoroutine());
	}

	protected IEnumerator shootCoroutine()
	{
		lastSalvoTime = Time.time;
		for (int i = 0; i < bulletsPerSalvo; i++)
		{
			muzzleFlashPS.Play();
			RaycastProjectile raycastProjectile = Object.Instantiate(bulletPrefab, muzzleTransform.position, muzzleTransform.rotation);
			Vector3 vector = (Vector3)Random.insideUnitCircle * maxDeflectionRadius;
			Vector3 velocity = (raycastProjectile.transform.forward + vector) * bulletVelocity;
			raycastProjectile.Velocity = velocity;
			Physics.IgnoreCollision(raycastProjectile.IgnoreCollider, colliderForBulletsToIgnore);
			cachedRigidbody.AddForceAtPosition(-base.transform.forward * kickbackForce, base.transform.TransformPoint(muzzleTransform.localPosition + kickbackForceOriginOffset), ForceMode.Impulse);
			yield return new WaitForSeconds(delayBetweenBullets);
		}
	}

	protected void OnDrawGizmos()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(muzzleTransform.position, muzzleTransform.forward, out hitInfo, maxDetectionRange, layerMask))
		{
			Debug.DrawLine(muzzleTransform.position, hitInfo.point);
		}
		else
		{
			Debug.DrawLine(muzzleTransform.position, muzzleTransform.position + muzzleTransform.forward * maxDetectionRange);
		}
	}
}
