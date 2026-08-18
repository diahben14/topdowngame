using System.Collections;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[ExecuteInEditMode]
public class ToggleObjectAnimation : MonoBehaviour
{
	[SerializeField]
	protected float animationDuration = 2f;

	[SerializeField]
	protected float initialDelay;

	[SerializeField]
	[EventRef]
	protected string onStateBecomesTrueSFX;

	protected float internalParameter;

	protected bool desiredToggleState;

	protected Animator cachedAnimator;

	[HideInInspector]
	[SerializeField]
	protected string defaultStateName;

	protected bool coroutineRunning;

	protected bool sfxPrimedToPlay;

	public void SetToggle(bool state)
	{
		desiredToggleState = state;
		if (!coroutineRunning)
		{
			StartCoroutine(transitionCoroutine());
		}
	}

	public void SetToggleInstant(bool state)
	{
		internalParameter = (state ? 1 : 0);
		sampleAnimation();
	}

	private void Awake()
	{
		cachedAnimator = GetComponent<Animator>();
	}

	protected IEnumerator transitionCoroutine()
	{
		coroutineRunning = true;
		sfxPrimedToPlay = true;
		while (internalParameter != (float)(desiredToggleState ? 1 : 0))
		{
			if (animationDuration > 0f)
			{
				internalParameter += (float)(desiredToggleState ? 1 : (-1)) / (animationDuration + initialDelay) * Time.deltaTime;
			}
			else
			{
				internalParameter = (desiredToggleState ? 1 : 0);
			}
			internalParameter = Mathf.Clamp01(internalParameter);
			sampleAnimation();
			yield return 0;
		}
		coroutineRunning = false;
	}

	protected void sampleAnimation()
	{
		float num = (initialDelay + animationDuration) / animationDuration;
		float num2 = Mathf.Clamp01(internalParameter * num - num + 1f);
		cachedAnimator.Play(defaultStateName, 0, num2);
		if (num2 > 0f && sfxPrimedToPlay)
		{
			SFX.PlayAudioClipHere(onStateBecomesTrueSFX, base.transform.position);
			sfxPrimedToPlay = false;
		}
	}
}
