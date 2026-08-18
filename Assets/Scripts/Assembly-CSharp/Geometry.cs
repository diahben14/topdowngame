using UnityEngine;

public class Geometry
{
	public static float Vector2Angle(Vector3 v)
	{
		return Mathf.Atan2(v.z, v.x) * 57.29578f - 90f;
	}

	public static Quaternion Angle2Quaternion(float a)
	{
		return Quaternion.Euler(0f, 0f - a, 0f);
	}

	public static Quaternion Vector2Quaternion(Vector3 v)
	{
		return Angle2Quaternion(Vector2Angle(v));
	}
}
