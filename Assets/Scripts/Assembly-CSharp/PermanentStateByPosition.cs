using UnityEngine;

public class PermanentStateByPosition : MonoBehaviour
{
	[SerializeField]
	private bool stayDeadForever = true;

	protected Vector3 initialPosition;

	private bool permanentlyDead
	{
		get
		{
			return PermanentStateByPositionManager.GetDead(initialPosition);
		}
		set
		{
			PermanentStateByPositionManager.SetDead(initialPosition, value);
		}
	}

	private void Start()
	{
		initialPosition = base.transform.position;
		if (permanentlyDead && stayDeadForever)
		{
			goToDeadState();
		}
		PermanentStateByPosition_Start();
	}

	protected virtual void PermanentStateByPosition_Start()
	{
	}

	protected virtual void goToDeadState()
	{
		Object.Destroy(base.gameObject);
	}

	protected void onKilled()
	{
		permanentlyDead = true;
	}
}
