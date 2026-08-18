using System;
using UnityEngine;

public class MonsterStateMonitor : MonoBehaviour
{
	[SerializeField]
	protected Monster[] monsters;

	protected IMonsterStateListener imsl;

	private void Awake()
	{
		for (int i = 0; i < monsters.Length; i++)
		{
			if (monsters[i] != null)
			{
				Monster obj = monsters[i];
				obj.OnDestroyCallbacks = (Action)Delegate.Combine(obj.OnDestroyCallbacks, new Action(onCreatureDestroyed));
				Monster obj2 = monsters[i];
				obj2.OnAggroChangeCallbacks = (Action)Delegate.Combine(obj2.OnAggroChangeCallbacks, new Action(onCreatureAggro));
			}
		}
	}

	private void Start()
	{
		imsl = (IMonsterStateListener)GetComponent(typeof(IMonsterStateListener));
		doAllDeadCheck();
	}

	protected void onCreatureDestroyed()
	{
		doAllDeadCheck();
	}

	protected void onCreatureAggro()
	{
		imsl.OnAnyMonsterAggro();
	}

	protected void doAllDeadCheck()
	{
		bool flag = true;
		for (int i = 0; i < monsters.Length; i++)
		{
			if (monsters[i] != null && !monsters[i].IsDead)
			{
				flag = false;
			}
		}
		if (flag)
		{
			imsl.OnAllMonstersDead();
		}
	}
}
