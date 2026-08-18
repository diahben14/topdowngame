using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
public class Chest : MonoBehaviour, IInteractionReceiver
{
	[Header("Chest Specific")]
	[SerializeField]
	protected Item itemContents;

	[SerializeField]
	protected int itemContentsQuantity = 1;

	[SerializeField]
	protected int moneySprayQuantity;

	[SerializeField]
	protected StateVariable openedStateVar;

	[SerializeField]
	protected Creature[] killAllTheseFirst;

	[SerializeField]
	protected Animator fairyReleaseAnimation;

	[SerializeField]
	protected float presentationDelay = 1f;

	[SerializeField]
	protected float moneySprayDelay = 0.833f;

	[SerializeField]
	protected Transform moneySpawnPosition;

	[HideInInspector]
	[SerializeField]
	protected Transform characterOpeningTransform;

	protected Animator cachedAnimator;

	protected bool opening;

	private bool interrupted;

	protected void Awake()
	{
		cachedAnimator = GetComponent<Animator>();
		if (openedStateVar == null || openedStateVar.BoolValue)
		{
			cachedAnimator.CrossFadeInFixedTime("open", 0f);
			if (fairyReleaseAnimation != null)
			{
				fairyReleaseAnimation.gameObject.SetActive(false);
			}
		}
		for (int i = 0; i < killAllTheseFirst.Length; i++)
		{
			if (killAllTheseFirst[i] != null)
			{
				Creature obj = killAllTheseFirst[i];
				obj.OnDestroyCallbacks = (Action)Delegate.Combine(obj.OnDestroyCallbacks, new Action(onCreatureDestroyed));
			}
		}
		checkKillAllTheseFirstList();
	}

	public void onCreatureDestroyed()
	{
		checkKillAllTheseFirstList();
	}

	bool IInteractionReceiver.Interact(Item i)
	{
		StartCoroutine(openSequence());
		return true;
	}

	bool IInteractionReceiver.CanReceiveInteration()
	{
		if (openedStateVar != null && !openedStateVar.BoolValue)
		{
			return !opening;
		}
		return false;
	}

	bool IInteractionReceiver.ShouldShowGenericInteractPrompt()
	{
		return true;
	}

	protected void checkKillAllTheseFirstList()
	{
		if (!(this != null))
		{
			return;
		}
		bool active = true;
		for (int i = 0; i < killAllTheseFirst.Length; i++)
		{
			if (killAllTheseFirst[i] != null && !killAllTheseFirst[i].IsDead)
			{
				active = false;
			}
		}
		base.gameObject.SetActive(active);
		if (openedStateVar == null || openedStateVar.BoolValue)
		{
			cachedAnimator.CrossFadeInFixedTime("open", 0f);
		}
	}

	public void InterruptOpening()
	{
		interrupted = true;
	}

	protected IEnumerator openSequence()
	{
		opening = true;
		interrupted = false;
		PlayerCharacter.MagnetizeToPositionAndFacing(characterOpeningTransform.position, characterOpeningTransform.rotation);
		PlayerCharacter.PlayOpenChestAnimation(this);
		cachedAnimator.SetTrigger("open");
		float delay = ((itemContents != null) ? presentationDelay : moneySprayDelay);
		for (float d = 0f; d < delay; d += Time.deltaTime)
		{
			if (interrupted)
			{
				break;
			}
			yield return 0;
		}
		opening = false;
		if (!interrupted)
		{
			openedStateVar.BoolValue = true;
			if (fairyReleaseAnimation != null)
			{
				fairyReleaseAnimation.SetTrigger("go");
			}
			else if (itemContents != null)
			{
				itemContents.Quantity += itemContentsQuantity;
				ItemPresentation.PresentItem(itemContents, itemContentsQuantity);
			}
			else
			{
				CoinSpawner.SpawnCoins(moneySprayQuantity, moneySpawnPosition.position);
			}
		}
		else
		{
			cachedAnimator.Play("closed", 0);
		}
	}

	protected void OnDrawGizmos()
	{
		if (killAllTheseFirst == null)
		{
			return;
		}
		for (int i = 0; i < killAllTheseFirst.Length; i++)
		{
			if (killAllTheseFirst[i] != null)
			{
				Gizmos.color = new Color(1f, 0.4f, 0.1f, 0.44f);
				Gizmos.DrawLine(base.transform.position, killAllTheseFirst[i].transform.position);
			}
		}
	}
}
