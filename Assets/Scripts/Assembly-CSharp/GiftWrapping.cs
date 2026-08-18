using System;
using System.Collections.Generic;
using UnityEngine;

public class GiftWrapping
{
	public static List<Vector3> ConvexHull(List<Vector3> vertices)
	{
		if (vertices.Count < 3)
		{
			throw new ArgumentException("At least 3 points reqired", "points");
		}
		List<Vector3> list = new List<Vector3>();
		Vector3 vector = vertices[0];
		foreach (Vector3 vertex in vertices)
		{
			if (vertex.x < vector.x)
			{
				vector = vertex;
			}
		}
		Vector3 vector2;
		do
		{
			list.Add(vector);
			vector2 = vertices[0];
			for (int i = 1; i < vertices.Count; i++)
			{
				if (vector == vector2 || Orientation(vector, vector2, vertices[i]) == -1)
				{
					vector2 = vertices[i];
				}
			}
			vector = vector2;
		}
		while (vector2 != list[0]);
		return list;
	}

	private static int Orientation(Vector3 p1, Vector3 p2, Vector3 p)
	{
		float num = (p2.x - p1.x) * (p.z - p1.z) - (p.x - p1.x) * (p2.z - p1.z);
		if (num > 0f)
		{
			return -1;
		}
		if (num < 0f)
		{
			return 1;
		}
		return 0;
	}
}
