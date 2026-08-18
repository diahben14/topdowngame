using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TrapTile : MonoBehaviour
{
	[SerializeField]
	public bool activatedByPlayer;

	[SerializeField]
	protected float telegraphDuration;

	[SerializeField]
	protected float hurtDuration;

	protected bool sequenceRunning;

	protected Animator cachedAnimator;

	protected float delay;

	protected static List<TrapTile> registered = new List<TrapTile>();

	protected static void register(TrapTile tt)
	{
		registered.Add(tt);
	}

	protected static void deregister(TrapTile tt)
	{
		registered.Remove(tt);
	}

	public static void ActivateWithinRadius(Vector3 position, float minRadius, float maxRadius, float propagationSpeed)
	{
		for (int i = 0; i < registered.Count; i++)
		{
			TrapTile trapTile = registered[i];
			float num = Mathx.Chebyshev(trapTile.transform.position, position);
			if (num <= maxRadius)
			{
				float d = 0f;
				if (propagationSpeed > 0f)
				{
					float t = Mathf.InverseLerp(minRadius, maxRadius, num);
					float b = (maxRadius - minRadius) / propagationSpeed;
					d = Mathf.Lerp(0f, b, t);
				}
				trapTile.triggerSequence(d);
			}
		}
	}

	protected void Awake()
	{
		cachedAnimator = GetComponent<Animator>();
		register(this);
	}

	protected void OnDestroy()
	{
		deregister(this);
	}

	protected void OnTriggerStay(Collider c)
	{
		if (activatedByPlayer && (bool)c.GetComponent<TrapTileTrigger>())
		{
			triggerSequence(0f);
		}
	}

	protected void triggerSequence(float d)
	{
		if (sequenceRunning)
		{
			if (d < delay)
			{
				delay = d;
			}
		}
		else
		{
			delay = d;
			StartCoroutine(sequence());
		}
	}

	protected IEnumerator sequence()
	{
		sequenceRunning = true;
		for (float f = 0f; f < delay; f += Time.deltaTime)
		{
			yield return 0;
		}
		cachedAnimator.SetFloat("telegraph_speed", 1f / telegraphDuration);
		cachedAnimator.SetTrigger("start");
		yield return new WaitForSeconds(telegraphDuration + hurtDuration);
		cachedAnimator.SetTrigger("stop");
		sequenceRunning = false;
	}
}
