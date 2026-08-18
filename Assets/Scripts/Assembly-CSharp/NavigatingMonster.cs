using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavigatingMonster : Monster
{
	[Header("Locomoting Monster")]
	[SerializeField]
	protected float navigationFloatBlendTime = 0.33f;

	[SerializeField]
	protected float turnFloatBlendTime = 0.1f;

	[SerializeField]
	protected float navigationDeadzone = 0.1f;

	[SerializeField]
	protected float freeTurnToFaceSpeed = 180f;

	[SerializeField]
	protected float maxFreeMoveToSpeed = 1f;

	[Header("Locomoting Monster - Animatable")]
	[SerializeField]
	protected float freeTurnToFaceFactor;

	[SerializeField]
	protected float freeMoveToFactor;

	[SerializeField]
	protected bool useParameterTurnToFace;

	[SerializeField]
	protected string turnToFaceParameterName = "turnToFace";

	[SerializeField]
	protected string freeMoveParameterName = "freeMove";

	[SerializeField]
	protected Transform turnToFacePivotTransform;

	protected float turnToFaceVelocity;

	protected Vector3 _locomotionTarget;

	protected Vector3 lastSetDestTarget;

	protected float lastSetDestTime;

	protected const float kForceSetDestinationInterval = 1f;

	protected const float kForceSetDestinationMinDistanceSqr = 1f;

	protected float cachedForceSetDestDistance;

	protected bool forceSetDestDistanceCached;

	protected bool distanceParameterExists;

	protected bool dotParameterExists;

	protected bool linearDotParameterExists;

	protected bool freeMoveParameterExists;

	protected Vector3 locomotionTarget
	{
		get
		{
			return _locomotionTarget;
		}
		set
		{
			_locomotionTarget = value;
		}
	}

	protected Vector3 facingTarget { get; set; }

	protected Vector3 turnToFacePivot
	{
		get
		{
			if (!(turnToFacePivotTransform == null))
			{
				return turnToFacePivotTransform.position;
			}
			return base.transform.position;
		}
	}

	protected Vector3 desiredDirection
	{
		get
		{
			return nma.desiredVelocity.normalized;
		}
	}

	protected float rawTurnParam { get; private set; }

	protected bool hasPath
	{
		get
		{
			return nma.hasPath;
		}
	}

	protected virtual bool updateNavigationParametersEachFrame
	{
		get
		{
			return true;
		}
	}

	protected bool SuppressNavWhenPlaying
	{
		get
		{
			for (int i = 0; i < stateBehaviours.Length; i++)
			{
				if (stateBehaviours[i].SuppressNavWhenPlaying)
				{
					return true;
				}
			}
			return false;
		}
	}

	protected float forceSetDestDistanceSqr
	{
		get
		{
			if (!forceSetDestDistanceCached)
			{
				cachedForceSetDestDistance = Mathf.Max(1f, navigationDeadzone * navigationDeadzone);
				forceSetDestDistanceCached = true;
			}
			return cachedForceSetDestDistance;
		}
	}

	protected virtual bool distanceParameterIsDistanceToFacingTarget
	{
		get
		{
			return false;
		}
	}

	protected virtual void LocomotingBehaviourUpdate()
	{
		if (PlayerCharacter.Instanced)
		{
			locomotionTarget = base.playerOrBaitWorldPosition;
		}
		facingTarget = base.transform.position + nma.desiredVelocity;
	}

	protected override void BehaviourUpdate()
	{
		LocomotingBehaviourUpdate();
		if (updateNavigationParametersEachFrame)
		{
			updateNavigationParameters();
		}
	}

	protected void SetDestinationOptimized(Vector3 locomotionTarget)
	{
		if ((locomotionTarget - lastSetDestTarget).sqrMagnitude > forceSetDestDistanceSqr || Time.time > lastSetDestTime + 1f)
		{
			nma.SetDestination(locomotionTarget);
			lastSetDestTime = Time.time;
			lastSetDestTarget = locomotionTarget;
		}
	}

	protected void updateNavigationParameters()
	{
		if (!SuppressNavWhenPlaying)
		{
			Vector3 vector = facingTarget - base.transform.position;
			float num;
			for (num = Geometry.Vector2Angle(base.transform.InverseTransformDirection(vector.normalized)); num > 180f; num -= 360f)
			{
			}
			for (; num < -180f; num += 360f)
			{
			}
			float value = (0f - num) / 90f;
			cachedAnimator.SetFloat("turn", value, turnFloatBlendTime, Time.deltaTime);
			rawTurnParam = value;
			Vector3 vector2 = locomotionTarget - base.transform.position;
			if (nma.isOnNavMesh)
			{
				SetDestinationOptimized(locomotionTarget);
			}
			Vector3 obj = ((vector2.magnitude < navigationDeadzone) ? Vector3.zero : base.transform.InverseTransformDirection(nma.desiredVelocity.normalized));
			float z = obj.z;
			float x = obj.x;
			cachedAnimator.SetFloat("forward", z, navigationFloatBlendTime, Time.deltaTime);
			cachedAnimator.SetFloat("right", x, navigationFloatBlendTime, Time.deltaTime);
			setSupplementalParameters();
		}
	}

	protected void checkForExistanceOfSupplementalParameters()
	{
		AnimatorControllerParameter[] parameters = cachedAnimator.parameters;
		distanceParameterExists = false;
		dotParameterExists = false;
		linearDotParameterExists = false;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].name == "distance")
			{
				distanceParameterExists = true;
			}
			else if (parameters[i].name == "dot")
			{
				dotParameterExists = true;
			}
			else if (parameters[i].name == "linearDot")
			{
				linearDotParameterExists = true;
			}
			else if (parameters[i].name == freeMoveParameterName)
			{
				freeMoveParameterExists = true;
			}
		}
	}

	protected void setSupplementalParameters()
	{
		if (distanceParameterExists)
		{
			Vector3 vector = ((!distanceParameterIsDistanceToFacingTarget) ? locomotionTarget : facingTarget);
			float value = Vector3.Magnitude(vector - base.transform.position);
			cachedAnimator.SetFloat("distance", value);
		}
		if (dotParameterExists)
		{
			float value2 = Vector3.Dot(base.transform.forward, (facingTarget - base.transform.position).normalized);
			cachedAnimator.SetFloat("dot", value2);
		}
		if (linearDotParameterExists)
		{
			cachedAnimator.SetFloat("linearDot", getLinearDot());
		}
	}

	protected float getLinearDot()
	{
		float f = Vector3.Dot(base.transform.forward, (facingTarget - base.transform.position).normalized);
		return 1f - 2f * Mathf.Acos(f) / (float)Math.PI;
	}

	protected override void creature_Awake()
	{
		base.creature_Awake();
		nma = GetComponent<NavMeshAgent>();
		nma.updatePosition = false;
		nma.updateRotation = false;
		checkForExistanceOfSupplementalParameters();
	}

	protected override void creature_OnAnimatorMove()
	{
		if (base.SleepingDueToPlayerDistance)
		{
			return;
		}
		base.creature_OnAnimatorMove();
		if (Time.deltaTime == 0f || float.IsNaN(cachedAnimator.deltaPosition.x))
		{
			return;
		}
		cachedRigidbody.velocity += cachedAnimator.deltaPosition / Time.deltaTime;
		nma.nextPosition = base.transform.position;
		if (Vector3.Distance(nma.nextPosition, base.transform.position) > 1f)
		{
			nma.Warp(base.transform.position);
		}
		float angle;
		Vector3 axis;
		cachedAnimator.deltaRotation.ToAngleAxis(out angle, out axis);
		base.transform.RotateAround(base.transform.position, axis, angle);
		cachedRigidbody.angularVelocity = Vector3.zero;
		float num = freeTurnToFaceSpeed * (useParameterTurnToFace ? cachedAnimator.GetFloat(turnToFaceParameterName) : freeTurnToFaceFactor);
		if (num != 0f && !base.Frozen)
		{
			Vector3 flatToFacingTarget = (facingTarget - base.transform.position).Flatten();
			if (flatToFacingTarget.sqrMagnitude > 0f)
			{
				Mathx.ApplySmoothTurnToFace(base.transform, base.transform.rotation.eulerAngles.y, flatToFacingTarget, num, ref turnToFaceVelocity, turnToFacePivot);
			}
			else
			{
				turnToFaceVelocity = 0f;
			}
		}
		else
		{
			turnToFaceVelocity = 0f;
		}
		float num2 = ((useParameterTurnToFace && freeMoveParameterExists) ? cachedAnimator.GetFloat(freeMoveParameterName) : freeMoveToFactor);
		if (base.Frozen)
		{
			num2 = 0f;
		}
		if (maxFreeMoveToSpeed != 0f && num2 != 0f)
		{
			cachedRigidbody.velocity += (_locomotionTarget - base.transform.position).normalized * maxFreeMoveToSpeed * num2;
		}
		monster_PostOnAnimatorMove();
	}

	protected virtual void monster_PostOnAnimatorMove()
	{
	}
}
