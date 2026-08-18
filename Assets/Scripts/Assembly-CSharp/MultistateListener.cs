using UnityEngine;

public class MultistateListener : MonoBehaviour
{
	[SerializeField]
	protected StateVariable[] stateVarsToListen;

	[SerializeField]
	protected StateVariable stateVarToTurnTrue;

	private void Start()
	{
		for (int i = 0; i < stateVarsToListen.Length; i++)
		{
			stateVarsToListen[i].SubscribeToChanges(Callback);
		}
	}

	public void Callback()
	{
		stateVarToTurnTrue.BoolValue = allAreTrue();
	}

	protected bool allAreTrue()
	{
		for (int i = 0; i < stateVarsToListen.Length; i++)
		{
			if (!stateVarsToListen[i].BoolValue)
			{
				return false;
			}
		}
		return true;
	}
}
