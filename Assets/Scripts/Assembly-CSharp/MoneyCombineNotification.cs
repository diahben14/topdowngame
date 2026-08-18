using UnityEngine;

public class MoneyCombineNotification : MonoBehaviour
{
	private static MoneyCombineNotification _instance;

	public static void Show()
	{
		_instance._show();
	}

	protected void Awake()
	{
		_instance = this;
	}

	protected void _show()
	{
		GetComponent<Animator>().SetTrigger("show");
	}
}
