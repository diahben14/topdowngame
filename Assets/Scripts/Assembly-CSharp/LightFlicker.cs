using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
	[SerializeField]
	protected float maxMovementAmount;

	[SerializeField]
	protected float maxIntensityDifference = 0.5f;

	[SerializeField]
	protected float flickerSpeed = 100f;

	[SerializeField]
	protected float maxFlickerSpeedDifference = 20f;

	protected float targetIntensity;

	private IEnumerator Start()
	{
		Light light = GetComponent<Light>();
		float originalIntensity = light.intensity;
		Vector3 originalLocalPosition = base.transform.localPosition;
		while (true)
		{
			float flickerDuration = 1f / (flickerSpeed + Random.Range(maxFlickerSpeedDifference, maxFlickerSpeedDifference));
			if (maxMovementAmount <= 0f || flickerDuration <= 0f)
			{
				yield return 0;
				continue;
			}
			Vector3 startPosition = base.transform.localPosition;
			Vector3 endPosition = originalLocalPosition + Random.insideUnitSphere * maxMovementAmount;
			float startIntensity = originalIntensity;
			float num = Random.Range(0f - maxIntensityDifference, maxIntensityDifference);
			float endIntensity = originalIntensity + num;
			for (float f = 0f; f < 1f; f += Time.deltaTime / flickerDuration)
			{
				base.transform.localPosition = Vector3.Lerp(startPosition, endPosition, f);
				light.intensity = Mathf.Lerp(startIntensity, endIntensity, f);
				yield return 0;
			}
		}
	}
}
