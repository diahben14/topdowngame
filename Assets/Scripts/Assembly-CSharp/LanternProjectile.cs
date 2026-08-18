using UnityEngine;

public class LanternProjectile : MonoBehaviour
{
	[SerializeField]
	protected GameObject spawnOnImpact;

	[SerializeField]
	protected float yNormalThreshold = 0.5f;

	protected void OnCollisionEnter(Collision c)
	{
		Creature component = c.gameObject.GetComponent<Creature>();
		if (c.contacts[0].normal.y > yNormalThreshold || component != null)
		{
			Vector3 point = c.contacts[0].point;
			explode(point);
		}
	}

	protected void explode(Vector3 position)
	{
		Object.Instantiate(spawnOnImpact, position, Quaternion.identity);
		Object.Destroy(base.gameObject);
	}
}
