using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class CycleController : MonoBehaviour
{
	[Range(0f, 1f)]
	[SerializeField]
	protected float previewParameter;

	protected const string kNightStateVarName = "Is Night";

	protected static StateVariable nightStateVar;

	protected const float kTransitionDuration = 3f;

	protected static float dayNight;

	protected static CycleController instance;

	protected Animator cachedAnimator;

	[SerializeField]
	protected string defaultStateName;

	public static bool IsNight
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static StateVariable IsNightStateVar
	{
		get
		{
			getNightStateVarIfNeeded();
			return nightStateVar;
		}
	}

	protected static float DayNight
	{
		get
		{
			return dayNight;
		}
		set
		{
			dayNight = value;
			if ((bool)instance)
			{
				instance.sampleAt(dayNight);
			}
		}
	}

	public static float AnimateSunset()
	{
		instance.StartCoroutine(instance.animateFromTo(0f, 0.5f, 3f));
		return 3f;
	}

	public static float AnimateSunrise()
	{
		instance.StartCoroutine(instance.animateFromTo(0.5f, 1f, 3f));
		return 3f;
	}

	public static void DoSweep()
	{
		if (instance != null && instance.cachedAnimator != null)
		{
			instance.cachedAnimator.SetTrigger("sweep");
		}
	}

	protected static void getNightStateVarIfNeeded()
	{
		if (nightStateVar == null)
		{
			nightStateVar = StateVariable.GetStateVariableByName("Is Night");
		}
	}

	protected IEnumerator animateFromTo(float start, float end, float duration)
	{
		if (duration > 0f)
		{
			sampleAt(start);
			for (float f = 0f; f < duration; f += Time.deltaTime)
			{
				yield return 0;
				float t = f / duration;
				sampleAt(Mathf.SmoothStep(start, end, t));
			}
		}
		sampleAt(end);
	}

	protected void sampleAt(float p)
	{
		if ((bool)cachedAnimator)
		{
			cachedAnimator.enabled = true;
			cachedAnimator.Play(defaultStateName, 0, p);
		}
	}

	protected void Awake()
	{
		if (Application.isPlaying)
		{
			Setup.DoStandardSetup();
			instance = this;
			cachedAnimator = GetComponent<Animator>();
			if (cachedAnimator != null && cachedAnimator.GetCurrentAnimatorStateInfo(0).speed != 0f)
			{
				Debug.LogErrorFormat("Cycle animation state on object {0} should have its speed set to 0.", base.gameObject.name);
			}
		}
	}
}
