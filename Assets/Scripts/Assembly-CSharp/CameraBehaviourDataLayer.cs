using System;
using UnityEngine;

[Serializable]
public class CameraBehaviourDataLayer
{
	public int priority;

	public bool conduitOnly;

	public bool override_orthographicSize;

	public float orthographicSize = 10f;

	public bool override_normalCameraEulerX;

	public float normalCameraEulerX = 35.264f;

	public bool override_eulerY;

	public float eulerY = 45f;

	public bool override_zTargetCameraEulerX;

	public float zTargetCameraEulerX = 55f;

	public bool override_orthographicSizeSmoothTime;

	public float orthographicSizeSmoothTime = 1f;

	public bool override_positionSmoothTime;

	public float positionSmoothTime = 0.5f;

	public bool override_xRotationOverrideSmoothTime;

	public float xRotationOverrideSmoothTime = 3f;

	public bool override_eulerYSmoothTime;

	public float eulerYSmoothTime = 0.5f;

	public bool override_xRotationOverrideMaxSpeed;

	public float xRotationOverrideMaxSpeed = 45f;

	public bool override_eulerYMaxSmoothSpeed;

	public float eulerYMaxSmoothSpeed = 360f;

	public bool override_maxZTargetDistance;

	public float maxZTargetDistance = 18f;

	public bool override_moveCameraToZTargetParameter;

	public float moveCameraToZTargetParameter = 0.5f;

	public bool override_orthographicNearClipPlane;

	public float orthographicNearClipPlane = 1f;

	public bool override_perspectiveFarClipPlane;

	public float perspectiveFarClipPlane = 500f;

	public bool override_perspectiveFOV;

	public float perspectiveFOV = 10f;

	public bool override_cameraDistance;

	public float cameraDistance = 500f;

	public bool override_orthographicDepth;

	public float orthographicDepth = 100f;

	public bool override_smoothCamera;

	public bool smoothCamera = true;

	public bool override_zTargettingPositionSmoothingTime;

	public float zTargettingPositionSmoothingTime = 0.25f;

	public bool override_zTargettingZoomAmount;

	public float zTargettingZoomAmount = 1f;

	public bool override_parameterizeZTargettingZoomByDistance;

	public bool parameterizeZTargettingZoomByDistance;

	public bool override_eulerYTarget;

	public Transform eulerYTarget;

	public bool override_faceAwayFromEulerYTarget;

	public bool faceAwayFromEulerYTarget;
}
