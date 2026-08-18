using UnityEngine;

[RequireComponent(typeof(AnimationScrubber))]
[ExecuteInEditMode]
public class AnimationScrubberByParameter : BySingleParameterBase
{
	private AnimationScrubber cachedAnimationScrubber;

	[SerializeField]
	protected bool monoDirectionalEffect;

	protected float lastValue;

	protected override void setup()
	{
		cachedAnimationScrubber = GetComponent<AnimationScrubber>();
	}

	protected override void setValue(bool boolValue, float floatValue)
	{
		if (monoDirectionalEffect)
		{
			if (floatValue < lastValue)
			{
				cachedAnimationScrubber.scrubParameter = 0f;
			}
			else
			{
				cachedAnimationScrubber.scrubParameter = floatValue;
			}
		}
		else
		{
			cachedAnimationScrubber.scrubParameter = floatValue;
		}
		lastValue = floatValue;
	}
}
