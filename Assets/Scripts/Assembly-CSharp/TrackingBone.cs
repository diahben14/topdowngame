using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TrackingBone : MonoBehaviour
{
	protected bool _trackingOn = true;

	protected Vector3 _trackingTarget;

	[Tooltip("Optional animator controlling tracking with trackParameterName")]
	[SerializeField]
	protected Animator controllingAnimator;

	[SerializeField]
	protected string trackParameterName = "AP_headtrack";

	[Tooltip("Turn off this transform isn't controlled by an Animator")]
	[SerializeField]
	protected bool isControlledByAnimation = true;

	[Tooltip("The smoothdamp time to turn on (and off) tracking")]
	[SerializeField]
	protected float trackingTime = 1f;

	[Tooltip("If false, unTrackingTime is ignored and trackingTime is used to turn off tracking")]
	[SerializeField]
	protected bool useSeparateUnTrackTime;

	[SerializeField]
	protected float unTrackingTime = 0.01f;

	[Tooltip("Maximum degree/sec the bone will track the target")]
	[SerializeField]
	protected float maxDegreesDelta = 180f;

	[Tooltip("How far will the bone turn to track target")]
	[SerializeField]
	protected float maxDeflectionDegrees = 60f;

	[Tooltip("When overdeflected, should tracking turn off?")]
	[SerializeField]
	protected bool disableTrackingWhenOverdeflected = true;

	[SerializeField]
	protected bool playerIsAlwaysTrackingTarget;

	[SerializeField]
	protected Transform testingTarget;

	[FormerlySerializedAs("headTrackForwardHint")]
	[SerializeField]
	protected Transform forwardOverride;

	[SerializeField]
	protected Transform upOverride;

	protected Quaternion initialLocalRotation;

	protected Quaternion trackedRotation;

	protected Vector3 toForwardHintLocal;

	protected float smoothedTrackParameter;

	protected float smoothedTrackParameterVelocity;

	protected List<TrackingBone> childrenTrackingBones;

	protected bool isChild;

	protected bool debug_overdeflected;

	protected bool debug_showAsAnimatorDisabled;

	protected Creature cachedCreature;

	public Vector3 TrackingTarget
	{
		get
		{
			if (testingTarget != null)
			{
				return testingTarget.position;
			}
			if (!playerIsAlwaysTrackingTarget || !PlayerCharacter.Instanced)
			{
				return _trackingTarget;
			}
			return PlayerCharacter.WorldPosition;
		}
		set
		{
			_trackingTarget = value;
		}
	}

	public bool TrackingOn
	{
		get
		{
			if (_trackingOn)
			{
				return base.enabled;
			}
			return false;
		}
		set
		{
			_trackingOn = value;
		}
	}

	protected void OnDrawGizmos()
	{
		Gizmos.color = (debug_showAsAnimatorDisabled ? Color.gray : (debug_overdeflected ? Color.red : Color.cyan));
		Vector3 position = base.transform.position;
		Vector3 to = base.transform.position + base.transform.TransformDirection(localPointingVector()).normalized;
		Vector3 up = (upOverride ? (upOverride.position - base.transform.position) : Vector3.up);
		Helpers.DrawGizmoArrow(position, to, up, 0.5f, 0.2f);
		Gizmos.DrawSphere(base.transform.position, 0.25f);
	}

	protected Vector3 localPointingVector()
	{
		if ((bool)forwardOverride)
		{
			Vector3 direction = forwardOverride.position - base.transform.position;
			return base.transform.InverseTransformDirection(direction).normalized;
		}
		return Vector3.forward;
	}

	protected void Awake()
	{
		if (testingTarget != null)
		{
			Debug.LogWarning("---------- TrackingBone is using testing transform----------");
		}
		initialLocalRotation = base.transform.localRotation;
		childrenTrackingBones = new List<TrackingBone>();
		collectChildTrackingBones(base.transform);
		cachedCreature = GetComponentInParent<Creature>();
	}

	protected void collectChildTrackingBones(Transform t)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			TrackingBone component = child.GetComponent<TrackingBone>();
			if (component == null)
			{
				collectChildTrackingBones(child);
				continue;
			}
			component.isChild = true;
			childrenTrackingBones.Add(component);
		}
	}

	protected void LateUpdate()
	{
		if (!isChild)
		{
			doTracking();
		}
	}

	protected void doTracking()
	{
		if (!(cachedCreature == null) && cachedCreature.Frozen)
		{
			return;
		}
		if (!isControlledByAnimation)
		{
			base.transform.localRotation = initialLocalRotation;
		}
		Quaternion rotation = base.transform.rotation;
		Quaternion lookatRotation = getLookatRotation();
		bool flag = TrackingOn;
		debug_overdeflected = false;
		if (base.transform.parent == null)
		{
			Debug.LogError("???");
		}
		Quaternion quaternion = base.transform.parent.rotation * initialLocalRotation;
		trackedRotation = Quaternion.RotateTowards(to: clampQuaternion(lookatRotation, quaternion, maxDeflectionDegrees), from: trackedRotation, maxDegreesDelta: maxDegreesDelta * Time.deltaTime);
		debug_showAsAnimatorDisabled = false;
		float num = maxDeflectionDegrees;
		if (controllingAnimator != null && TrackingOn)
		{
			float @float = controllingAnimator.GetFloat(trackParameterName);
			num *= Mathf.Clamp01(@float);
			if (@float > 0f)
			{
				flag = true;
			}
			else
			{
				flag = false;
				debug_showAsAnimatorDisabled = true;
			}
		}
		if (TwistlessAngle(lookatRotation, quaternion, localPointingVector()) > num)
		{
			if (disableTrackingWhenOverdeflected)
			{
				flag = false;
			}
			debug_overdeflected = true;
		}
		trackedRotation = clampQuaternion(trackedRotation, quaternion, num);
		smoothedTrackParameter = Mathf.SmoothDamp(smoothTime: (useSeparateUnTrackTime && !flag) ? unTrackingTime : trackingTime, current: smoothedTrackParameter, target: flag ? 1 : 0, currentVelocity: ref smoothedTrackParameterVelocity, maxSpeed: float.PositiveInfinity);
		Quaternion rotation2 = Quaternion.Slerp(rotation, trackedRotation, smoothedTrackParameter);
		base.transform.rotation = rotation2;
		for (int i = 0; i < childrenTrackingBones.Count; i++)
		{
			childrenTrackingBones[i].doTracking();
		}
	}

	protected Quaternion getLookatRotation()
	{
		Vector3 forward = TrackingTarget - base.transform.position;
		Quaternion rotation = Quaternion.FromToRotation(toDirection: localPointingVector(), fromDirection: Vector3.forward);
		Vector3 upwards = (upOverride ? (upOverride.position - base.transform.position) : Vector3.up);
		return Quaternion.LookRotation(forward, upwards) * Quaternion.Inverse(rotation);
	}

	protected float TwistlessAngle(Quaternion a, Quaternion b, Vector3 twistAxis)
	{
		Vector3 from = a * twistAxis.normalized;
		Vector3 to = b * twistAxis.normalized;
		return Vector3.Angle(from, to);
	}

	protected bool overDeflectedCheck(Quaternion r)
	{
		Vector3 direction = r * Vector3.forward;
		Vector3 to = base.transform.parent.InverseTransformDirection(direction);
		return Vector3.Angle(initialLocalRotation * Vector3.forward, to) > maxDeflectionDegrees;
	}

	protected Quaternion clampQuaternion(Quaternion r, Quaternion reference, float maxDeflection)
	{
		if (TwistlessAngle(r, reference, localPointingVector()) <= maxDeflection)
		{
			return r;
		}
		return Quaternion.RotateTowards(reference, r, maxDeflection);
	}
}
