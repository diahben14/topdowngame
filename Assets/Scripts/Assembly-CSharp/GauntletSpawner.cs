using System;
using System.Collections;
using UnityEngine;

public class GauntletSpawner : MonoBehaviour
{
	[Serializable]
	protected class GauntletWave
	{
		public GameObject waveHolder;

		public float delayBeforeNext = 30f;
	}

	[SerializeField]
	protected float elevatorStartDelay = 3f;

	[SerializeField]
	protected float firstWaveDelay = 6f;

	[SerializeField]
	protected GauntletWave[] waves;

	[SerializeField]
	protected Animator elevatorAnimator;

	[SerializeField]
	protected float noGauntletElevatorDuration = 8f;

	[SerializeField]
	protected StateVariable hasCompletedGauntlet;

	protected IEnumerator Start()
	{
		for (int i = 0; i < waves.Length; i++)
		{
			waves[i].waveHolder.SetActive(false);
		}
		yield return new WaitForSeconds(elevatorStartDelay);
		elevatorAnimator.SetTrigger("go");
		yield return new WaitForSeconds(firstWaveDelay);
		if (!hasCompletedGauntlet.BoolValue)
		{
			for (int j = 0; j < waves.Length; j++)
			{
				waves[j].waveHolder.SetActive(true);
				Monster[] componentsInChildren = GetComponentsInChildren<Monster>();
				for (int k = 0; k < componentsInChildren.Length; k++)
				{
					componentsInChildren[k].ReceiveBroadcastAggro();
				}
				yield return new WaitForSeconds(waves[j].delayBeforeNext);
			}
			elevatorAnimator.SetTrigger("stop");
			while (MonsterManager.MonsterCount > 0)
			{
				yield return 0;
			}
			hasCompletedGauntlet.BoolValue = true;
			elevatorAnimator.SetTrigger("doorOpen");
		}
		else
		{
			yield return new WaitForSeconds(noGauntletElevatorDuration);
			elevatorAnimator.SetTrigger("stop");
			elevatorAnimator.SetTrigger("doorOpen");
		}
	}
}
