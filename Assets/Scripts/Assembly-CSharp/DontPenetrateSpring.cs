using UnityEngine;

public class DontPenetrateSpring : MonoBehaviour
{
	[SerializeField]
	protected Transform extendPoint;

	[SerializeField]
	protected Transform retreatPoint;

	[SerializeField]
	protected LayerMask raycastLayerMask;

	[SerializeField]
	protected float wallBufferDistance = 0.2f;

	protected Vector3 originalLocalPosition;

	private void Awake()
	{
		if (extendPoint.parent != retreatPoint.parent)
		{
			Debug.LogError("DontPenetrateString retreat point and extend point need same parent.");
		}
	}

	private void Update()
	{
		Vector3 vector = extendPoint.position - retreatPoint.position;
		Debug.DrawRay(retreatPoint.position, vector);
		RaycastHit hitInfo;
		if (Physics.Raycast(retreatPoint.position, vector, out hitInfo, vector.magnitude + wallBufferDistance, raycastLayerMask))
		{
			base.transform.position = retreatPoint.position + vector.normalized * (hitInfo.distance - wallBufferDistance);
		}
		else
		{
			base.transform.position = extendPoint.position;
		}
	}
}
