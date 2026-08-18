using UnityEngine;

public class DemoEndingDisplay : MonoBehaviour
{
	[SerializeField]
	protected Item needThree;

	private void Awake()
	{
		base.gameObject.SetActive(needThree.Quantity >= 3);
	}
}
