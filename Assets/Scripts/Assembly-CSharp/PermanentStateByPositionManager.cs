using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PermanentStateByPositionManager : MonoBehaviour
{
	protected const int kRoundingResolution = 1;

	protected static PermanentStateByPositionManager instance;

	protected static HashSet<Vector3> deadPositionsInCurrentScene = new HashSet<Vector3>();

	protected static int lastLoadedSceneBuildIndex = -1;

	public static void Reset()
	{
		deadPositionsInCurrentScene.Clear();
	}

	public static bool GetDead(Vector3 position)
	{
		Vector3 item = roundPosition(position);
		return deadPositionsInCurrentScene.Contains(item);
	}

	public static void SetDead(Vector3 position, bool value)
	{
		Vector3 item = roundPosition(position);
		if (value)
		{
			deadPositionsInCurrentScene.Add(item);
		}
		else
		{
			deadPositionsInCurrentScene.Remove(item);
		}
	}

	protected static Vector3 roundPosition(Vector3 position)
	{
		return new Vector3(round(position.x), round(position.y), round(position.z));
	}

	protected static float round(float f)
	{
		return (float)(int)(f * 1f) / 1f;
	}

	protected void Awake()
	{
		instance = this;
		loadPermanentStates();
		SceneManager.sceneLoaded += onLevelWasLoaded;
	}

	protected void onLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		if (lastLoadedSceneBuildIndex != -1)
		{
			Profile.SavePermanentStatesByPosition(lastLoadedSceneBuildIndex, deadPositionsInCurrentScene);
			deadPositionsInCurrentScene.Clear();
		}
		loadPermanentStates();
	}

	protected void loadPermanentStates()
	{
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		Profile.LoadPermanentStatesByPosition(buildIndex, deadPositionsInCurrentScene);
		lastLoadedSceneBuildIndex = buildIndex;
	}
}
