using UnityEngine;

public class TuningForkBell : MonoBehaviour, IInteractionReceiver, IDamageable
{
	[SerializeField]
	protected StateVariable stateVar;

	[SerializeField]
	protected bool isSpiritWorldBell;

	[SerializeField]
	protected float animate_gyroSpin;

	[SerializeField]
	protected Transform[] gyroTransforms;

	[SerializeField]
	protected float gyroTimescale;

	bool IInteractionReceiver.Interact(Item i)
	{
		if (interactionCheck())
		{
			doRing();
			return true;
		}
		return false;
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return interactionCheck();
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return interactionCheck();
	}

	bool IDamageable.ReceiveDamage(int damagePoints, int poisePoints, Vector3 physicsForce, float freezeTime, int poisonDamage)
	{
		return false;
	}

	void IDamageable.BlockDamage(int sp, Vector3 physicsForce)
	{
		if (!isSpiritWorldBell)
		{
			doRing();
		}
	}

	protected bool interactionCheck()
	{
		if (isSpiritWorldBell)
		{
			return !stateVar.BoolValue;
		}
		return false;
	}

	protected void doRing()
	{
		if (!stateVar.BoolValue)
		{
			bool isSpiritWorldBell2 = isSpiritWorldBell;
		}
		GetComponent<Animator>().SetTrigger("ring");
		if (!stateVar.BoolValue)
		{
			stateVar.BoolValue = true;
		}
	}

	protected void Start()
	{
		if (stateVar.BoolValue)
		{
			GetComponent<Animator>().Play("active");
		}
		else
		{
			stateVar.SubscribeToChanges(onStateChange);
		}
	}

	protected void onStateChange()
	{
		if (stateVar.BoolValue)
		{
			doRing();
		}
	}

	protected void FixedUpdate()
	{
		float num = gyroTimescale;
		float num2 = 45f * animate_gyroSpin * Time.deltaTime;
		Quaternion quaternion = Quaternion.Euler(0f, num2 * num, 0f) * Quaternion.Euler((0f - num2) * num * 0.56f, 0f, 0f) * Quaternion.Euler(0f, 0f, num2 * num * 0.33f);
		quaternion.Normalize();
		gyroTransforms[0].localRotation = quaternion * gyroTransforms[0].localRotation;
		gyroTransforms[1].localRotation = quaternion * gyroTransforms[1].localRotation;
	}
}
