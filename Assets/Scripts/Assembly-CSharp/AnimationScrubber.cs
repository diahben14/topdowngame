using UnityEngine;

[RequireComponent(typeof(Animator))]
[ExecuteInEditMode]
public class AnimationScrubber : MonoBehaviour
{
	[SerializeField]
	public float scrubParameter;

	[SerializeField]
	protected string defaultStateName;

	protected Animator _animator;

	protected Animator animator
	{
		get
		{
			if (_animator == null)
			{
				_animator = GetComponent<Animator>();
			}
			return _animator;
		}
	}

	protected void sampleAt(float p)
	{
		if (animator.isInitialized && animator.runtimeAnimatorController != null)
		{
			animator.Play(defaultStateName, 0, p);
		}
	}

	protected void Update()
	{
		sampleAt(scrubParameter);
	}
}
