using UnityEngine;

public class HPPickup : Pickup
{
	protected const string kPathToHPPickup = "hp_pickup";

	protected static GameObject heartPrefab;

	public static void Drop(Vector3 position)
	{
		setupIfNeeded();
		Object.Instantiate(heartPrefab, position, Quaternion.identity).GetComponent<Rigidbody>().AddForce(Vector3.up * 15f, ForceMode.Impulse);
	}

	protected static void setupIfNeeded()
	{
		if (heartPrefab == null)
		{
			heartPrefab = Resources.Load("hp_pickup") as GameObject;
			if (heartPrefab == null)
			{
				Debug.LogError("Couldn't load heart drop prefab");
			}
		}
	}

	protected override void onGetIt()
	{
		PlayerCharacter.GiveHP(new Dat("hp_restore_per_small_heart").IntValue);
	}
}
