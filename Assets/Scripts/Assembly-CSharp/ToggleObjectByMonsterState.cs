using UnityEngine;

[RequireComponent(typeof(ToggleObjectAnimation))]
[RequireComponent(typeof(MonsterStateMonitor))]
public class ToggleObjectByMonsterState : MonoBehaviour, IMonsterStateListener
{
	[SerializeField]
	protected bool startOpenAndCloseOnAggro;

	[SerializeField]
	protected bool openOnAllDead = true;

	protected ToggleObjectAnimation _cachedToggleObjectAnimation;

	protected bool allMonstersDead;

	protected ToggleObjectAnimation cachedToggleObjectAnimation
	{
		get
		{
			if (_cachedToggleObjectAnimation == null && this != null)
			{
				_cachedToggleObjectAnimation = GetComponent<ToggleObjectAnimation>();
			}
			return _cachedToggleObjectAnimation;
		}
	}

	protected void Start()
	{
		if (startOpenAndCloseOnAggro)
		{
			cachedToggleObjectAnimation.SetToggleInstant(true);
		}
	}

	void IMonsterStateListener.OnAllMonstersDead()
	{
		if (this != null && openOnAllDead)
		{
			cachedToggleObjectAnimation.SetToggle(true);
		}
	}

	void IMonsterStateListener.OnAnyMonsterAggro()
	{
		if (startOpenAndCloseOnAggro)
		{
			cachedToggleObjectAnimation.SetToggle(false);
		}
	}
}
