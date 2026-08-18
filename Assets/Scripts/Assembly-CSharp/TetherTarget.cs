using System.Collections;
using UnityEngine;

public class TetherTarget : MonoBehaviour
{
	[SerializeField]
	protected bool grapplePoint;

	[SerializeField]
	protected float grappleDepositDistance = 2f;

	protected Rigidbody cachedRigidbody;

	protected int coroutineTicket;

	public bool IsGrapplePoint
	{
		get
		{
			return grapplePoint;
		}
	}

	public float GrappleDepositRadius
	{
		get
		{
			return grappleDepositDistance;
		}
	}

	protected void Start()
	{
		cachedRigidbody = GetComponent<Rigidbody>();
	}

	public void DoTether(Vector3 pullToPosition, float speed, Vector3 forceVector)
	{
		StartCoroutine(tetherCoroutine(pullToPosition, speed));
	}

	protected IEnumerator tetherCoroutine(Vector3 target, float speed)
	{
		if (cachedRigidbody == null)
		{
			Debug.LogError("TetherTarget needs rigidbody unless it's a grapple point.");
			yield break;
		}
		int myTicket = ++coroutineTicket;
		bool tethering = true;
		while (myTicket == coroutineTicket && tethering)
		{
			if (Time.deltaTime > 0f)
			{
				Vector3 vector = target - base.transform.position;
				Vector3 velocity = vector.normalized * speed;
				if (velocity.magnitude * Time.deltaTime > vector.magnitude)
				{
					cachedRigidbody.velocity = vector / Time.deltaTime;
					yield return 0;
					cachedRigidbody.velocity = Vector3.zero;
					tethering = false;
				}
				else
				{
					cachedRigidbody.velocity = velocity;
				}
			}
			yield return 0;
		}
	}
}
