using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class BombFlask : MonoBehaviour, IDamageable
{
	[SerializeField]
	private float fuseLength;

	[SerializeField]
	private float animationLength = 0.5f;

	[SerializeField]
	private GameObject explosionEffect;

	[EventRef]
	[SerializeField]
	protected string fizzleSFX;

	[SerializeField]
	private bool fizzleInWater = true;

	protected bool justBlowUpNow;

	protected static List<BombFlask> bombFlasks = new List<BombFlask>();

	public void EnterWater()
	{
		if (fizzleInWater)
		{
			SFX.PlayAudioClipHere(fizzleSFX, base.transform.position);
			Object.Destroy(base.gameObject);
		}
	}

	private IEnumerator Start()
	{
		register(this);
		for (float f = 0f; f < fuseLength - animationLength; f += Time.deltaTime)
		{
			if (justBlowUpNow)
			{
				break;
			}
			yield return 0;
		}
		GetComponent<Animator>().SetTrigger("animate");
		for (float f = 0f; f < animationLength; f += Time.deltaTime)
		{
			if (justBlowUpNow)
			{
				break;
			}
			yield return 0;
		}
		Object.Destroy(base.gameObject);
		Object.Instantiate(explosionEffect, base.transform.position, Quaternion.identity);
		deregister(this);
	}

	protected void OnDestroy()
	{
		deregister(this);
	}

	bool IDamageable.ReceiveDamage(int damagePoints, int poisePoints, Vector3 physicsForce, float freezeTime, int poisonDamage)
	{
		justBlowUpNow = true;
		return true;
	}

	void IDamageable.BlockDamage(int sp, Vector3 physicsForce)
	{
	}

	public static Vector3? ClosestBombFlaskPosition(Vector3 position)
	{
		Vector3? result = null;
		float num = float.PositiveInfinity;
		for (int i = 0; i < bombFlasks.Count; i++)
		{
			float num2 = Vector3.Distance(bombFlasks[i].transform.position, position);
			if (num2 < num)
			{
				result = bombFlasks[i].transform.position;
				num = num2;
			}
		}
		return result;
	}

	protected static void register(BombFlask b)
	{
		bombFlasks.Add(b);
	}

	protected static void deregister(BombFlask b)
	{
		bombFlasks.Remove(b);
	}
}
