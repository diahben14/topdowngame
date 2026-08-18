using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class Extensions
{
	public static Vector3 Flatten(this Vector3 v)
	{
		return new Vector3(v.x, 0f, v.z);
	}

	public static Vector3 Clamp1(this Vector3 v)
	{
		if (v.magnitude < 1f)
		{
			return v;
		}
		return v.normalized;
	}

	public static int clockMod(this int i, int len)
	{
		int num = i % len;
		if (num < 0)
		{
			return num + len;
		}
		return num;
	}

	public static Vector3 Orthoganalize(this Vector3 v)
	{
		if (v.sqrMagnitude == 0f)
		{
			return v;
		}
		if (Mathf.Abs(v.x) > Mathf.Abs(v.z))
		{
			if (!(v.x > 0f))
			{
				return Vector3.left;
			}
			return Vector3.right;
		}
		if (!(v.z > 0f))
		{
			return Vector3.back;
		}
		return Vector3.forward;
	}

	public static bool IsApproxZero(this float f)
	{
		return Mathf.Approximately(f, 0f);
	}

	public static bool IsApprox(this float f, float ff)
	{
		return Mathf.Approximately(f, ff);
	}

	public static void SetHeight(this RectTransform rt, float height)
	{
		Vector2 sizeDelta = rt.sizeDelta;
		sizeDelta.y = height;
		rt.sizeDelta = sizeDelta;
	}

	public static void Shuffle<T>(this List<T> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = UnityEngine.Random.Range(0, num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	public static string ColorToHex(this Color32 color)
	{
		return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
	}

	public static Color? HexToColor(this string hex)
	{
		hex = hex.Replace("0x", "");
		hex = hex.Replace("#", "");
		byte a = byte.MaxValue;
		try
		{
			byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			if (hex.Length == 8)
			{
				a = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			}
			return new Color32(r, g, b, a);
		}
		catch (Exception)
		{
			Debug.LogErrorFormat("Couldn't parse {0} into a colour", hex);
			return null;
		}
	}
}
