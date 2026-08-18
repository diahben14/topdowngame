using System.Collections;
using UnityEngine;

public class PoisonZone : MonoBehaviour
{
	[SerializeField]
	protected int damagePoints;

	[SerializeField]
	protected int powerLevel = -1;

	[SerializeField]
	protected ParticleSystem cachedParticleSystem;

	[SerializeField]
	protected float destroyAfterSeconds = float.PositiveInfinity;

	protected void OnTriggerStay(Collider c)
	{
		HitReceiver component = c.GetComponent<HitReceiver>();
		if (component != null)
		{
			component.ReceiveHit(HitType.POISON, 0, -1f, 0, 0, 0f, damagePoints, true, true, base.transform.position, 0f, base.transform.position, Quaternion.identity, false, null, null, powerLevel);
		}
	}

	protected IEnumerator Start()
	{
		yield return new WaitForSeconds(destroyAfterSeconds);
		cachedParticleSystem.transform.parent = null;
		cachedParticleSystem.Stop();
		Object.Destroy(base.gameObject);
		Object.Destroy(cachedParticleSystem.gameObject, 4f);
	}
}
