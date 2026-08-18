using System;
using UnityEngine;

public class PrismCog : MonoBehaviour
{
	[SerializeField]
	protected float phaseOffset;

	[SerializeField]
	protected float speed = 1f;

	[SerializeField]
	protected float amplitude = 8f;

	protected Vector3 startPosition;

	private void Start()
	{
		startPosition = base.transform.position;
	}

	private void Update()
	{
		float num = Mathf.Sin((Time.time + phaseOffset) * speed * (float)Math.PI * 2f) * amplitude;
		base.transform.position = startPosition + Vector3.up * num;
	}
}
