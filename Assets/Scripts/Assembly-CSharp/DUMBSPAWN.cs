using System.Collections;
using UnityEngine;

public class DUMBSPAWN : MonoBehaviour
{
	public float rate;

	public GameObject thing;

	private IEnumerator Start()
	{
		while (true)
		{
			if (rate > 0f)
			{
				Object.Instantiate(thing, base.transform.position + Random.onUnitSphere, Random.rotation);
				yield return new WaitForSeconds(1f / rate);
				rate += 1f;
			}
		}
	}
}
