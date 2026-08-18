using System.Collections;
using UnityEngine;

public class CrossItemBehaviour : SpecificMagicItemBehaviour
{
	[SerializeField]
	protected Collider crossAOEtrigger;

	[SerializeField]
	protected ParticleSystem crossAOEParticleSystem;

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		if (mpSaysOkToUse())
		{
			cachedAnimator.SetTrigger("cross use");
		}
		yield return 0;
	}

	protected void __crossUse()
	{
		StartCoroutine(useCross());
	}

	protected IEnumerator useCross()
	{
		applyMPCost();
		crossAOEParticleSystem.Play();
		crossAOEtrigger.enabled = true;
		yield return 0;
		crossAOEtrigger.enabled = false;
	}
}
