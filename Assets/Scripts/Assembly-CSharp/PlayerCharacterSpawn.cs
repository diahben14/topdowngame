using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class PlayerCharacterSpawn : MonoBehaviour
{
	[Header("Spawn Point")]
	[SerializeField]
	protected float transitionDuration = 0.5f;

	[SerializeField]
	protected string id = "";

	[SerializeField]
	protected bool suppressGUI;

	[SerializeField]
	protected Ladder ladderToArriveOn;

	[SerializeField]
	protected AreaData labelToPlayOnArrival;

	[SerializeField]
	protected bool whiteout;

	[Header("Testing")]
	[SerializeField]
	protected bool useAsTestingSpawn;

	protected static readonly string gizmoIconName = "Player Spawn Location Icon.psd";

	protected static readonly string gizmoTestSpawnIconName = "Player Spawn Location Icon Testing.psd";

	protected static readonly Color gizmoColor = new Color(0f, 1f, 0f, 0.75f);

	protected static readonly Quaternion gizmoRotation = Quaternion.Euler(-180f, 0f, 90f);

	protected static readonly float size = 2.5f;

	private static string portalIDToSpawnAt;

	protected static bool sceneLoadingViaSpawn;

	protected static bool spawnPointFound;

	protected static bool respawnAtLastSpawnPoint;

	public static bool ArrivalLoadingSpinlock { get; protected set; }

	protected virtual bool doCameraSweepOnSpawn
	{
		get
		{
			return false;
		}
	}

	protected virtual Transform playerSpawnTransform
	{
		get
		{
			return base.transform;
		}
	}

	public static float SceneTransitionFade { get; set; }

	public virtual string FullID
	{
		get
		{
			return id;
		}
	}

	public static string GetCurrentSceneName()
	{
		return SceneManager.GetActiveScene().name;
	}

	public static void GoToTestingSpawn()
	{
		Debug.Log(string.Format("[Player Character Spawn] going to testing spawn"));
		portalIDToSpawnAt = "";
		spawnPointFound = false;
		sceneLoadingViaSpawn = false;
		respawnAtLastSpawnPoint = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void GoTo(string destinationSceneName, string destinationID)
	{
		Debug.Log(string.Format("[Player Character Spawn] going to {0}, {1}", destinationSceneName, destinationID));
		portalIDToSpawnAt = destinationID;
		spawnPointFound = false;
		sceneLoadingViaSpawn = true;
		respawnAtLastSpawnPoint = false;
		SceneManager.LoadScene(destinationSceneName);
	}

	protected bool itsMe()
	{
		if (!spawnPointFound)
		{
			if (!sceneLoadingViaSpawn || !(portalIDToSpawnAt == FullID))
			{
				if (Application.isEditor && !sceneLoadingViaSpawn && (Time.time == 0f || respawnAtLastSpawnPoint))
				{
					return useAsTestingSpawn;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private void Start()
	{
		if (Application.isPlaying && itsMe())
		{
			Debug.LogFormat("[SPAWN] Spawn found name:{0}; id:{1} ", base.name, FullID);
			Profile.SetLastSpawn(SceneManager.GetActiveScene().name, FullID);
			PlayerCharacter.SpawnAt(playerSpawnTransform);
			if ((bool)ladderToArriveOn)
			{
				ladderToArriveOn.WarpOntoLadder(playerSpawnTransform.position);
			}
			if (!suppressGUI)
			{
				GUIMode.ClearStack();
				GUIMode.PushMode(GUIMode.GetMode<GameMode>());
			}
			else
			{
				GUIMode.ClearStack();
			}
			CameraController.SnapToPlayer();
			PlayerCharacter.CutsceneHidden = false;
			sceneLoadingViaSpawn = false;
			spawnPointFound = true;
			respawnAtLastSpawnPoint = false;
			Debug.Log("[SPAWN] about to fire onArrivalStart");
			onArrivalStart();
			StartCoroutine(transitionSequenceArrive());
		}
	}

	private IEnumerator transitionSequenceArrive()
	{
		ArrivalLoadingSpinlock = true;
		Debug.Log("Locking time for DynamicGI to converge...");
		float t = Time.realtimeSinceStartup;
		while (!DynamicGI.isConverged)
		{
			yield return 0;
		}
		Debug.LogFormat("... DynamicGI converged. Took {0}s.", Time.realtimeSinceStartup - t);
		ArrivalLoadingSpinlock = false;
		CameraController.CutsceneFade = 0f;
		if (doCameraSweepOnSpawn)
		{
			CycleController.DoSweep();
		}
		FullscreenFader.Whiteout = whiteout;
		if (transitionDuration > 0f)
		{
			for (float i = 0f; i < 1f; i += Time.deltaTime / transitionDuration)
			{
				SceneTransitionFade = 1f - i;
				yield return 0;
			}
		}
		SceneTransitionFade = 0f;
		if ((bool)labelToPlayOnArrival)
		{
			AreaLabel.ShowLabel(labelToPlayOnArrival);
		}
		onArrivalDone();
	}

	protected virtual void onArrivalStart()
	{
	}

	protected virtual void onArrivalDone()
	{
	}
}
