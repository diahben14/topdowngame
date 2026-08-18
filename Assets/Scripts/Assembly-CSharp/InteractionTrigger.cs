using UnityEngine;

public class InteractionTrigger : ProximityRegistered<InteractionTrigger>
{
	[Tooltip("0 is normal (90º). More negative = narrower angle. More positive = broader angle")]
	[SerializeField]
	public float m_dotProductAdjustment;

	[SerializeField]
	protected Transform optionalPromptPositionAnchor;

	protected IInteractionReceiver[] receivers;

	protected bool showingPrompt;

	public Vector3 AnchorPosition
	{
		get
		{
			if (optionalPromptPositionAnchor != null)
			{
				return optionalPromptPositionAnchor.position;
			}
			return base.transform.position;
		}
	}

	protected override bool isCandidateForBest
	{
		get
		{
			if (receivers != null)
			{
				for (int i = 0; i < receivers.Length; i++)
				{
					if (receivers[i].CanReceiveInteration())
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	protected override float dotProductAdjustment
	{
		get
		{
			return m_dotProductAdjustment;
		}
	}

	public bool AnyInteractionReceiverShowsGenericInteractPrompt
	{
		get
		{
			bool flag = false;
			for (int i = 0; i < receivers.Length; i++)
			{
				flag |= receivers[i].ShouldShowGenericInteractPrompt();
			}
			return flag;
		}
	}

	public bool Interact(Item item)
	{
		if (receivers != null)
		{
			bool flag = false;
			for (int i = 0; i < receivers.Length; i++)
			{
				flag |= receivers[i].Interact(item);
			}
			return flag;
		}
		Debug.LogError("No IInteractionReceivers on Interactable", this);
		return false;
	}

	protected void Start()
	{
		receivers = GetComponents<IInteractionReceiver>();
	}
}
