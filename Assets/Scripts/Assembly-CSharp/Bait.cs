using System.Collections;
using UnityEngine;

public class Bait : MonoBehaviour
{
	[SerializeField]
	protected float baitDuration = 10f;

	[SerializeField]
	protected float flickerTime = 2f;

	[SerializeField]
	protected Renderer flickerRenderer;

	public static Bait LatestBate { get; private set; }

	private IEnumerator Start()
	{
		LatestBate = this;
		yield return new WaitForSeconds(baitDuration - flickerTime);
		for (float f = 0f; f < flickerTime; f += Time.deltaTime)
		{
			flickerRenderer.enabled = !flickerRenderer.enabled;
			yield return 0;
		}
		if (LatestBate == this)
		{
			LatestBate = null;
		}
		Object.Destroy(base.gameObject);
	}
}
