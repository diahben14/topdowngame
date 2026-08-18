using System.Collections;
using UnityEngine;

public class SwordItemBehaviour : SpecificItemBehaviour
{
	[SerializeField]
	protected string triggerString = "swing sword";

	[SerializeField]
	protected bool needStaminaToSwing;

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		if (!needStaminaToSwing || StaminaSettings.AttackRequirements != StaminaSettings.StaminaRequirementMode.NOT_ALLOWED_WHEN_STAMINA_IS_ZERO || PlayerCharacter.Stamina > 0f)
		{
			cachedAnimator.SetTrigger(triggerString);
		}
		yield break;
	}
}
