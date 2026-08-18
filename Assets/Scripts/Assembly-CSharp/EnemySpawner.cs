using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField]
	private Creature[] toSpawnEasy;

	[SerializeField]
	private Creature[] toSpawnHard;

	[SerializeField]
	protected float radius;

	[SerializeField]
	protected int easyToSpawn;

	[SerializeField]
	protected int hardToSpawn;

	protected int aliveCount;

	protected IEnumerator Start()
	{
		while (true)
		{
			if (aliveCount > 0)
			{
				yield return new WaitForSeconds(0.1f);
				continue;
			}
			StartCoroutine(spawnBatch());
			yield return new WaitForSeconds(0.1f);
		}
	}

	protected IEnumerator spawnBatch()
	{
		List<Creature> list = new List<Creature>();
		for (int i = 0; i < 2; i++)
		{
			Creature[] array = ((i == 0) ? toSpawnEasy : toSpawnHard);
			int num = UnityEngine.Random.Range(0, array.Length);
			for (int j = 0; j < ((i == 0) ? easyToSpawn : hardToSpawn); j++)
			{
				Vector3 vector = Vector3.Scale(UnityEngine.Random.insideUnitSphere, new Vector3(1f, 0f, 1f)).normalized * radius;
				Quaternion rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0, 360), 0f);
				Creature creature = UnityEngine.Object.Instantiate(array[num], base.transform.position + vector, rotation);
				aliveCount++;
				creature.OnDestroyCallbacks = (Action)Delegate.Combine(creature.OnDestroyCallbacks, new Action(onEnemyDestroyed));
				creature.Paused = true;
				list.Add(creature);
			}
		}
		yield return new WaitForSeconds(0.5f);
		foreach (Creature item in list)
		{
			item.Paused = false;
		}
	}

	protected void onEnemyDestroyed()
	{
		aliveCount--;
	}
}
