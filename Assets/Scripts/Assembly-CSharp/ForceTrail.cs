using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ForceTrail : MonoBehaviour
{
	[SerializeField]
	protected float minDistBetweenPoints = 0.5f;

	[SerializeField]
	protected Vector3 forceVector = Vector3.up;

	protected LineRenderer lr;

	protected Vector3[] points;

	private void Start()
	{
		lr = GetComponent<LineRenderer>();
		points = new Vector3[16];
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, lr.GetPosition(0)) > minDistBetweenPoints)
		{
			addNewPoint();
		}
		for (int i = 0; i < points.Length; i++)
		{
			points[i] += forceVector * (i + 1) * Time.deltaTime;
		}
		lr.positionCount = points.Length;
		lr.SetPositions(points);
	}

	protected void addNewPoint()
	{
		for (int num = points.Length - 1; num > 0; num--)
		{
			points[num] = points[num - 1];
		}
		points[0] = base.transform.position;
	}
}
