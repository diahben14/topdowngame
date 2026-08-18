using System.Collections;
using UnityEngine;

public class DisableAfterSeconds : MonoBehaviour
{
	[SerializeField]
	protected float seconds;

	protected void OnEnable()
	{
		StartCoroutine(sequence());
	}

	protected IEnumerator sequence()
	{
		yield return new WaitForSeconds(seconds);
		base.gameObject.SetActive(false);
	}
}
