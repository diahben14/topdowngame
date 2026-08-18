using System.Collections;
using UnityEngine;

public class GameMode : GUIMode
{
	[SerializeField]
	protected float animationDuration = 0.5f;

	protected Animator cachedAnimator;

	protected override void guiModeSetup()
	{
		cachedAnimator = GetComponent<Animator>();
	}

	protected override IEnumerator afterEnable()
	{
		cachedAnimator.SetBool("show", true);
		for (float f = 0f; f < animationDuration; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
	}

	protected override IEnumerator beforeDisable()
	{
		cachedAnimator.SetBool("show", false);
		for (float f = 0f; f < animationDuration; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
	}
}
