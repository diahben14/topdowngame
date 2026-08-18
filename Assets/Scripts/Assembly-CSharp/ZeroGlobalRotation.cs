using UnityEngine;

public class ZeroGlobalRotation : MonoBehaviour
{
	private void Update()
	{
		base.transform.rotation = Quaternion.identity;
	}
}
