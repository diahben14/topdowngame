using System.Collections;
using UnityEngine;

public class TossItemBehaviour : SpecificItemBehaviour
{
	[SerializeField]
	private Transform throwItem;

	[SerializeField]
	private Transform spawnTransform;

	[SerializeField]
	private float throwAngle = 45f;

	[SerializeField]
	private float minThrowForce = 2f;

	[SerializeField]
	private float maxThrowForce = 10f;

	[SerializeField]
	private float torqueForce = 1f;

	[SerializeField]
	private float timeForFullForce = 0.66f;

	[SerializeField]
	private float reductionFactorToAccountForBounce = 0.8f;

	protected float throwChargeTime;

	protected static TossItemBehaviour currentTossBehaviour;

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		if (item.Quantity > 0)
		{
			cachedAnimator.SetTrigger("ready to toss");
			throwChargeTime = 0f;
			throwChargeTime = 1f;
			if (!animationWasInterrupted)
			{
				currentTossBehaviour = this;
				cachedAnimator.SetTrigger("toss");
			}
		}
		yield break;
	}

	protected void __doToss()
	{
		if (this == currentTossBehaviour)
		{
			item.Quantity--;
			Rigidbody component = Object.Instantiate(throwItem, spawnTransform.position, spawnTransform.rotation).gameObject.GetComponent<Rigidbody>();
			Vector3 vector = Mathx.CalculateTossVelocity(target: PlayerCharacter.ZTargetWorldPositionOrNull ?? (base.transform.position + PlayerCharacter.Forward * 100f), origin: spawnTransform.position, angle: throwAngle);
			vector *= reductionFactorToAccountForBounce;
			vector = Vector3.ClampMagnitude(vector, maxThrowForce);
			component.AddForce(vector, ForceMode.Impulse);
			component.AddTorque(Random.onUnitSphere * torqueForce, ForceMode.VelocityChange);
		}
	}
}
