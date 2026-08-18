using System;
using System.Collections;
using UnityEngine;

public static class Mathx
{
	internal delegate bool TestDelegate();

	public static readonly Vector3 flatteningVector = new Vector3(1f, 0f, 1f);

	public static float Poisson(float average)
	{
		return (0f - Mathf.Log(1f - UnityEngine.Random.value)) * average;
	}

	public static float xAngleBetween(Vector3 from, Vector3 to)
	{
		Vector3 vector = from - to;
		float y = vector.y;
		float num = Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z);
		return Mathf.Atan(y / num) * 57.29578f;
	}

	public static void ApplySmoothTurnToFace(Transform transformToRotate, float currentYAngle, Vector3 flatToFacingTarget, float turnToFaceSpeed, ref float turnToFaceVelocity, Vector3 pivot)
	{
		float angle = CalculateSmoothTurnToFaceAngle(currentYAngle, flatToFacingTarget, turnToFaceSpeed, ref turnToFaceVelocity) - currentYAngle;
		transformToRotate.RotateAround(pivot, Vector3.up, angle);
	}

	public static Quaternion CalculateSmoothTurnToFaceQuaternion(float currentYAngle, Vector3 flatToFacingTarget, float turnToFaceSpeed, ref float turnToFaceVelocity)
	{
		float y = CalculateSmoothTurnToFaceAngle(currentYAngle, flatToFacingTarget, turnToFaceSpeed, ref turnToFaceVelocity);
		return Quaternion.Euler(0f, y, 0f);
	}

	public static float CalculateSmoothTurnToFaceAngle(float currentYAngle, Vector3 flatToFacingTarget, float turnToFaceSpeed, ref float turnToFaceVelocity)
	{
		float target = Vector3.SignedAngle(Vector3.forward, flatToFacingTarget, Vector3.up);
		Mathf.DeltaAngle(currentYAngle, target);
		float smoothTime = 1f / turnToFaceSpeed * 30f;
		return Mathf.SmoothDampAngle(currentYAngle, target, ref turnToFaceVelocity, smoothTime, turnToFaceSpeed * 3f);
	}

	internal static float Chebyshev(Vector3 position1, Vector3 position2)
	{
		Vector3 vector = position2 - position1;
		return Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
	}

	internal static Vector3 CalculateTossVelocity(Vector3 origin, Vector3 target, float angle)
	{
		Vector3 vector = target.Flatten() - origin.Flatten();
		float magnitude = vector.magnitude;
		float num = 0f - Physics.gravity.y;
		float num2 = Mathf.Sin(2f * angle * ((float)Math.PI / 180f));
		float num3 = Mathf.Sqrt(magnitude * num / num2);
		return (Quaternion.AngleAxis(angle, Vector3.Cross(vector, Vector3.up)) * vector).normalized * num3;
	}

	internal static Vector3 Octoliniearize(Vector3 dir)
	{
		dir = dir.Flatten().normalized;
		float num = Mathf.Round(Vector3.Angle(Vector3.forward, dir) / 45f) * 45f;
		if (Vector3.Cross(Vector3.forward, dir).y < 0f)
		{
			num *= -1f;
		}
		return Quaternion.Euler(0f, num, 0f) * Vector3.forward;
	}

	internal static void Choose(params Action[] paramList)
	{
		paramList[UnityEngine.Random.Range(0, paramList.Length)]();
	}

	internal static T RandomElement<T>(this T[] array)
	{
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	internal static IEnumerator WaitForTrue(TestDelegate fn)
	{
		while (!fn())
		{
			yield return 0;
		}
	}
}
