using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpawnOnImpactAndDestroy : MonoBehaviour
{
	[SerializeField]
	protected GameObject spawn;

	[SerializeField]
	protected bool adoptRotation;

	[SerializeField]
	protected float relativeVelocityThreshold;

	protected void OnCollisionEnter(Collision c)
	{
		if (c.relativeVelocity.magnitude > relativeVelocityThreshold)
		{
			breakOpen();
		}
	}

	protected void breakOpen()
	{
		Object.Instantiate(spawn, base.transform.position, adoptRotation ? base.transform.rotation : Quaternion.identity);
		Object.Destroy(base.gameObject);
	}
}
