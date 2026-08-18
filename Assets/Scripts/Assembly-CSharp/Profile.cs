using System.Collections.Generic;
using UnityEngine;

public static class Profile
{
	private static readonly string kDefaultStartingScene = "Overworld Redux";

	private static readonly string kDefaultStartingFullID = "titlecard_start";

	private static readonly string kDefaultStartingCampfireScene = "Overworld Redux";

	private static readonly string kDefaultStartingCampfireFullID = "titlecard_start";

	private static readonly string kLastSpawnSceneNameTag = "last spawn scene name";

	private static readonly string kLastSpawnIDTag = "last spawn id";

	private static readonly string kCampfireSceneNameTag = "last campfire scene name";

	private static readonly string kCampfireIDTag = "last campfire id";

	private static readonly string kBloodstainSceneNameTag = "bloodstain scene name";

	private static readonly string kBloodstainPosition = "bloodstain position";

	private static readonly string kPageUnlockedPrefix = "unlocked page ";

	private static readonly string kLastPageViewed = "last page viewed";

	private static readonly int kDefaultLastPageViewed = -1;

	public static readonly string kInventoryQuantityPrefx = "inventory quantity ";

	private static readonly string kSupplyCoinDepositCountPrefix = "deposited ";

	private static readonly string kBloodstainQuantityPrefx = "bloodstain quantity ";

	private static readonly string kGameStarted = "game started";

	private static readonly string kPermanentStatePrefix = "permanentlyDead ";

	private static readonly string kSpiritWorldUnlockPrefix = "spiritWorldUnlock ";

	private static readonly string kFuseClosedPrefix = "fuseClosed ";

	private static readonly string kFuseLockPrefix = "fuseLocked ";

	private static readonly string kDemoStarted = "demoStarted";

	private static readonly string kHP = "hp";

	private static readonly string kSP = "sp";

	private static readonly string kMP = "mp";

	private static readonly string[] kRememberThese = new string[3] { "Screenmanager Is Fullscreen mode", "UnityGraphicsQuality", "thruShadowOffset" };

	public static bool PlayerCharacterShouldLoadMetersFromSaveData;

	public static void Reset()
	{
		PermanentStateByPositionManager.Reset();
		Monster.ClearRuntimeDeadMonsters();
		DeleteAll();
		SetInt("true", 1);
		StateVariable.NotifyAllSubscribers();
		SetLastSpawn(kDefaultStartingScene, kDefaultStartingFullID);
		SetCampfire(kDefaultStartingCampfireScene, kDefaultStartingCampfireFullID);
		SetLastViewedPage(kDefaultLastPageViewed);
		Inventory.Reset();
		Save();
	}

	public static void LoadFromPath(string p)
	{
		SaveFile.LoadFromPath(p);
		PlayerCharacterShouldLoadMetersFromSaveData = true;
	}

	public static void LoadFromFile(string f)
	{
		SaveFile.LoadFromFile(f);
		PlayerCharacterShouldLoadMetersFromSaveData = true;
	}

	public static void LoadPlayerMetersFromSaveDataIfNecessary()
	{
		if (PlayerCharacterShouldLoadMetersFromSaveData)
		{
			Debug.Log("[Profile] loading fox's meters from save data...");
			PlayerCharacter.HP = SaveFile.GetInt(kHP);
			PlayerCharacter.SetMP(2, SaveFile.GetInt(kMP));
			PlayerCharacterShouldLoadMetersFromSaveData = false;
		}
		else
		{
			Debug.Log("[Profile] NOT loading fox's meters from save data.");
		}
	}

	public static void SetUpDemoState()
	{
		SetDemoMode(true);
	}

	public static void SetDemoMode(bool value)
	{
		SetInt(kDemoStarted, value ? 1 : 0);
	}

	public static bool GetDemoMode()
	{
		return GetInt(kDemoStarted) != 0;
	}

	public static bool GameStarted()
	{
		return GetInt(kGameStarted) != 0;
	}

	public static void MarkGameAsStarted()
	{
		SetInt(kGameStarted, 1);
	}

	public static void GoToLastSpawn()
	{
		PlayerCharacterSpawn.GoTo(GetString(kLastSpawnSceneNameTag), GetString(kLastSpawnIDTag));
	}

	public static void SetLastSpawn(string loadedLevelName, string fullID)
	{
		SetString(kLastSpawnSceneNameTag, loadedLevelName);
		SetString(kLastSpawnIDTag, fullID);
	}

	public static void SetCampfire(string loadedLevelName, string fullID)
	{
		SetString(kCampfireSceneNameTag, loadedLevelName);
		SetString(kCampfireIDTag, fullID);
	}

	public static string GetCampfireScene()
	{
		return GetString(kCampfireSceneNameTag);
	}

	public static string GetCampfireID()
	{
		return GetString(kCampfireIDTag);
	}

	public static void SetBloodstain(string scene, Vector3 position)
	{
		SetString(kBloodstainSceneNameTag, scene);
		SetFloat(kBloodstainPosition + " x", position.x);
		SetFloat(kBloodstainPosition + " y", position.y);
		SetFloat(kBloodstainPosition + " z", position.z);
	}

