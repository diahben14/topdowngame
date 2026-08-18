using UnityEngine;

public class Reticule : MonoBehaviour
{
	[SerializeField]
	protected float normalRadius = 2f;

	[SerializeField]
	protected float degreesPerSecond = 360f;

	[SerializeField]
	protected Renderer[] renderers;

	[SerializeField]
	protected Transform[] rotatingBits;

	[SerializeField]
	protected Transform circle;

	[SerializeField]
	protected float smoothTime = 0.1f;

	[SerializeField]
	protected bool preview;

	protected Vector3 velocity;

	protected bool _visible = true;

	protected bool visible
	{
		get
		{
			return _visible;
		}
		set
		{
			if (_visible != value)
			{
				_visible = value;
				for (int i = 0; i < renderers.Length; i++)
				{
					renderers[i].enabled = value;
				}
			}
		}
	}

	private void LateUpdate()
	{
		if (Cheats.GUIHidden)
		{
			visible = false;
			return;
		}
		Vector3? vector = (preview ? PlayerCharacter.PreviewZTargetWorldPositionOrNull : PlayerCharacter.ZTargetWorldPositionOrNull);
		if (vector.HasValue)
		{
			if (preview && PlayerCharacter.ZTargetWorldPositionOrNull == vector)
			{
				visible = false;
			}
			else
			{
				visible = true;
			}
			Vector3 position = Vector3.SmoothDamp(base.transform.position, vector.Value, ref velocity, smoothTime, float.PositiveInfinity);
			base.transform.position = position;
			base.transform.rotation = Quaternion.identity;
			for (int i = 0; i < rotatingBits.Length; i++)
			{
				rotatingBits[i].localPosition = Quaternion.AngleAxis(degreesPerSecond * Time.time + (float)(360 / rotatingBits.Length * i), CameraController.MainCameraForward) * CameraController.MainCameraRight * normalRadius;
			}
			circle.rotation = Quaternion.LookRotation(-CameraController.MainCameraForward);
		}
		else
		{
			visible = false;
			base.transform.position = PlayerCharacter.WorldPosition;
		}
	}
}
