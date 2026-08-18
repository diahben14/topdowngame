using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
	protected static bool setupDone;

	protected static List<ItemPickup> pickupsList;

	protected const string kPathToCoinPickups = "CoinPickups/";

	protected const float kDefaultSpawnForce = 3f;

	protected const float kDefaultMaxUpForceBoost = 22f;

	protected static CoroutineRunner coroutineRunner;

	public static void SpawnCoins(int valueToSpawn, Vector3 position)
	{
		SpawnCoins(valueToSpawn, position, 3f, 22f, false, false);
	}

	public static void SpawnCoins(int valueToSpawn, Vector3 position, float force, float maxUpForceBoost, bool preferHighestValueGems, bool stagger)
	{
		setupIfNeeded();
		List<ItemPickup> list = makeSpawnList(valueToSpawn, preferHighestValueGems);
		if (list.Count > 0)
		{
			list.Shuffle();
			coroutineRunner.StartCoroutine(SpawnSequence(position, force, maxUpForceBoost, list, stagger));
		}
	}

	private static IEnumerator SpawnSequence(Vector3 position, float force, float maxUpForceBoost, List<ItemPickup> toSpawn, bool stagger)
	{
		Vector3 direction = Quaternion.Euler(0f, Random.Range(0, 360), 0f) * Vector3.forward;
		Quaternion rotation = Quaternion.Euler(0f, 360f / (float)toSpawn.Count, 0f);
		for (int i = 0; i < toSpawn.Count; i++)
		{
			Rigidbody component = Object.Instantiate(toSpawn[i], position, Quaternion.Euler(0f, Random.Range(0, 90), 0f)).transform.GetComponent<Rigidbody>();
			if ((bool)component)
			{
				Vector3 force2 = direction * force + Vector3.up * maxUpForceBoost;
				component.AddForce(force2, ForceMode.Impulse);
				direction = rotation * direction;
			}
			if (stagger)
			{
				yield return new WaitForSeconds(0.005f);
			}
		}
	}

	public static List<Transform> InstantiateDumbCoins(int valueToSpawn, Vector3 position, bool preferHighestValueGems)
	{
		setupIfNeeded();
		List<ItemPickup> list = makeSpawnList(valueToSpawn, preferHighestValueGems);
		List<Transform> list2 = new List<Transform>();
		if (list.Count > 0)
		{
			list.Shuffle();
			for (int i = 0; i < list.Count; i++)
			{
				ItemPickup itemPickup = Object.Instantiate(list[i], position, Quaternion.identity);
				Rigidbody component = itemPickup.transform.GetComponent<Rigidbody>();
				Collider component2 = itemPickup.gameObject.GetComponent<Collider>();
				if ((bool)component)
				{
					Object.Destroy(component);
				}
				if ((bool)component2)
				{
					Object.Destroy(component2);
				}
				if ((bool)itemPickup)
				{
					Object.Destroy(itemPickup);
				}
				list2.Add(itemPickup.transform);
			}
		}
		return list2;
	}

	private static List<ItemPickup> makeSpawnList(int valueToSpawn, bool preferHighestValueGems)
	{
		List<ItemPickup> list = new List<ItemPickup>();
		int num = valueToSpawn;
		for (int i = 0; i < pickupsList.Count; i++)
		{
			while (true)
			{
				bool num2;
				if (!preferHighestValueGems)
				{
					if (num > pickupsList[i].QuantityToGive * 2)
					{
						goto IL_000c;
					}
					if (num <= 0)
					{
						break;
					}
					num2 = pickupsList[i].QuantityToGive == 1;
				}
				else
				{
					num2 = num >= pickupsList[i].QuantityToGive;
				}
				if (!num2)
				{
					break;
				}
				goto IL_000c;
				IL_000c:
				list.Add(pickupsList[i]);
				num -= pickupsList[i].QuantityToGive;
			}
		}
		return list;
	}

	protected static void setupIfNeeded()
	{
		if (!setupDone)
		{
			ItemPickup[] collection = Resources.LoadAll<ItemPickup>("CoinPickups/");
			pickupsList = new List<ItemPickup>();
			pickupsList.AddRange(collection);
			pickupsList.Sort((ItemPickup x, ItemPickup y) => y.QuantityToGive.CompareTo(x.QuantityToGive));
			setupDone = true;
			coroutineRunner = new GameObject("CoinSpanwer coroutine runner").AddComponent<CoroutineRunner>();
			Object.DontDestroyOnLoad(coroutineRunner);
		}
	}
}
