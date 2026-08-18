using UnityEngine;

public class IntroText : MonoBehaviour
{
	[SerializeField]
	protected Animator animator;

	protected bool played;

	private void Awake()
	{
		animator.speed = 0f;
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<PlayerCharacter>() != null && !played)
		{
			animator.speed = 1f;
		}
	}
}
