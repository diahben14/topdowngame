using UnityEngine;

public class QuaternionDiffTest : MonoBehaviour
{
	private void Update()
	{
		float num = Quaternion.Angle(base.transform.rotation, Quaternion.identity);
		Debug.Log("QuaternionDiffTest angle: " + num);
	}
}
