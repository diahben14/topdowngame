using UnityEngine;

public class CreatureWaterFringeFX : MonoBehaviour
{
	[SerializeField]
	protected ParticleSystem movingPS;

	[SerializeField]
	protected ParticleSystem stillPS;

	protected const float disableStillPSVelocityTheshold = 1f;

	protected ParticleSystem.EmissionModule movingPSEmission;

	protected ParticleSystem.EmissionModule stillPSEmission;

	public Creature Creature { get; set; }

	private void Awake()
	{
		stillPSEmission = stillPS.emission;
		movingPSEmission = movingPS.emission;
	}

	private void LateUpdate()
	{
		if ((bool)Creature)
		{
			bool flag = Creature.Velocity.magnitude <= 1f;
			stillPSEmission.enabled = flag;
			movingPSEmission.enabled = !flag;
		}
	}

	internal void Kill()
	{
		movingPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		stillPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		Object.Destroy(base.gameObject, 3f);
	}
}
