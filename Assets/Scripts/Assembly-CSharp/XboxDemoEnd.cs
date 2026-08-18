using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class XboxDemoEnd : MonoBehaviour
{
	[SerializeField]
	protected float delay = 2f;

	[SerializeField]
	protected float transitionDuration = 2f;

	[SerializeField]
	protected bool whiteout;

	protected bool sequenceRunning;

	protected void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<PlayerCharacter>() != null && !sequenceRunning)
		{
			StartCoroutine(Sequence());
		}
	}

	protected IEnumerator Sequence()
	{
		sequenceRunning = true;
		yield return new WaitForSeconds(delay);
		if (transitionDuration > 0f)
		{
			for (float i = 0f; i < 1f; i += TimeManager.UnscaledDeltaTime / transitionDuration)
			{
				FullscreenFader.Whiteout = true;
				CameraController.CutsceneFade = i;
				yield return 0;
			}
		}
		CameraController.CutsceneFade = 1f;
		PlayerCharacterSpawn.GoTo("Archipelagos Redux", "Sewer_DemoBoss_");
	}
}
