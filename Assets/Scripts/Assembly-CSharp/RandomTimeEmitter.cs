using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class RandomTimeEmitter : MonoBehaviour
{
	private ParticleSystem particleSystem;

	public void Awake()
	{
		particleSystem = GetComponent<ParticleSystem>();
		ParticleSystem.EmissionModule emission = particleSystem.emission;
		emission.enabled = false;
	}

	public void Update()
	{
		float num = 1f - Mathf.Exp((0f - particleSystem.emission.rateOverTime.constant) * Time.deltaTime);
		if (Random.value < num)
		{
			particleSystem.Emit(1);
		}
	}
}
