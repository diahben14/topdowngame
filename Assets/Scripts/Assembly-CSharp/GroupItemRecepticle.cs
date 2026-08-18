using UnityEngine;

public class GroupItemRecepticle : ItemRecepticle
{
	[SerializeField]
	private StateVariable[] groupStateVars;

	[SerializeField]
	private StateVariable finalStateVar;

	protected override void onPlaced()
	{
		for (int i = 0; i < groupStateVars.Length; i++)
		{
			if (!groupStateVars[i].BoolValue)
			{
				return;
			}
		}
		finalStateVar.BoolValue = true;
	}
}
