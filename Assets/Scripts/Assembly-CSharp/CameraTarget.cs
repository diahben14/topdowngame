using UnityEngine;

public class CameraTarget : MonoBehaviour
{
	protected static CameraTarget instance;

	public static Vector3? Position
	{
		get
		{
			if (instance == null)
			{
				return null;
			}
			return instance.transform.position;
		}
	}

	private void Start()
	{
		instance = this;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(base.transform.position, base.transform.position + Vector3.up * 2f);
		Gizmos.DrawSphere(base.transform.position, 0.25f);
	}
}
