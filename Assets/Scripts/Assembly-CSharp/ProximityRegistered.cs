using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ProximityRegistered<T> : MonoBehaviour where T : ProximityRegistered<T>
{
	[SerializeField]
	protected Transform overridePositionForFacing;

	protected static List<T> nearbyThings = new List<T>();

	protected T thisComponent;

	protected virtual bool isCandidateForBest
	{
		get
		{
			return true;
		}
	}

	protected virtual float dotProductAdjustment
	{
		get
		{
			return 0f;
		}
	}

	protected virtual Vector3 positionForFacing
	{
		get
		{
			if (!(overridePositionForFacing != null))
			{
				return base.transform.position;
			}
			return overridePositionForFacing.position;
		}
	}

	public static T GetBest()
	{
		return GetBest(0f);
	}

	public static T GetBest(float minimumFacingDot)
	{
		if (PlayerCharacter.Instanced)
		{
			Vector3 worldPosition = PlayerCharacter.WorldPosition;
			Vector3 forward = PlayerCharacter.Forward;
			cleanList();
			T result = null;
			float num = minimumFacingDot;
			for (int i = 0; i < nearbyThings.Count; i++)
			{
				T val = nearbyThings[i];
				Vector3 vector = val.positionForFacing;
				if (val.enabled && val.isCandidateForBest)
				{
					float num2 = Vector3.Dot((vector - worldPosition).normalized, forward);
					num2 += val.dotProductAdjustment;
					if (num2 > num)
					{
						result = val;
						num = num2;
					}
				}
			}
			return result;
		}
		return null;
	}

	protected static void cleanList()
	{
		nearbyThings.RemoveAll((T x) => x == null);
	}

	protected static void register(T thing)
	{
		nearbyThings.Add(thing);
	}

	protected static void deregister(T thing)
	{
		nearbyThings.Remove(thing);
	}

	protected virtual void onDisableProximityRegistered()
	{
	}

	protected virtual void onDestroyProximityRegistered()
	{
	}

	protected void Awake()
	{
		SphereCollider[] components = GetComponents<SphereCollider>();
		thisComponent = (T)this;
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i].isTrigger)
			{
				return;
			}
		}
		Debug.LogError("No trigger SphereCollider on ProximityRegistered " + base.name);
	}

	protected void OnTriggerEnter(Collider c)
	{
		if ((bool)c.GetComponent<PlayerCharacter>())
		{
			register(thisComponent);
		}
	}

	protected void OnTriggerExit(Collider c)
	{
		if ((bool)c.GetComponent<PlayerCharacter>())
		{
			deregister(thisComponent);
		}
	}

	protected void OnDestroy()
	{
		deregister(thisComponent);
		onDestroyProximityRegistered();
	}

	protected void OnDisable()
	{
		deregister(thisComponent);
		onDisableProximityRegistered();
	}
}
