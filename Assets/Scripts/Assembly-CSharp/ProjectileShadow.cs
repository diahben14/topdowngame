using UnityEngine;

[ExecuteInEditMode]
public class ProjectileShadow : MonoBehaviour
{
	[SerializeField]
	protected LayerMask layerMask;

	[SerializeField]
	protected LineRenderer lineRenderer;

	[SerializeField]
	protected GameObject groundShadowObject;

	[SerializeField]
	protected const float maxDistance = 32f;

	[SerializeField]
	protected float shadowGroundOffset = 0.01f;

	private void Update()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(base.transform.position, Vector3.down, out hitInfo, 32f, layerMask))
		{
			if ((bool)lineRenderer)
			{
				lineRenderer.enabled = true;
				lineRenderer.SetPosition(0, base.transform.position);
				lineRenderer.SetPosition(1, hitInfo.point);
			}
			if ((bool)groundShadowObject)
			{
				groundShadowObject.SetActive(true);
				groundShadowObject.transform.position = hitInfo.point + hitInfo.normal * shadowGroundOffset;
				groundShadowObject.transform.rotation = Quaternion.LookRotation(-hitInfo.normal);
			}
		}
		else
		{
			if ((bool)lineRenderer)
			{
				lineRenderer.enabled = false;
			}
			if ((bool)groundShadowObject)
			{
				groundShadowObject.SetActive(false);
			}
		}
	}
}
