using System.Collections;
using UnityEngine;

public abstract class ItemBehaviour : MonoBehaviour
{
	[SerializeField]
	protected GameObject itemRoot;

	protected bool _visible;

	protected Animator cachedAnimator;

	protected bool animationWasInterrupted;

	protected bool wasCancelled;

	public bool Visible
	{
		get
		{
			return _visible;
		}
		set
		{
			_visible = value;
			if ((bool)itemRoot)
			{
				itemRoot.SetActive(value);
			}
		}
	}

	public virtual bool ShouldUsageMakeItemRootVisible
	{
		get
		{
			return true;
		}
	}

	public abstract bool IsItemBehaviourForItem(Item i);

	public void AnimationWasInterrupted()
	{
		animationWasInterrupted = true;
	}

	public void Cancel()
	{
		wasCancelled = true;
	}

	protected void Awake()
	{
		Visible = false;
		cachedAnimator = GetComponent<Animator>();
		itemBehaviour_Awake();
	}

	protected virtual void itemBehaviour_Awake()
	{
	}

	public IEnumerator OnActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonIndex, ButtonAssignableItem[] itemUsedThisFrame)
	{
		if (IsItemBehaviourForItem(itemUsed))
		{
			animationWasInterrupted = false;
			wasCancelled = false;
			yield return StartCoroutine(onActionButtonDown(itemUsed, actionButtonIndex, itemUsedThisFrame));
		}
	}

	protected abstract IEnumerator onActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonUp, ButtonAssignableItem[] itemUsedThisFrame);
}
