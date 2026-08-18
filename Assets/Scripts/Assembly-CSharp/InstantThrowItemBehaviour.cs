using System.Collections;
using UnityEngine;

public class InstantThrowItemBehaviour : SpecificItemBehaviour
{
	[SerializeField]
	private RaycastProjectile projectilePrefab;

	[SerializeField]
	private Transform spawnTransform;

	[SerializeField]
	private float fireForceMagnitude = 100f;

	[SerializeField]
	private string throwTriggerString = "throw dart";

	[SerializeField]
	private float timeToSpawn = 0.25f;

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		if (item.Quantity > 0)
		{
			cachedAnimator.SetTrigger(throwTriggerString);
			item.Quantity--;
			yield return new WaitForSeconds(timeToSpawn);
			Vector3 forward = (PlayerCharacter.ZTargetWorldPositionOrNull ?? (spawnTransform.position + base.transform.forward)) - spawnTransform.position;
			Quaternion rotation = Quaternion.LookRotation(forward);
			RaycastProjectile raycastProjectile = Object.Instantiate(projectilePrefab, spawnTransform.position, rotation);
			Vector3 velocity = forward.normalized * fireForceMagnitude;
			raycastProjectile.Velocity = velocity;
		}
	}
}
