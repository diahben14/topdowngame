using UnityEngine;

public class Ladder : MonoBehaviour
{
	[SerializeField]
	protected float height = 4f;

	[SerializeField]
	protected float engageRadius = 1f;

	[SerializeField]
	protected Vector3 offset;

	[SerializeField]
	protected bool _animatable_topActive = true;

	[SerializeField]
	protected bool _animatable_bottomActive = true;

	protected const float climbingDistance = 0.75f;

	protected const float horizDeltaOfClimbOnAnimation = 2f;

	protected const float verticalDeltaOfClimbOnAnimation = 1.5f;

	protected const float gizmoRadius = 0.75f;

	protected LadderEnd[] ladderEnds = new LadderEnd[2];

	public float GetParamFromAltitude(float altitude)
	{
		return Mathf.InverseLerp(ladderEnds[0].transform.position.y, ladderEnds[1].transform.position.y, altitude);
	}

	public Vector3 GetPositionFromParam(float p)
	{
		return Vector3.Lerp(ladderEnds[0].transform.position, ladderEnds[1].transform.position, Mathf.Clamp01(p)) - base.transform.forward * 0.75f;
	}

	public float Normalize(float move)
	{
		return move / height;
	}

	public bool OkToGrabEnd(LadderEnd end)
	{
		if (end == ladderEnds[0] && _animatable_bottomActive)
		{
			return true;
		}
		if (end == ladderEnds[1] && _animatable_topActive)
		{
			return true;
		}
		return false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0.3f, 1f, 0.9f);
		Gizmos.DrawCube(base.transform.TransformVector(offset) + base.transform.position + Vector3.up * height / 2f, new Vector3(0.375f, height, 0.375f));
		Gizmos.DrawSphere(base.transform.TransformVector(offset) + base.transform.position, 0.75f);
		Gizmos.DrawSphere(base.transform.TransformVector(offset) + base.transform.position + Vector3.up * height + base.transform.forward * 1.25f, 0.75f);
	}

	private void Awake()
	{
		for (int i = 0; i < 2; i++)
		{
			GameObject gameObject = new GameObject((i == 0) ? "Ladder Bottom" : "Ladder Top");
			SphereCollider sphereCollider = gameObject.gameObject.AddComponent<SphereCollider>();
			sphereCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			sphereCollider.radius = engageRadius;
			sphereCollider.isTrigger = true;
			ladderEnds[i] = gameObject.AddComponent<LadderEnd>();
			ladderEnds[i].ParentLadder = this;
			ladderEnds[i].transform.position = base.transform.position + Vector3.up * height * i + base.transform.TransformVector(offset);
			ladderEnds[i].gameObject.AddComponent<InteractionTrigger>().m_dotProductAdjustment = 0.5f;
			gameObject.transform.parent = base.transform;
		}
	}

	public void ClimbOn(LadderEnd ladderEnd)
	{
		PlayerCharacter.ClimbOnLadder(this, (!(ladderEnd == ladderEnds[0])) ? 1 : 0, (ladderEnd == ladderEnds[0]) ? (base.transform.TransformVector(offset) + base.transform.position - base.transform.forward * 0.75f) : (base.transform.TransformVector(offset) + base.transform.position + Vector3.up * height + base.transform.forward * 1.25f), (ladderEnd == ladderEnds[0]) ? Quaternion.LookRotation(base.transform.forward) : Quaternion.LookRotation(-base.transform.forward));
	}

	public bool AtBottom(float ladderProgressParam)
	{
		return ladderProgressParam <= 0f;
	}

	public bool AtTop(float ladderProgressParam)
	{
		return ladderProgressParam * height >= height - 1.5f;
	}

	public void WarpOntoLadder(Vector3 position)
	{
		float paramFromAltitude = GetParamFromAltitude(position.y);
		PlayerCharacter.ClimbOnLadder(this, paramFromAltitude, GetPositionFromParam(paramFromAltitude), Quaternion.LookRotation(base.transform.forward));
	}
}
