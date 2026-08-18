using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	protected Animator cachedAnimator;

	protected List<Rigidbody> rigidbodies = new List<Rigidbody>();

	private Vector3 lastPosition;

	private void OnTriggerEnter(Collider c)
	{
		Rigidbody component = c.gameObject.GetComponent<Rigidbody>();
		if (component != null)
		{
			rigidbodies.Add(component);
		}
	}

	private void OnTriggerExit(Collider c)
	{
		Rigidbody component = c.gameObject.GetComponent<Rigidbody>();
		if (component != null)
		{
			rigidbodies.Remove(component);
		}
	}

	private void Start()
	{
		cachedAnimator = GetComponent<Animator>();
		if (base.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
		{
			Debug.LogError("Moving platform component " + base.name + " needs to be Ignore Raycast", this);
		}
		lastPosition = base.transform.position;
	}

	private void LateUpdate()
	{
		if (!(lastPosition != base.transform.position))
		{
			return;
		}
		Vector3 vector = base.transform.position - lastPosition;
		for (int i = 0; i < rigidbodies.Count; i++)
		{
			if ((bool)rigidbodies[i])
			{
				Vector3 position = rigidbodies[i].position + vector;
				rigidbodies[i].MovePosition(position);
			}
		}
		lastPosition = base.transform.position;
	}
}
