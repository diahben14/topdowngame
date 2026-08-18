using UnityEngine;

public class Helpers : MonoBehaviour
{
	public static void DrawGizmoArrow(Vector3 from, Vector3 to, Vector3 up, float headLength, float arrowWidth)
	{
		Vector3 vector = to - from;
		Vector3 vector2 = Vector3.Cross(vector, up.normalized);
		Gizmos.DrawRay(from + vector2 * arrowWidth / 2f, vector * (vector.magnitude - arrowWidth / 2f));
		Gizmos.DrawRay(from - vector2 * arrowWidth / 2f, vector * (vector.magnitude - arrowWidth / 2f));
		Gizmos.DrawLine(to, to + (vector2.normalized - vector.normalized) * Mathf.Min(headLength, arrowWidth));
		Gizmos.DrawLine(to, to + (-vector2.normalized - vector.normalized) * Mathf.Min(headLength, arrowWidth));
	}
}
