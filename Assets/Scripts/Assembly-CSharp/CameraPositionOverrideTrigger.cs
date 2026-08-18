using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class CameraPositionOverrideTrigger : MonoBehaviour
{
	protected enum MappingMode
	{
		CLAMP = 0,
		PROPORTIONAL = 1,
		NONE = 2
	}

	[SerializeField]
	protected Transform overrideTransform;

	[SerializeField]
	protected MappingMode mappingMode;

	[SerializeField]
	protected bool suppressGizmos;

	[SerializeField]
	protected CameraBehaviourDataLayer data;

	[Header("Use this to constrain camera preview when editing.")]
	[SerializeField]
	protected bool lockInEditor;

	protected static List<CameraPositionOverrideTrigger> triggers = new List<CameraPositionOverrideTrigger>();

	protected BoxCollider _boxCollider;

	protected bool registeredWithCameraController;

	protected int playerInTriggerCount;

	protected int priority
	{
		get
		{
			return data.priority;
		}
	}

	public static bool PlayerInTrigger
	{
		get
		{
			cullNullsFromList();
			for (int i = 0; i < triggers.Count; i++)
			{
				if (triggers[i].enabled)
				{
					return true;
				}
			}
			return false;
		}
	}

	public CameraBehaviourDataLayer CameraBehaviourDataLayer
	{
		get
		{
			return data;
		}
	}

	protected BoxCollider BoxCollider
	{
		get
		{
			if (_boxCollider == null)
			{
				_boxCollider = GetComponent<BoxCollider>();
			}
			return _boxCollider;
		}
	}

	public static CameraPositionOverrideTrigger BestTrigger(Vector3 position)
	{
		CameraPositionOverrideTrigger cameraPositionOverrideTrigger = null;
		if (triggers.Count == 0)
		{
			return null;
		}
		float num = float.PositiveInfinity;
		foreach (CameraPositionOverrideTrigger trigger in triggers)
		{
			if (trigger != null && trigger.enabled && trigger.gameObject.activeInHierarchy)
			{
				float num2 = Vector3.Distance(position, trigger.transform.position);
				if ((cameraPositionOverrideTrigger == null || trigger.priority >= cameraPositionOverrideTrigger.priority) && (num2 < num || trigger.priority > cameraPositionOverrideTrigger.priority))
				{
					num = num2;
					cameraPositionOverrideTrigger = trigger;
				}
			}
		}
		if (cameraPositionOverrideTrigger == null)
		{
			return null;
		}
		return cameraPositionOverrideTrigger;
	}

	protected static void cullNullsFromList()
	{
		triggers.RemoveAll((CameraPositionOverrideTrigger item) => item == null);
	}

	public Vector3 MapPosition(Vector3 position)
	{
		switch (mappingMode)
		{
		case MappingMode.PROPORTIONAL:
		{
			Vector3 vector2 = base.transform.InverseTransformPoint(position);
			Vector3 center = BoxCollider.center;
			Vector3 a = vector2 - center;
			Vector3 b = new Vector3(1f / BoxCollider.size.x, 1f / BoxCollider.size.y, 1f / BoxCollider.size.z);
			Vector3 position3 = Vector3.Scale(a, b);
			return overrideTransform.TransformPoint(position3);
		}
		case MappingMode.CLAMP:
		{
			Vector3 vector = overrideTransform.InverseTransformPoint(position);
			Vector3 position2 = new Vector3(Mathf.Clamp(vector.x, -0.5f, 0.5f), Mathf.Clamp(vector.y, -0.5f, 0.5f), Mathf.Clamp(vector.z, -0.5f, 0.5f));
			return overrideTransform.TransformPoint(position2);
		}
		case MappingMode.NONE:
			return position;
		default:
			Debug.LogError("Bad MappingMode");
			return Vector3.zero;
		}
	}

	private void Awake()
	{
		if (Application.isPlaying)
		{
			triggers.Clear();
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (Application.isPlaying && c.gameObject.GetComponent<PlayerCharacter>() != null)
		{
			playerInTriggerCount++;
			updateCameraControllerRegistration();
			triggers.Add(this);
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if (Application.isPlaying && c.gameObject.GetComponent<PlayerCharacter>() != null)
		{
			playerInTriggerCount--;
			updateCameraControllerRegistration();
			if (triggers.Contains(this))
			{
				triggers.Remove(this);
			}
			else
			{
				Debug.LogError("Tried to remove trigger but it wasn't in the list.");
			}
		}
	}

	private void OnEnable()
	{
		updateCameraControllerRegistration();
	}

	private void OnDisable()
	{
		updateCameraControllerRegistration();
	}

	private void updateCameraControllerRegistration()
	{
		if (playerInTriggerCount > 0 && base.enabled)
		{
			if (!registeredWithCameraController)
			{
				CameraController.AddCameraBehaviourDataLayer(data);
				registeredWithCameraController = true;
			}
		}
		else if (registeredWithCameraController)
		{
			CameraController.RemoveCameraBehaviourDataLayer(data);
			registeredWithCameraController = false;
		}
	}
}
