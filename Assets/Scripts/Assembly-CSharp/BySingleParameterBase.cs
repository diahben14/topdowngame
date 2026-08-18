using UnityEngine;

public abstract class BySingleParameterBase : MonoBehaviour
{
	[SerializeField]
	protected string parameterName = "potent";

	[SerializeField]
	[Tooltip("turn on to bypass the local animator. Used when this is a sub-animator and you want to get a parameter from the ancestral animator")]
	protected bool useParentsAnimator;

	protected Animator _cachedAnimator;

	protected const float kMinimumDistanceFromZeroForTrue = 0.01f;

	protected Animator cachedAnimator
	{
		get
		{
			if (_cachedAnimator == null)
			{
				if (useParentsAnimator)
				{
					_cachedAnimator = base.gameObject.transform.parent.GetComponentInParent<Animator>();
				}
				else
				{
					_cachedAnimator = base.gameObject.GetComponentInParent<Animator>();
				}
				if (cachedAnimator == null)
				{
					Debug.LogError("[ByParameterBase] `" + base.name + "` couldn't find ancestor animator.", base.gameObject);
				}
			}
			return _cachedAnimator;
		}
	}

	protected void Awake()
	{
		setup();
	}

	protected virtual void setup()
	{
	}

	protected abstract void setValue(bool boolValue, float floatValue);

	private void LateUpdate()
	{
		if (cachedAnimator != null)
		{
			float @float = cachedAnimator.GetFloat(parameterName);
			bool boolValue = @float > 0.01f;
			setValue(boolValue, @float);
		}
		else
		{
			setValue(false, 0f);
		}
	}
}
