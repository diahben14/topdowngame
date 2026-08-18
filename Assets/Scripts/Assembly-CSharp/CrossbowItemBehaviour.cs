using System.Collections;
using UnityEngine;

public class CrossbowItemBehaviour : SpecificItemBehaviour
{
	[SerializeField]
	private Item bowItem;

	[SerializeField]
	private RaycastProjectile arrowPrefab;

	[SerializeField]
	private Rigidbody thrownArrowPrefab;

	[SerializeField]
	private Transform spawnTransform;

	[SerializeField]
	private Collider projectileIgnoreCollider;

	[SerializeField]
	private float fireForceMagnitude = 100f;

	[SerializeField]
	private float sweetspotForce = 100f;

	[SerializeField]
	private float sweetspotDamageFactor = 1.5f;

	[SerializeField]
	private float throwForceMagnitude = 10f;

	[SerializeField]
	private AudioClip aimSFX;

	[SerializeField]
	private AudioClip fireSFX;

	[SerializeField]
	protected float minimumChargeDuration = 1.1f;

	[SerializeField]
	protected float chargeSweetspotWidth = 0.2f;

	[SerializeField]
	protected LineRenderer laserSightLineRenderer;

	[SerializeField]
	protected float maxLaserSightDistance = 20f;

	[ColorUsage(true, true)]
	[SerializeField]
	protected Color sweetspotLaserColour;

	[SerializeField]
	protected Color normalLaserColour;

	protected bool inSweetSpot;

	public override bool ShouldUsageMakeItemRootVisible
	{
		get
		{
			return bowItem.Quantity > 0;
		}
	}

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		if (item.Quantity <= 0)
		{
			yield break;
		}
		if (bowItem.Quantity > 0)
		{
			cachedAnimator.SetTrigger("ready crossbow");
			if ((bool)aimSFX)
			{
				AudioSource.PlayClipAtPoint(aimSFX, base.transform.position);
			}
			float timer = 0f;
			bool actionButtonReleased = false;
			bool actionButtonReleasedEarly = false;
			while (!wasCancelled && (timer < minimumChargeDuration || !actionButtonReleased))
			{
				if (animationWasInterrupted)
				{
					laserSightLineRenderer.enabled = false;
					yield break;
				}
				timer += Time.deltaTime;
				actionButtonReleased |= PlayerCharacter.itemBehaviourInputUps[actionButtonIndex];
				Vector3 to = PlayerCharacter.ZTargetWorldPositionOrNull ?? (spawnTransform.position + base.transform.forward);
				float x = Mathx.xAngleBetween(spawnTransform.position, to);
				spawnTransform.localRotation = Quaternion.Euler(x, 0f, 0f);
				if (timer >= minimumChargeDuration)
				{
					RaycastHit hitInfo;
					Vector3 vector = ((!Physics.Raycast(spawnTransform.position, spawnTransform.forward, out hitInfo, maxLaserSightDistance)) ? (spawnTransform.position + spawnTransform.forward * maxLaserSightDistance) : hitInfo.point);
					laserSightLineRenderer.enabled = true;
					laserSightLineRenderer.SetPositions(new Vector3[2] { spawnTransform.position, vector });
					float t = Mathf.InverseLerp(minimumChargeDuration, minimumChargeDuration + chargeSweetspotWidth, timer);
					Color color = Color.Lerp(sweetspotLaserColour, normalLaserColour, t);
					Color startColor = Color.Lerp(color, Color.black, hitInfo.distance / maxLaserSightDistance);
					laserSightLineRenderer.startColor = color;
					laserSightLineRenderer.startColor = startColor;
				}
				else
				{
					if (actionButtonReleased)
					{
						actionButtonReleasedEarly = true;
					}
					laserSightLineRenderer.enabled = false;
				}
				yield return new WaitForFixedUpdate();
			}
			if (!wasCancelled)
			{
				laserSightLineRenderer.enabled = false;
				inSweetSpot = timer < minimumChargeDuration + chargeSweetspotWidth && !actionButtonReleasedEarly;
				cachedAnimator.SetTrigger("fire crossbow");
			}
			else
			{
				laserSightLineRenderer.enabled = false;
				cachedAnimator.SetTrigger("abort item");
			}
		}
		else
		{
			item.Quantity--;
			Rigidbody rigidbody = Object.Instantiate(thrownArrowPrefab, spawnTransform.position, spawnTransform.rotation);
			Vector3 force = base.transform.forward.normalized * throwForceMagnitude;
			rigidbody.AddForce(force, ForceMode.Impulse);
			Object.Destroy(rigidbody.gameObject, 3f);
		}
	}

	protected void __fireBow()
	{
		item.Quantity--;
		if ((bool)fireSFX)
		{
			AudioSource.PlayClipAtPoint(fireSFX, base.transform.position);
		}
		RaycastProjectile raycastProjectile = Object.Instantiate(arrowPrefab, spawnTransform.position, spawnTransform.rotation);
		Physics.IgnoreCollision(raycastProjectile.IgnoreCollider, projectileIgnoreCollider);
		raycastProjectile.Velocity = spawnTransform.forward.normalized * (inSweetSpot ? sweetspotForce : fireForceMagnitude);
		if (inSweetSpot)
		{
			raycastProjectile.DamageFactor = sweetspotDamageFactor;
		}
	}
}
