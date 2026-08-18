using UnityEngine;

public class VisibleByHavingInventoryItem : MonoBehaviour
{
	[SerializeField]
	protected Item enablingItem;

	[SerializeField]
	protected Renderer[] renderers;

	[SerializeField]
	protected Light[] lights;

	private void Update()
	{
		updateActiveness();
	}

	private void updateActiveness()
	{
		bool flag = enablingItem != null && enablingItem.Quantity > 0;
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].enabled = flag;
		}
		for (int j = 0; j < lights.Length; j++)
		{
			lights[j].enabled = flag;
		}
	}
}
