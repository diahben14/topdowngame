using System.Collections;
using UnityEngine;

public class LighthouseCutscene : MonoBehaviour
{
	[SerializeField]
	protected float minDeltaPerSecond = 5f;

	[SerializeField]
	protected float sustainedDurationToTriggerCutscene = 20f;

	[SerializeField]
	protected StateVariable towerRaisedStateVar;

	protected float lastAngle;

	protected float lastBadDeltaTime;

	private IEnumerator Start()
	{
		lastBadDeltaTime = Time.time;
		while (true)
		{
			if (PlayerCharacter.Instanced)
			{
				Vector3 normalized = (PlayerCharacter.WorldPosition - base.transform.position).normalized;
				normalized.y = 0f;
				float num = (0f - Mathf.Atan2(normalized.z, normalized.x)) * 57.29578f;
				float num2 = num - lastAngle;
				if (num < -90f && lastAngle > 90f)
				{
					Debug.Log("flip over");
					num2 += 360f;
				}
				if (num2 < 0f)
				{
					lastBadDeltaTime = Time.time;
				}
				else if (num2 > minDeltaPerSecond)
				{
					if (Time.time > lastBadDeltaTime + sustainedDurationToTriggerCutscene)
					{
						towerRaisedStateVar.BoolValue = true;
					}
				}
				else
				{
					lastBadDeltaTime = Time.time;
				}
				lastAngle = num;
			}
			yield return new WaitForSeconds(1f);
		}
	}
}
