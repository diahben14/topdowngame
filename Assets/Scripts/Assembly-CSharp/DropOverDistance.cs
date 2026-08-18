using UnityEngine;

public class DropOverDistance : MonoBehaviour
{
	[SerializeField]
	protected GameObject objectToDrop;

	[SerializeField]
	protected float distanceBetweenDrops = 2f;

	[SerializeField]
	protected bool raycastToGround;

	[SerializeField]
	protected LayerMask raycastLayerMask;

	[SerializeField]
	protected float raycastMaxDist = 8f;

	protected Vector3 lastDropPoint;

	protected void Awake()
	{
		lastDropPoint = base.transform.position;
	}

	protected void FixedUpdate()
	{
		if ((base.transform.position - lastDropPoint).magnitude >= distanceBetweenDrops)
		{
			doDrop();
		}
	}

	protected void doDrop()
	{
		bool flag = true;
		Vector3 position = base.transform.position;
		if (raycastToGround)
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(base.transform.position, Vector3.down, out hitInfo, raycastMaxDist, raycastLayerMask, QueryTriggerInteraction.Ignore))
			{
				position = hitInfo.point;
			}
			else
			{
				flag = false;
			}
		}
		if (flag)
		{
			Object.Instantiate(objectToDrop, position, Quaternion.identity);
		}
		lastDropPoint = base.transform.position;
	}
}
