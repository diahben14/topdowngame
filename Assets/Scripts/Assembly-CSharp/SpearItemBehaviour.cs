using System.Collections;
using UnityEngine;

public class SpearItemBehaviour : SpecificMagicItemBehaviour
{
	[SerializeField]
	protected Collider hitTriggerCollider;

	[SerializeField]
	protected Transform beamOrigin;

	[SerializeField]
	protected ParticleSystem chargeEmitter;

	[SerializeField]
	protected Transform beamTransform;

	[SerializeField]
	protected Renderer beamRenderer;

	[SerializeField]
	protected ParticleSystem beamParticleSystem;

	protected const float kMaxDistance = 64f;

	private Dat chargeDurationDat;

	protected override void specificMagicItemBehaviour_Awake()
	{
		chargeDurationDat = new Dat("spear_chargeDuration");
	}

	protected override IEnumerator onActionButtonDown(ButtonAssignableItem itemUsed, int actionButtonIndex, ButtonAssignableItem[] itemsUsedThisFrame)
	{
		if (mpSaysOkToUse())
		{
			cachedAnimator.SetBool("spear aim", true);
			chargeEmitter.Play();
			for (float timer = 0f; timer < chargeDurationDat.Value; timer += Time.deltaTime)
			{
				if (animationWasInterrupted)
				{
					break;
				}
				if (wasCancelled)
				{
					break;
				}
				yield return 0;
			}
			Debug.LogFormat("[Spear] done charge! interrupted? {0}; cancelled? {1}", animationWasInterrupted, wasCancelled);
			chargeEmitter.Stop();
			if (animationWasInterrupted || wasCancelled)
			{
				cachedAnimator.SetBool("spear aim", false);
				yield break;
			}
			StartCoroutine(fireSpear());
		}
		yield return 0;
	}

	protected IEnumerator fireSpear()
	{
		applyMPCost();
		cachedAnimator.SetTrigger("spear fire");
		cachedAnimator.SetBool("spear aim", false);
		Vector3 normalized = ((PlayerCharacter.ZTargetWorldPositionOrNull ?? (beamOrigin.position + base.transform.forward)) - beamOrigin.position).normalized;
		hitTriggerCollider.transform.rotation = Quaternion.LookRotation(normalized);
		beamRenderer.enabled = true;
		beamParticleSystem.Play();
		hitTriggerCollider.enabled = true;
		yield return new WaitForSeconds(0.1f);
		beamRenderer.enabled = false;
		beamParticleSystem.Stop();
		hitTriggerCollider.enabled = false;
	}
}
