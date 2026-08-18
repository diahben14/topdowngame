using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
	public class FPSCounter : MonoBehaviour
	{
		private const float fpsMeasurePeriod = 0.5f;

		private const string display = "{0:0000} mspf; {1:0000.#} fps \n{2:0000} calls per second, avg value {3:0.##}";

		protected int frameAccumulator;

		protected float nextPeriodTime;

		[SerializeField]
		private Text text;

		protected static int eventAccumulator;

		protected static float eventValueAccumulator;

		public static void LogEvent(float value)
		{
			eventAccumulator++;
			eventValueAccumulator += value;
		}

		private void Start()
		{
			nextPeriodTime = Time.realtimeSinceStartup + 0.5f;
		}

		private void Update()
		{
			if (RuntimeQualityManager.ShowFPS)
			{
				text.enabled = true;
				frameAccumulator++;
				if (Time.realtimeSinceStartup > nextPeriodTime)
				{
					nextPeriodTime += 0.5f;
					text.text = string.Format("{0:0000} mspf; {1:0000.#} fps \n{2:0000} calls per second, avg value {3:0.##}", 0.5f / (float)frameAccumulator * 1000f, (float)frameAccumulator / 0.5f, (float)eventAccumulator / 0.5f, eventValueAccumulator / (float)eventAccumulator);
					frameAccumulator = 0;
					eventAccumulator = 0;
					eventValueAccumulator = 0f;
				}
			}
			else
			{
				nextPeriodTime = Time.realtimeSinceStartup + 0.5f;
				frameAccumulator = 0;
				eventAccumulator = 0;
				eventValueAccumulator = 0f;
				text.enabled = false;
			}
		}
	}
}
