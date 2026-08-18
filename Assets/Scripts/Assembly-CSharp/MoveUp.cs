using UnityEngine;

public class MoveUp : MonoBehaviour
{
	private void Update()
	{
		base.transform.Translate(Vector3.up * Time.deltaTime);
	}
}
