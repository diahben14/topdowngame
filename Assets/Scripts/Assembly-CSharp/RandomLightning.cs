using System.Collections;
using UnityEngine;

public class RandomLightning : MonoBehaviour
{
	[SerializeField]
	protected bool doFlashes = true;

	[SerializeField]
	protected float averageWait = 3f;

	[SerializeField]
	protected float flashIntensity = 2f;

	[SerializeField]
	protected float flashLength = 0.5f;

	private Light cached_light;

	protected IEnumerator Start()
	{
		cached_light = GetComponent<Light>();
		while (true)
		{
			yield return new WaitForSeconds(Mathx.Poisson(averageWait));
			if (doFlashes)
			{
				cached_light.transform.rotation = Random.rotation;
				LightingController.SuppressAmbientForSeconds(flashLength);
				for (float f = 0f; f < flashLength; f += Time.deltaTime)
				{
					float num = 1f - f / flashLength;
					num *= num;
					cached_light.intensity = flashIntensity * num;
					yield return 0;
				}
			}
		}
	}
}