	public static void ClearBloodstain()
	{
		SetString(kBloodstainSceneNameTag, string.Empty);
		SetFloat(kBloodstainPosition + " x", 0f);
		SetFloat(kBloodstainPosition + " y", 0f);
		SetFloat(kBloodstainPosition + " z", 0f);
	}

	public static string GetBloodstainScene()
	{
		return GetString(kBloodstainSceneNameTag);
	}

	public static Vector3 GetBloodstainLocation()
	{
		return new Vector3(GetFloat(kBloodstainPosition + " x"), GetFloat(kBloodstainPosition + " y"), GetFloat(kBloodstainPosition + " z"));
	}

	public static void SetInventoryQuantity(Item i, int value)
	{
		SetInt(i.CachedInventoryString, value);
	}

	public static int GetInventoryQuantity(Item i)
	{
		return GetInt(i.CachedInventoryString);
	}

	public static void SetBloodstainQuantity(Item i, int value)
	{
		SetInt(kBloodstainQuantityPrefx + i.name, value);
	}

	public static int GetBloodstainQuantity(Item i)
	{
		return GetInt(kBloodstainQuantityPrefx + i.name);
	}

	public static void SetSupplyCoinDepositCount(SupplyCoinItem i, int value)
	{
		SetInt(kSupplyCoinDepositCountPrefix + i.name, value);
	}

	public static int GetSupplyCoinDepositCount(SupplyCoinItem i)
	{
		return GetInt(kSupplyCoinDepositCountPrefix + i.name);
	}

	public static bool IsSpiritWorldSpawnUnlocked(string fullID)
	{
		return GetInt(kSpiritWorldUnlockPrefix + fullID) != 0;
	}

	public static void UnlockSpiritWorldSpawn(string fullID)
	{
		SetInt(kSpiritWorldUnlockPrefix + fullID, 1);
	}

	public static bool IsPageUnlocked(int i)
	{
		return GetInt(kPageUnlockedPrefix + i) != 0;
	}

	public static void UnlockPage(int i)
	{
		SetInt(kPageUnlockedPrefix + i, 1);
	}

	public static bool AnyPageEverViewed()
	{
		return GetInt(kLastPageViewed) != kDefaultLastPageViewed;
	}

	public static int GetLastViewedPage()
	{
		return GetInt(kLastPageViewed);
	}

	public static void SetLastViewedPage(int i)
	{
		SetInt(kLastPageViewed, i);
	}

	public static int GetInt(string name)
	{
		return SaveFile.GetInt(name);
	}

	public static void SetInt(string name, int value)
	{
		SaveFile.SetInt(name, value);
	}

	public static float GetFloat(string name)
	{
		return SaveFile.GetFloat(name);
	}

	public static void SetFloat(string name, float value)
	{
		SaveFile.SetFloat(name, value);
	}

	public static string GetString(string name)
	{
		return SaveFile.GetString(name);
	}

	public static void SetString(string name, string value)
	{
		SaveFile.SetString(name, value);
	}

	public static void Save()
	{
		SaveFile.SetInt(kHP, PlayerCharacter.HP);
		SaveFile.SetInt(kMP, (int)PlayerCharacter.GetMP(2));
		SaveFile.SaveToDisk();
	}

	public static void DeleteAll()
	{
		SaveFile.DeleteAll();
	}

	public static void SavePermanentStatesByPosition(int sceneIndex, HashSet<Vector3> positions)
	{
		string name = kPermanentStatePrefix + sceneIndex;
		string text = "";
		int count = positions.Count;
		Vector3[] array = new Vector3[count];
		positions.CopyTo(array);
		for (int i = 0; i < count; i++)
		{
			text = text + array[i].ToString() + ((i == count - 1) ? "" : ";");
		}
		SetString(name, text);
	}

	public static void LoadPermanentStatesByPosition(int sceneIndex, HashSet<Vector3> positions)
	{
		positions.Clear();
		string[] array = GetString(kPermanentStatePrefix + sceneIndex).Split(';');
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			Vector3? vector = vector3FromString(array[i]);
			if (vector.HasValue)
			{
				positions.Add(vector ?? Vector3.zero);
			}
		}
	}

	public static bool GetFuseClosed(int id)
	{
		return GetInt(kFuseClosedPrefix + id) != 0;
	}

	public static void SetFuseClosed(int id, bool value)
	{
		SetInt(kFuseClosedPrefix + id, value ? 1 : 0);
	}

	public static bool GetFuseLocked(int id)
	{
		return GetInt(kFuseLockPrefix + id) != 0;
	}

	public static void SetFuseLocked(int id, bool value)
	{
		SetInt(kFuseLockPrefix + id, value ? 1 : 0);
	}

	public static Vector3? vector3FromString(string inputString)
	{
		if (inputString.Length > 1)
		{
			string[] array = inputString.Substring(1, inputString.Length - 2).Split(',');
			float x = float.Parse(array[0]);
			float y = float.Parse(array[1]);
			float z = float.Parse(array[2]);
			return new Vector3(x, y, z);
		}
		return null;
	}
}
