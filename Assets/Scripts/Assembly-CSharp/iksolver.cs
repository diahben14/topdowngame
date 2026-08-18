using UnityEngine;

public class iksolver : MonoBehaviour
{
	[SerializeField]
	protected Transform testingTarget;

	protected Vector3 _targetPosition;

	public GameObject[] joints;

	protected float[] theta;

	protected float[] sin;

	protected float[] cos;

	[SerializeField]
	private int iterations = 8;

	public Vector3 TargetPosition
	{
		get
		{
			if (testingTarget != null)
			{
				return testingTarget.position;
			}
			return _targetPosition;
		}
		set
		{
			_targetPosition = value;
		}
	}

	public float strength { get; set; }

	public Vector3 EndWorldPosition
	{
		get
		{
			return joints[joints.Length - 1].transform.position;
		}
	}

	private void Start()
	{
		theta = new float[joints.Length];
		sin = new float[joints.Length];
		cos = new float[joints.Length];
	}

	private void LateUpdate()
	{
		Debug.DrawLine(base.transform.position, TargetPosition, Color.gray);
		for (int i = 0; i < joints.Length - 1; i++)
		{
			Debug.DrawLine(joints[i].transform.position, joints[i + 1].transform.position, Color.white);
		}
		for (int j = 0; j < iterations; j++)
		{
			for (int num = joints.Length - 2; num >= 0; num--)
			{
				Vector3 lhs = joints[joints.Length - 1].transform.position - joints[num].transform.position;
				Vector3 rhs = TargetPosition - joints[num].transform.position;
				if (lhs.magnitude * rhs.magnitude <= 0.001f)
				{
					cos[num] = 1f;
					sin[num] = 0f;
				}
				else
				{
					cos[num] = Vector3.Dot(lhs, rhs) / (lhs.magnitude * rhs.magnitude);
					sin[num] = Vector3.Cross(lhs, rhs).magnitude / (lhs.magnitude * rhs.magnitude);
				}
				Vector3 axis = Vector3.Cross(lhs, rhs) / (lhs.magnitude * rhs.magnitude);
				theta[num] = Mathf.Acos(Mathf.Max(-1f, Mathf.Min(1f, cos[num])));
				if (sin[num] < 0f)
				{
					theta[num] = 0f - theta[num];
				}
				theta[num] = (float)SimpleAngle(theta[num]) * 57.29578f;
				joints[num].transform.Rotate(axis, theta[num] * (strength * strength * strength), Space.World);
			}
		}
	}

	private double SimpleAngle(double theta)
	{
		theta %= 6.2831854820251465;
		if (theta < -3.1415927410125732)
		{
			theta += 6.2831854820251465;
		}
		else if (theta > 3.1415927410125732)
		{
			theta -= 6.2831854820251465;
		}
		return theta;
	}
}
