using System.Collections;
using UnityEngine;

public class TorchItemBehaviour : SpecificItemBehaviour
{
	protected override IEnumerator onActionButtonDown(ButtonAssignableItem usedItem, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		cachedAnimator.SetBool("torch", true);
		while (PlayerCharacter.itemBehaviourInputUps[actionButtonIndex])
		{
			yield return new WaitForFixedUpdate();
		}
		cachedAnimator.SetBool("torch", false);
	}
}
