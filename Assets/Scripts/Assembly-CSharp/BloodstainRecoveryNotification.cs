using UnityEngine;

public class BloodstainRecoveryNotification : MonoBehaviour
{
	protected static BloodstainRecoveryNotification instance;

	protected Animator cachedAnimator;

	public static void Show()
	{
		instance.cachedAnimator.SetTrigger("show");
	}

	protected void Awake()
	{
		instance = this;
		cachedAnimator = GetComponent<Animator>();
	}
}
