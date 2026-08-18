using UnityEngine;

[ExecuteInEditMode]
public class FaceMainCamera : MonoBehaviour
{
	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
	}
}
