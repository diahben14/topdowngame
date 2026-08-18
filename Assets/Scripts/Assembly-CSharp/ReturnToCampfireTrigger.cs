using System.Collections;
using UnityEngine;

public class ReturnToCampfireTrigger : MonoBehaviour
{
	[SerializeField]
	protected float transitionDuration;

	private void OnTriggerEnter(Collider c)
	{
		StartCoroutine(sequence());
	}

	private IEnumerator sequence()
	{
		FullscreenFader.Whiteout = true;
		if (transitionDuration > 0f)
		{
			for (float i = 0f; i < 1f; i += TimeManager.UnscaledDeltaTime / transitionDuration)
			{
				PlayerCharacterSpawn.SceneTransitionFade = i;
				yield return 0;
			}
		}
		Campfire.RespawnAtLastCampfire();
	}
}
