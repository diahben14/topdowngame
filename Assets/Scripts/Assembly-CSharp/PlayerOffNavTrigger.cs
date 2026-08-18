using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerOffNavTrigger : PlayerTriggerZone<PlayerOffNavTrigger>
{
	protected BoxCollider _bc;

	protected BoxCollider bc
	{
		get
		{
			if (_bc == null)
			{
				_bc = GetComponent<BoxCollider>();
			}
			return _bc;
		}
	}

	protected void OnDrawGizmos()
	{
		Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, base.transform.lossyScale);
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(bc.center, bc.size);
		Gizmos.DrawWireCube(bc.center, bc.size);
	}
}
