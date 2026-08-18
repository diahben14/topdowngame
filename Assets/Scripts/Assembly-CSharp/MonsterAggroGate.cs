using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[ExecuteInEditMode]
public class MonsterAggroGate : MonoBehaviour
{
	[SerializeField]
	protected Monster[] monsters;

	[SerializeField]
	protected bool onlyOpenWhenDefeated;

	[SerializeField]
	protected float animationDuration = 1f;

	protected float internalParameter;

	protected Animator cachedAnimator;

	[HideInInspector]
	[SerializeField]
	protected string defaultStateName;

	protected bool _gateShouldBeClosed;

	protected bool coroutineRunning;

	protected bool gateShouldBeClosed
	{
		get
		{
			return _gateShouldBeClosed;
		}
		set
		{
			if (_gateShouldBeClosed != value)
			{
				_gateShouldBeClosed = value;
				onStateChange();
			}
		}
	}

	private void Awake()
	{
		cachedAnimator = GetComponent<Animator>();
		for (int i = 0; i < monsters.Length; i++)
		{
			if (monsters[i] != null)
			{
				Monster obj = monsters[i];
				obj.OnDestroyCallbacks = (Action)Delegate.Combine(obj.OnDestroyCallbacks, new Action(onCreatureDestroyed));
				Monster obj2 = monsters[i];
				obj2.OnAggroChangeCallbacks = (Action)Delegate.Combine(obj2.OnAggroChangeCallbacks, new Action(onCreatureAggroChange));
			}
		}
	}

	private void Start()
	{
		check();
		internalParameter = (gateShouldBeClosed ? 1 : 0);
		sampleAnimation();
	}

	protected void onCreatureDestroyed()
	{
		check();
	}

	protected void onCreatureAggroChange()
	{
		check();
	}

	protected void check()
	{
		bool flag = false;
		for (int i = 0; i < monsters.Length; i++)
		{
			if (monsters[i] != null && !monsters[i].IsDead && (monsters[i].IsAggroed || onlyOpenWhenDefeated))
			{
				flag = true;
			}
		}
		gateShouldBeClosed = flag;
	}

	private void OnDestroy()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		for (int i = 0; i < monsters.Length; i++)
		{
			if (monsters[i] != null)
			{
				Monster obj = monsters[i];
				obj.OnDestroyCallbacks = (Action)Delegate.Remove(obj.OnDestroyCallbacks, new Action(onCreatureDestroyed));
			}
		}
	}

	protected void onStateChange()
	{
		if (!coroutineRunning)
		{
			StartCoroutine(transitionCoroutine());
		}
	}

	protected IEnumerator transitionCoroutine()
	{
		coroutineRunning = true;
		float animationDuration = ((Time.timeSinceLevelLoad == 0f) ? 0f : this.animationDuration);
		while (internalParameter != (float)(gateShouldBeClosed ? 1 : 0))
		{
			if (animationDuration > 0f)
			{
				internalParameter += (float)(gateShouldBeClosed ? 1 : (-1)) / animationDuration * Time.deltaTime;
			}
			else
			{
				internalParameter = (gateShouldBeClosed ? 1 : 0);
			}
			internalParameter = Mathf.Clamp01(internalParameter);
			sampleAnimation();
			yield return 0;
		}
		coroutineRunning = false;
	}

	protected void sampleAnimation()
	{
		cachedAnimator.Play(defaultStateName, 0, internalParameter);
		cachedAnimator.speed = 0f;
	}
}
