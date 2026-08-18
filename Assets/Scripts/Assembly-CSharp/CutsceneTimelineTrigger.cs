using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTimelineTrigger : MonoBehaviour
{
	[SerializeField]
	protected PlayableDirector timelineDirector;

	private void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<PlayerCharacter>() != null)
		{
			timelineDirector.Play();
		}
	}
}
