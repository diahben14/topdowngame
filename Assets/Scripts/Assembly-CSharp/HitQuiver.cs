using System;
using System.Collections;
using UnityEngine;

public class HitQuiver : MonoBehaviour
{
	[SerializeField]
	protected bool doQuiver = true;

	[SerializeField]
	protected float quiverDuration = 0.5f;

	[SerializeField]
	protected float attenuationPower = 0.33f;

	[SerializeField]
	protected float quiverFrequency = 8f;

	[SerializeField]
	protected float quiverAmplitude = 0.75f;

	protected float lastQuiverTime = float.NegativeInfinity;

	protected float lastVibrateTime = float.NegativeInfinity;

	protected bool quiverCoroutineRunning;

	protected bool vibrateCoroutineRunning;

	protected float vibrationPulseTime;

	protected static float kVibrationPulseDuration = 0.125f;

	protected static float kVibrationDistance = 0.1f;

	protected bool _steadyVibration;

	public bool SteadVibration
	{
		set
		{
			_steadyVibration = value;
			if (_steadyVibration && !vibrateCoroutineRunning)
			{
				StartCoroutine(vibrateCoroutine());
			}
		}
	}

	public void Quiver()
	{
		if (doQuiver)
		{
			lastQuiverTime = Time.time;
			if (!quiverCoroutineRunning)
			{
				StartCoroutine(quiverCoroutine());
			}
		}
	}

	public void VibrationPulse()
	{
		vibrationPulseTime = Time.time;
		if (!vibrateCoroutineRunning)
		{
			StartCoroutine(vibrateCoroutine());
		}
	}

	protected IEnumerator quiverCoroutine()
	{
		quiverCoroutineRunning = true;
		while (Time.time < lastQuiverTime + quiverDuration)
		{
			if (quiverDuration > 0f)
			{
				float num = (Time.time - lastQuiverTime) / quiverDuration;
				float f = num * quiverFrequency * (float)Math.PI;
				float num2 = 0f - Mathf.Cos(f);
				float num3 = Mathf.Cos(f);
				float num4 = 1f - Mathf.Pow(num, attenuationPower);
				num2 *= num4;
				num3 *= num4;
				Vector3 vector = new Vector3(num3, num2, num3);
				Vector3 localScale = Vector3.one + vector * quiverAmplitude;
				base.transform.localScale = localScale;
			}
			yield return 0;
		}
		base.transform.localScale = Vector3.one;
		quiverCoroutineRunning = false;
	}

	protected IEnumerator vibrateCoroutine()
	{
		vibrateCoroutineRunning = true;
		while (_steadyVibration || Time.time < vibrationPulseTime + kVibrationPulseDuration)
		{
			float num = (_steadyVibration ? 1f : (2f * (1f - (Time.time - vibrationPulseTime) / kVibrationPulseDuration)));
			base.transform.localPosition = kVibrationDistance * UnityEngine.Random.onUnitSphere * num;
			yield return 0;
		}
		vibrateCoroutineRunning = false;
	}
}
