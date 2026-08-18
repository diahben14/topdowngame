using UnityEngine;

public class LegImpactDetector : MonoBehaviour
{
	[SerializeField]
	protected float maxDistance;

	[SerializeField]
	protected LayerMask layerMask;

	[SerializeField]
	protected ParticleSystem impactParticleSystem;

	protected bool wasGrounded = true;

	protected float kDragDistanceThreshold = 2f;

	protected Vector3 lastGroundingPosition;

	private void Update()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(base.transform.position, base.transform.forward, out hitInfo, maxDistance, layerMask))
		{
			if (!wasGrounded || Vector3.Distance(lastGroundingPosition, hitInfo.point) > kDragDistanceThreshold)
			{
				wasGrounded = true;
				lastGroundingPosition = hitInfo.point;
				doImpact(hitInfo.point);
			}
		}
		else
		{
			wasGrounded = false;
		}
	}

	protected void doImpact(Vector3 impactPoint)
	{
		if (!(Time.timeSinceLevelLoad < 1f))
		{
			CameraController.AddScreenshake(0.1f);
			impactParticleSystem.transform.position = impactPoint;
			impactParticleSystem.transform.rotation = Quaternion.identity;
			impactParticleSystem.Play();
		}
	}

	protected void OnDrawGizmos()
	{
		Gizmos.color = (wasGrounded ? Color.yellow : Color.cyan);
		Gizmos.DrawCube(base.transform.position, Vector3.one);
		Gizmos.DrawRay(base.transform.position, base.transform.forward * maxDistance);
		Gizmos.DrawWireSphere(base.transform.position + base.transform.forward * maxDistance, 1f);
	}
}
