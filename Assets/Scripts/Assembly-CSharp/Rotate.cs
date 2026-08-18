using UnityEngine;

public class Rotate : MonoBehaviour
{
	[SerializeField]
	protected Vector3 eulerAnglesPerSecond;

	[SerializeField]
	protected bool unscaled;

	private void Update()
	{
		base.transform.Rotate(eulerAnglesPerSecond * (unscaled ? TimeManager.UnscaledDeltaTime : Time.deltaTime));
	}
}
