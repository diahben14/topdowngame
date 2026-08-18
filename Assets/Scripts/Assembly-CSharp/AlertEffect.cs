using System.Collections;
using UnityEngine;

public class AlertEffect : MonoBehaviour
{
	[SerializeField]
	protected float duration = 1f;

	protected Transform followTransform;

	protected float followHeight;

	public void SetFollowTransformAndHeight(Transform transform, float height)
	{
		followTransform = transform;
		followHeight = height;
	}

	public void StartSequence()
	{
		StartCoroutine(sequence());
	}

	protected IEnumerator sequence()
	{
		yield return new WaitForSeconds(duration);
		base.gameObject.SetActive(false);
	}

	protected void LateUpdate()
	{
		if (followTransform != null)
		{
			base.transform.position = followTransform.position + Vector3.up * followHeight;
		}
	}
}
