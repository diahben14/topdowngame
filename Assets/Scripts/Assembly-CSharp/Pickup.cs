using FMODUnity;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractionReceiver
{
	[Header("Picking up")]
	[SerializeField]
	protected bool useInteractionToPickUp;

	[SerializeField]
	protected LanguageLine optionalPickupPrompt;

	[Header("Effects")]
	[SerializeField]
	[EventRef]
	protected string pickupSFX;

	[Header("Attraction (for non-interaction pickups)")]
	[SerializeField]
	protected float pickupDistance = 1.25f;

	[SerializeField]
	protected float initialNoAttractionDuration = 0.5f;

	[SerializeField]
	protected float minimumAttractDistance = 3f;

	[SerializeField]
	protected float minAttractSpeed = 3f;

	[SerializeField]
	protected float maxAttractSpeed = 3f;

	[SerializeField]
	protected float timeToDestroyRigidbody = 0.5f;

	protected static readonly float attractionSourceHeight;

	protected Rigidbody rb;

	protected SphereCollider sc;

	protected float spawnTime;

	protected float closestDistance = float.PositiveInfinity;

	protected virtual bool destroyOnPickup
	{
		get
		{
			return true;
		}
	}

	protected bool deadTimeOver
	{
		get
		{
			return Time.time > spawnTime + initialNoAttractionDuration;
		}
	}

	bool IInteractionReceiver.Interact(Item i)
	{
		if (useInteractionToPickUp)
		{
			return pickupSequence();
		}
		return false;
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		return true;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}

	protected virtual void onGetIt()
	{
	}

	protected virtual bool alreadyPickedUp()
	{
		return false;
	}

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		spawnTime = Time.time;
		if (alreadyPickedUp())
		{
			destroyOrDisable();
		}
	}

	private void FixedUpdate()
	{
		if (useInteractionToPickUp || !PlayerCharacter.Instanced || PlayerCharacter.InstanceIsDead)
		{
			return;
		}
		Vector3 vector = PlayerCharacter.WorldPosition + Vector3.up * attractionSourceHeight - base.transform.position;
		if (vector.magnitude < pickupDistance && deadTimeOver)
		{
			pickupSequence();
		}
		if (vector.magnitude < closestDistance)
		{
			closestDistance = vector.magnitude;
		}
		if (!(closestDistance < minimumAttractDistance) || !deadTimeOver)
		{
			return;
		}
		float t = Mathf.InverseLerp(minimumAttractDistance, 0f, closestDistance);
		float num = Mathf.Lerp(minAttractSpeed, maxAttractSpeed, t);
		Vector3 vector2 = vector.normalized * num;
		if ((bool)rb)
		{
			rb.velocity = vector2;
			if (Time.time > spawnTime + timeToDestroyRigidbody)
			{
				Object.Destroy(rb);
			}
		}
		else
		{
			base.transform.Translate(vector2 * Time.deltaTime, Space.World);
		}
	}

	protected void destroyOrDisable()
	{
		if (destroyOnPickup)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	protected virtual bool pickupSequence()
	{
		if ((bool)optionalPickupPrompt)
		{
			GenericPrompt.ShowPrompt(optionalPickupPrompt, getIt, null);
		}
		else
		{
			getIt();
		}
		return true;
	}

	protected void getIt()
	{
		SFX.PlayAudioClipHere(pickupSFX, base.transform.position);
		onGetIt();
		destroyOrDisable();
	}
}
