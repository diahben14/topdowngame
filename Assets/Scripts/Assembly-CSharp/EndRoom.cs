using System.Collections;
using UnityEngine;

public class EndRoom : MonoBehaviour
{
	[SerializeField]
	protected StateVariable stateVar;

	[SerializeField]
	protected Collider exitBlocker;

	[SerializeField]
	protected Animator shadowAnimator;

	[SerializeField]
	protected float delayBeforeBlack;

	private void Start()
	{
		shadowAnimator.speed = 0f;
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!stateVar.BoolValue && c.GetComponent<PlayerCharacter>() != null)
		{
			stateVar.BoolValue = true;
			StartCoroutine(doIt());
		}
	}

	protected IEnumerator doIt()
	{
		exitBlocker.enabled = true;
		shadowAnimator.speed = 1f;
		for (float f = 0f; f < delayBeforeBlack; f += Time.deltaTime)
		{
			CameraController.AddScreenshake(Time.deltaTime / Mathf.Lerp(100f, 10f, f));
			yield return 0;
		}
		MusicManager.Stop();
		GUIMode.PushMode(GUIMode.GetMode<DemoEnd>());
	}
}
