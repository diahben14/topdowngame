using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class LightingController : MonoBehaviour
{
	[SerializeField]
	protected Color ambientColor = new Color(0.749f, 0.882f, 1f);

	[SerializeField]
	protected Color ambientColorEquator = new Color(0.749f, 0.882f, 1f);

	[SerializeField]
	protected Color ambientColorGround = new Color(0.749f, 0.882f, 1f);

	[SerializeField]
	protected float ambientIntensity = 0.5f;

	[SerializeField]
	protected Color fogColor = Color.white;

	[SerializeField]
	protected Light lightToSuppressDuringAmbientSupression;

	private static float lastAmbientSuppressTime = float.NegativeInfinity;

	private static float ambientSuppressDuration = 0f;

	public static void SuppressAmbientForSeconds(float s)
	{
		lastAmbientSuppressTime = Time.time;
		ambientSuppressDuration = s;
	}

	protected void LateUpdate()
	{
		if (RenderSettings.ambientMode != AmbientMode.Trilight)
		{
			RenderSettings.ambientMode = AmbientMode.Trilight;
		}
		RenderSettings.ambientLight = ambientColor * ambientIntensity;
		RenderSettings.ambientEquatorColor = ambientColorEquator * ambientIntensity;
		RenderSettings.ambientGroundColor = ambientColorGround * ambientIntensity;
		RenderSettings.fogColor = fogColor;
		float num = ambientIntensity;
		if (lastAmbientSuppressTime + ambientSuppressDuration > Time.time && Application.isPlaying)
		{
			float num2 = Mathf.InverseLerp(lastAmbientSuppressTime, lastAmbientSuppressTime + ambientSuppressDuration, Time.time);
			num *= num2;
			if ((bool)lightToSuppressDuringAmbientSupression)
			{
				lightToSuppressDuringAmbientSupression.intensity *= num2;
			}
		}
		if ((double)RenderSettings.reflectionIntensity != 0.5)
		{
			RenderSettings.reflectionIntensity = 0.5f;
		}
	}
}
