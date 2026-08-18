using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ElectricityEffect : MonoBehaviour
{
	[SerializeField]
	protected float lifeTimeMin = 0.1f;

	[SerializeField]
	protected float lifeTimeMax = 1f;

	[SerializeField]
	protected float speedScale = 100f;

	[SerializeField]
	protected float minVelocityChange = 1f;

	[SerializeField]
	protected float maxVelocityChange = 1f;

	protected const int kMaxParticleCount = 16;

	protected bool[] alive = new bool[16];

	protected Vector3[] position = new Vector3[16];

	protected Vector3[] velocity = new Vector3[16];

	protected float[] energy = new float[16];

	protected bool coroutineRunning;

	protected int livingParticles;

	protected int index;

	protected Quaternion lastVelocity = Quaternion.identity;

	protected LineRenderer lineRenderer;

	public void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.positionCount = 16;
		for (int i = 0; i < 16; i++)
		{
			position[i] = (velocity[i] = Vector3.zero);
		}
		for (int j = 0; j < 16; j++)
		{
			addParticle();
		}
		StartCoroutine(animateParticlesCoroutine());
	}

	protected void addParticle()
	{
		if (livingParticles < 16)
		{
			while (alive[index])
			{
				index = (index + 1) % 16;
			}
			alive[index] = true;
			position[index] = Vector3.zero;
			Quaternion quaternion = Quaternion.Slerp(lastVelocity, Random.rotationUniform, Random.Range(minVelocityChange, maxVelocityChange));
			velocity[index] = quaternion * Vector3.forward;
			lastVelocity = quaternion;
			velocity[index] *= Mathf.Pow(velocity[index].magnitude, 3f);
			energy[index] = Random.Range(lifeTimeMin, lifeTimeMax);
			livingParticles++;
		}
	}

	protected IEnumerator animateParticlesCoroutine()
	{
		if (coroutineRunning)
		{
			yield break;
		}
		coroutineRunning = true;
		while (livingParticles > 0)
		{
			for (int i = 0; i < 16; i++)
			{
				if (alive[i])
				{
					updateParticle(i);
				}
			}
			lineRenderer.SetPositions(position);
			yield return 0;
		}
		coroutineRunning = false;
	}

	protected void updateParticle(int indexToUpdate)
	{
		position[indexToUpdate] += velocity[indexToUpdate] * Time.deltaTime * speedScale;
		energy[indexToUpdate] -= Time.deltaTime;
		if (energy[indexToUpdate] < 0f)
		{
			alive[indexToUpdate] = false;
			livingParticles--;
			addParticle();
		}
	}
}
