using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class UnscaledParticleSystem : MonoBehaviour
{
	protected ParticleSystem ps;

	protected void Awake()
	{
		ps = GetComponent<ParticleSystem>();
	}

	protected void Update()
	{
		ps.Simulate(TimeManager.UnscaledDeltaTime, true, false);
	}
}
