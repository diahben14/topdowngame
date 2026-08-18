using System.Collections;
using UnityEngine;

public class LanternItemBehaviour : SpecificMagicItemBehaviour
{
	[SerializeField]
	protected GameObject effectPrefab;

	[SerializeField]
	protected float effectSpawnDistance;

	[SerializeField]
	protected Transform spawnTransform;

	[SerializeField]
	protected float throwForce;

	[SerializeField]
	protected float throwAngle;

	[SerializeField]
	protected Collider ignoreCollider;

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		if (!PlayerCharacter.ShieldUp && mpSaysOkToUse())
		{
			cachedAnimator.SetTrigger("lantern");
			yield return 0;
		}
	}

	protected void __lanternToss()
	{
		if (mpSaysOkToUse())
		{
			CameraController.AddFastScreenshake(1f);
			applyMPCost();
		}
	}
}
