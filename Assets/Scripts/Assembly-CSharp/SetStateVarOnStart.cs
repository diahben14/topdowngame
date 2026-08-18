using UnityEngine;

public class SetStateVarOnStart : MonoBehaviour
{
	[SerializeField]
	protected StateVariable stateVariable;

	[SerializeField]
	protected bool setTo;

	private void Start()
	{
		stateVariable.BoolValue = setTo;
	}
}
