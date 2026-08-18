using System.Collections.Generic;
using UnityEngine;

public class ZTarget : MonoBehaviour
{
	[SerializeField]
	protected Transform targetPoint;

	[SerializeField]
	protected bool isActive = true;

	[SerializeField]
	protected float minDistanceToTarget;

	[SerializeField]
	protected bool penalizeAltitudeDifference = true;

	[SerializeField]
	protected float altitudeBonusDirection;

	[SerializeField]
	protected bool penalizeIfNoLOS;

	[SerializeField]
	protected float scoreMultiplier = 1f;

	[SerializeField]
	protected bool useAnimatorParameter;

	[SerializeField]
	protected string animatorParameter = "AP_showZTarget";

	[SerializeField]
	protected bool hideHPBar;

	protected static readonly float kDistanceToDeprioritizeFrozenEnemies = 8f;

	protected static readonly float kRaycastBuffer = 2f;

	protected static ZTarget cachedBestTargetForGizmos;

	protected Animator cachedAnimator;

	protected float cachedScoreForGizmo;

	protected bool isGrappleTarget;

	protected static List<ZTarget> targets = new List<ZTarget>();

	protected Creature creature;

	public Vector3 WorldPosition
	{
		get
		{
			if (targetPoint != null)
			{
				return targetPoint.position;
			}
			return base.transform.position;
		}
	}

	public DungeonRoomRigidbodyTracker RoomTracker { get; set; }

	public bool IsActive
	{
		get
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return false;
			}
			if (!isActive)
			{
				return false;
			}
			if (useAnimatorParameter)
			{
				return cachedAnimator.GetFloat(animatorParameter) > 0f;
			}
			return true;
		}
		set
		{
			isActive = value;
		}
	}

	public float? NormalizedHealth
	{
		get
		{
			if ((bool)creature)
			{
				return creature.NormalizeHP;
			}
			return null;
		}
	}

	public float? NormalizedPoise
	{
		get
		{
			if ((bool)creature)
			{
				return creature.NormalizedPoise;
			}
			return null;
		}
	}

	public bool HideHPBar
	{
		get
		{
			return hideHPBar;
		}
	}

	public static ZTarget GetTargetFromPositionAndDirection(Vector3 position, Vector3 direction, float maxDistance)
	{
		return GetTargetFromPositionAndDirection(position, direction, maxDistance, null, false);
	}

	public static ZTarget GetTargetFromPositionAndDirection(Vector3 position, Vector3 direction, float maxDistance, ZTarget exclude, bool simple)
	{
		float num = float.PositiveInfinity;
		ZTarget result = null;
		foreach (ZTarget target in targets)
		{
			if (!target.IsActive || (!(target.RoomTracker == null) && !target.RoomTracker.InCurrentRoom) || (!(exclude == null) && !(exclude != target)) || (target.isGrappleTarget && !(target.creature != null)))
			{
				continue;
			}
			Vector3 toTarget = target.WorldPosition - position;
			if (toTarget.magnitude <= maxDistance && toTarget.magnitude > target.minDistanceToTarget)
			{
				float num2 = calculateScore(direction, simple, target, toTarget, position);
				if (num2 < num)
				{
					num = num2;
					result = target;
				}
			}
		}
		cachedBestTargetForGizmos = result;
		return result;
	}

	private static float calculateScore(Vector3 lookDirection, bool simple, ZTarget target, Vector3 toTarget, Vector3 position)
	{
		float num = Vector3.Dot(lookDirection, toTarget.normalized);
		float num2 = float.PositiveInfinity;
		if (!simple)
		{
			num2 = toTarget.magnitude / 2f * (2f - num);
			if (target.penalizeAltitudeDifference)
			{
				float num3 = Mathf.Abs((target.transform.position - position).y);
				num2 *= 1f + Mathf.Log(num3 + 1f);
			}
			if (target.creature != null && target.creature.Frozen && toTarget.magnitude > kDistanceToDeprioritizeFrozenEnemies)
			{
				num2 *= 4f;
			}
			if (target.penalizeIfNoLOS && Physics.Raycast(position + Vector3.up, toTarget, toTarget.magnitude - kRaycastBuffer, 1, QueryTriggerInteraction.Ignore))
			{
				Debug.DrawRay(position, toTarget, Color.red, 0.1f);
				num2 *= 4f;
			}
			if (target.altitudeBonusDirection != 0f && Mathf.Sign(toTarget.y) == Mathf.Sign(target.altitudeBonusDirection))
			{
				num2 *= 0.5f;
			}
			num2 *= target.scoreMultiplier;
		}
		else
		{
			num2 = 1f / toTarget.magnitude * (0f - num);
		}
		target.cachedScoreForGizmo = num2;
		return num2;
	}

	private void Awake()
	{
		targets.Add(this);
		creature = GetComponent<Creature>();
		if (useAnimatorParameter)
		{
			cachedAnimator = GetComponent<Animator>();
		}
		TetherTarget component = GetComponent<TetherTarget>();
		if ((bool)component && component.IsGrapplePoint)
		{
			isGrappleTarget = true;
		}
	}

	private void OnDestroy()
	{
		targets.Remove(this);
	}
}
