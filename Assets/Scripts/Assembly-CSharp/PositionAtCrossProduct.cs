using UnityEngine;

public class PositionAtCrossProduct : MonoBehaviour
{
	[SerializeField]
	protected Transform targetTransform;

	[SerializeField]
	protected float distance = 8f;

	[SerializeField]
	protected float paramTowardsTarget;

	[SerializeField]
	protected bool useParamDistance = true;

	[SerializeField]
	protected float paramDistance = 0.25f;

	[SerializeField]
	protected float altitudeOffset;

	protected Vector3 initialPosition;

	private void Start()
	{
		initialPosition = base.transform.position;
	}

	private void LateUpdate()
	{
		if (PlayerCharacter.Instanced && targetTransform != null)
		{
			Vector3 vector = targetTransform.position - PlayerCharacter.WorldPosition;
			Vector3 normalized = Vector3.Cross(CameraController.MainCameraForward, vector).normalized;
			Vector3 normalized2 = Vector3.Cross(vector.normalized, normalized.normalized).Flatten().normalized;
			Debug.DrawRay(PlayerCharacter.WorldPosition, vector, Color.red);
			Debug.DrawRay(PlayerCharacter.WorldPosition + vector * paramTowardsTarget, normalized.normalized, Color.green);
			Debug.DrawRay(PlayerCharacter.WorldPosition, normalized2 * 8f, Color.blue);
			float num = (useParamDistance ? (vector.magnitude * paramDistance) : distance);
			base.transform.position = PlayerCharacter.WorldPosition + normalized2 * num + vector * paramTowardsTarget + altitudeOffset * Vector3.up;
			Debug.DrawRay(base.transform.position, Vector3.up, Color.green);
		}
		else
		{
			base.transform.position = initialPosition;
		}
	}
}
