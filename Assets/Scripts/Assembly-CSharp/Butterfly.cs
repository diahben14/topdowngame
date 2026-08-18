using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Butterfly : MonoBehaviour
{
	[SerializeField]
	protected float maxDestinationDistance = 5f;

	[SerializeField]
	protected float moveSpeed = 1f;

	[SerializeField]
	protected float maximumIdleTime = 3f;

	private IEnumerator Start()
	{
		Animator animator = GetComponent<Animator>();
		while (true)
		{
			Vector3 destination = getNewDestination();
			Vector3 start = base.transform.position;
			if (!(moveSpeed > 0f))
			{
				continue;
			}
			animator.SetBool("fly", true);
			float duration = Vector3.Distance(start, destination) / moveSpeed;
			base.transform.rotation = Quaternion.LookRotation(destination - start);
			if (duration > 0f)
			{
				for (float f = 0f; f < 1f; f += Time.deltaTime / duration)
				{
					base.transform.position = Vector3.Lerp(start, destination, Mathf.SmoothStep(0f, 1f, f));
					yield return 0;
				}
			}
			animator.SetBool("fly", false);
			yield return new WaitForSeconds(Random.Range(0f, maximumIdleTime));
		}
	}

	protected Vector3 getNewDestination()
	{
		Vector3 onUnitSphere = Random.onUnitSphere;
		onUnitSphere.y = 0f;
		onUnitSphere.Normalize();
		NavMeshHit hit;
		if (NavMesh.SamplePosition(base.transform.position + onUnitSphere * Random.Range(0f, maxDestinationDistance), out hit, float.PositiveInfinity, -1))
		{
			return hit.position;
		}
		return base.transform.position;
	}
}
