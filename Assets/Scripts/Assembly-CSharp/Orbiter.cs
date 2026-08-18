using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
	public float TankCollisionOrbitRadius = 1.5f;

	public float TankCollisionRotationSpeed = 1f;

	public Trail Trail;

	private TankController _tankBeingController;

	private Vector3 _pos;

	private void Start()
	{
		_pos = Vector3.zero;
	}

	private void Update()
	{
		bool flag = false;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		TankController tankController = null;
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f))
		{
			tankController = hitInfo.collider.transform.root.GetComponent<TankController>();
			if (tankController == null)
			{
				_pos = hitInfo.point;
			}
			else
			{
				flag = true;
				_pos = tankController.transform.position;
			}
		}
		if (!flag)
		{
			Trail.EmitAndNotSuppressEmit = false;
			return;
		}
		if (_tankBeingController != tankController)
		{
			Trail.EmitAndNotSuppressEmit = true;
			base.transform.localScale = Vector3.one * TankCollisionOrbitRadius;
			base.transform.Rotate(Vector3.up, TankCollisionRotationSpeed * Time.deltaTime);
			base.transform.position = _pos;
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (_tankBeingController != null)
			{
				_tankBeingController.InControl = false;
			}
			tankController.InControl = true;
			_tankBeingController = tankController;
		}
	}
}
