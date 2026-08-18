using UnityEngine;

public class MPPickup : Pickup
{
	protected static Dat mpToGive;

	protected const string kPathToHPPickup = "mp_pickup";

	protected static GameObject mpPrefab;

	public static void Drop(float value, Vector3 position)
	{
	}

	protected static void setupIfNeeded()
	{
		if (mpPrefab == null)
		{
			mpPrefab = Resources.Load("mp_pickup") as GameObject;
			if (mpPrefab == null)
			{
				Debug.LogError("Couldn't load mp drop prefab");
			}
			mpToGive = new Dat("mp_restore_per_drop");
		}
	}

	protected override void onGetIt()
	{
		float mP = PlayerCharacter.GetMP(2);
		if (mP < (float)PlayerCharacter.GetMaxMP(2))
		{
			mP += mpToGive.Value;
			PlayerCharacter.SetMP(2, mP);
		}
	}
}
