using UnityEngine;

public class AnimatedParticleSystemHelper : MonoBehaviour
{
	[SerializeField]
	protected bool emit;

	[SerializeField]
	protected bool useUnscaledTime;

	protected float emissionCounter;

	protected ParticleSystem ps;

	protected void Awake()
	{
		ps = GetComponent<ParticleSystem>();
	}

	protected void Update()
	{
		if (emit && ps != null)
		{
			float num = Random.Range(ps.emission.rateOverTime.constantMin, ps.emission.rateOverTime.constantMax);
			emissionCounter += num * (useUnscaledTime ? TimeManager.UnscaledDeltaTime : Time.deltaTime);
			while (emissionCounter > 1f)
			{
				ps.Emit(1);
				emissionCounter -= 1f;
			}
		}
	}
}
