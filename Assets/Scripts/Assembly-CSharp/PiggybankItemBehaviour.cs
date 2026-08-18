using System.Collections;
using FMODUnity;
using UnityEngine;

public class PiggybankItemBehaviour : SpecificItemBehaviour
{
	[SerializeField]
	protected float force = 5f;

	[SerializeField]
	protected float maxUpForceBoost = 10f;

	[EventRef]
	[SerializeField]
	protected string smashSFX;

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem usedItem, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		if (item.Quantity > 0)
		{
			item.Quantity--;
			SFX.PlayAudioClipHere(smashSFX, base.transform.position);
			CoinSpawner.SpawnCoins(item.Value, PlayerCharacter.WorldPosition + Vector3.up, force, maxUpForceBoost, false, false);
			yield return 0;
		}
	}
}
