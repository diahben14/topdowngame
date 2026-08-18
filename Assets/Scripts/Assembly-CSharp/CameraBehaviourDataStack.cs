using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CameraBehaviourDataStack
{
	protected List<CameraBehaviourDataLayer> stack;

	protected CameraBehaviourDataLayer flattened_normal;

	protected CameraBehaviourDataLayer flattened_conduit;

	public float orthographicSize
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).orthographicSize;
		}
	}

	public float orthographicSizeSmoothTime
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).orthographicSizeSmoothTime;
		}
	}

	public float positionSmoothTime
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).positionSmoothTime;
		}
	}

	public float normalCameraEulerX
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).normalCameraEulerX;
		}
	}

	public float zTargetCameraEulerX
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).zTargetCameraEulerX;
		}
	}

	public float eulerY
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).eulerY;
		}
	}

	public float eulerYSmoothTime
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).eulerYSmoothTime;
		}
	}

	public float eulerYMaxSmoothSpeed
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).eulerYMaxSmoothSpeed;
		}
	}

	public float maxZTargetDistance
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).maxZTargetDistance;
		}
	}

	public float moveCameraToZTargetParameter
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).moveCameraToZTargetParameter;
		}
	}

	public float orthographicNearClipPlane
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).orthographicNearClipPlane;
		}
	}

	public float perspectiveFarClipPlane
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).perspectiveFarClipPlane;
		}
	}

	public float perspectiveFOV
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).perspectiveFOV;
		}
	}

	public float cameraDistance
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).cameraDistance;
		}
	}

	public float orthographicDepth
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).orthographicDepth;
		}
	}

	public bool smoothCamera
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).smoothCamera;
		}
	}

	public float zTargettingPositionSmoothingTime
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).zTargettingPositionSmoothingTime;
		}
	}

	public float zTargettingZoomAmount
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).zTargettingZoomAmount;
		}
	}

	public bool parameterizeZTargettingZoomByDistance
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).parameterizeZTargettingZoomByDistance;
		}
	}

	public Transform eulerYTarget
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).eulerYTarget;
		}
	}

	public bool faceAwayFromEulerYTarget
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).faceAwayFromEulerYTarget;
		}
	}

	public float xRotationOverrideSmoothTime
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).xRotationOverrideSmoothTime;
		}
	}

	public float xRotationOverrideMaxSpeed
	{
		get
		{
			return (PlayerCharacter.InstanceInConduit ? flattened_conduit : flattened_normal).xRotationOverrideMaxSpeed;
		}
	}

	public CameraBehaviourDataStack()
	{
		stack = new List<CameraBehaviourDataLayer>();
		rebuildAllFlattened();
	}

	public void Clear()
	{
		stack.Clear();
	}

	public void AddLayer(CameraBehaviourDataLayer layer)
	{
		stack.Add(layer);
		rebuildAllFlattened();
	}

	public void RemoveLayer(CameraBehaviourDataLayer layer)
	{
		stack.Remove(layer);
		rebuildAllFlattened();
	}

	private int sortFunction(CameraBehaviourDataLayer x, CameraBehaviourDataLayer y)
	{
		return x.priority - y.priority;
	}

	protected void rebuildAllFlattened()
	{
		flattened_normal = new CameraBehaviourDataLayer();
		rebuildFlattened(flattened_normal, false);
		flattened_conduit = new CameraBehaviourDataLayer();
		rebuildFlattened(flattened_conduit, true);
	}

	protected void rebuildFlattened(CameraBehaviourDataLayer f, bool conduitOnly)
	{
		stack.Sort(sortFunction);
		for (int i = 0; i < stack.Count; i++)
		{
			if (stack[i].override_orthographicSize && (conduitOnly || !stack[i].conduitOnly))
			{
				f.orthographicSize = stack[i].orthographicSize;
			}
			if (stack[i].override_orthographicSizeSmoothTime && (conduitOnly || !stack[i].conduitOnly))
			{
				f.orthographicSizeSmoothTime = stack[i].orthographicSizeSmoothTime;
			}
			if (stack[i].override_positionSmoothTime && (conduitOnly || !stack[i].conduitOnly))
			{
				f.positionSmoothTime = stack[i].positionSmoothTime;
			}
			if (stack[i].override_normalCameraEulerX && (conduitOnly || !stack[i].conduitOnly))
			{
				f.normalCameraEulerX = stack[i].normalCameraEulerX;
			}
			if (stack[i].override_zTargetCameraEulerX && (conduitOnly || !stack[i].conduitOnly))
			{
				f.zTargetCameraEulerX = stack[i].zTargetCameraEulerX;
			}
			if (stack[i].override_eulerY && (conduitOnly || !stack[i].conduitOnly))
			{
				f.eulerY = stack[i].eulerY;
			}
			if (stack[i].override_eulerYSmoothTime && (conduitOnly || !stack[i].conduitOnly))
			{
				f.eulerYSmoothTime = stack[i].eulerYSmoothTime;
			}
			if (stack[i].override_eulerYMaxSmoothSpeed && (conduitOnly || !stack[i].conduitOnly))
			{
				f.eulerYMaxSmoothSpeed = stack[i].eulerYMaxSmoothSpeed;
			}
			if (stack[i].override_maxZTargetDistance && (conduitOnly || !stack[i].conduitOnly))
			{
				f.maxZTargetDistance = stack[i].maxZTargetDistance;
			}
			if (stack[i].override_moveCameraToZTargetParameter && (conduitOnly || !stack[i].conduitOnly))
			{
				f.moveCameraToZTargetParameter = stack[i].moveCameraToZTargetParameter;
			}
			if (stack[i].override_orthographicNearClipPlane && (conduitOnly || !stack[i].conduitOnly))
			{
				f.orthographicNearClipPlane = stack[i].orthographicNearClipPlane;
			}
			if (stack[i].override_perspectiveFarClipPlane && (conduitOnly || !stack[i].conduitOnly))
			{
				f.perspectiveFarClipPlane = stack[i].perspectiveFarClipPlane;
			}
			if (stack[i].override_perspectiveFOV && (conduitOnly || !stack[i].conduitOnly))
			{
				f.perspectiveFOV = stack[i].perspectiveFOV;
			}
			if (stack[i].override_cameraDistance && (conduitOnly || !stack[i].conduitOnly))
			{
				f.cameraDistance = stack[i].cameraDistance;
			}
			if (stack[i].override_orthographicDepth && (conduitOnly || !stack[i].conduitOnly))
			{
				f.orthographicDepth = stack[i].orthographicDepth;
			}
			if (stack[i].override_smoothCamera && (conduitOnly || !stack[i].conduitOnly))
			{
				f.smoothCamera = stack[i].smoothCamera;
			}
			if (stack[i].override_zTargettingPositionSmoothingTime && (conduitOnly || !stack[i].conduitOnly))
			{
				f.zTargettingPositionSmoothingTime = stack[i].zTargettingPositionSmoothingTime;
			}
			if (stack[i].override_zTargettingZoomAmount && (conduitOnly || !stack[i].conduitOnly))
			{
				f.zTargettingZoomAmount = stack[i].zTargettingZoomAmount;
			}
			if (stack[i].override_parameterizeZTargettingZoomByDistance && (conduitOnly || !stack[i].conduitOnly))
			{
				f.parameterizeZTargettingZoomByDistance = stack[i].parameterizeZTargettingZoomByDistance;
			}
			if (stack[i].override_eulerYTarget && (conduitOnly || !stack[i].conduitOnly))
			{
				f.eulerYTarget = stack[i].eulerYTarget;
			}
			if (stack[i].override_faceAwayFromEulerYTarget && (conduitOnly || !stack[i].conduitOnly))
			{
				f.faceAwayFromEulerYTarget = stack[i].faceAwayFromEulerYTarget;
			}
			if (stack[i].override_xRotationOverrideSmoothTime && (conduitOnly || !stack[i].conduitOnly))
			{
				f.xRotationOverrideSmoothTime = stack[i].xRotationOverrideSmoothTime;
			}
			if (stack[i].override_xRotationOverrideMaxSpeed && (conduitOnly || !stack[i].conduitOnly))
			{
				f.xRotationOverrideMaxSpeed = stack[i].xRotationOverrideMaxSpeed;
			}
		}
	}
}
