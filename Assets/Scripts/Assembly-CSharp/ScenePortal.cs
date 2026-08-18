using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ScenePortal : PlayerCharacterSpawn
{
	[Header("Scene Portal")]
	[SerializeField]
	protected string destinationSceneName = "";

	[SerializeField]
	protected string optionalIDToSpawnAt = "";

	[SerializeField]
	protected Transform spawnTransform;

	[SerializeField]
	protected bool teleporterStyleDeparture;

	public static bool DepartureSequenceRunning { get; protected set; }

	public static bool DepartureSequencePausingTime { get; protected set; }

	public override string FullID
	{
		get
		{
			return destinationSceneName + "_" + id;
		}
	}

	protected override Transform playerSpawnTransform
	{
		get
		{
			return spawnTransform;
		}
	}

	public static void DepartToScene(MonoBehaviour coroutineRunner, bool whiteout, float transitionDuration, string destinationSceneName, string id, bool pauseTime, float delay)
	{
		coroutineRunner.StartCoroutine(transitionSequenceDepart(whiteout, transitionDuration, destinationSceneName, id, pauseTime, delay));
	}

	protected static IEnumerator transitionSequenceDepart(bool whiteout, float transitionDuration, string destinationSceneName, string id, bool pauseTime, float delay)
	{
		if (DepartureSequenceRunning)
		{
			yield break;
		}
		PlayerCharacter.CancelHyperdash();
		DepartureSequenceRunning = true;
		DepartureSequencePausingTime = pauseTime;
		GUIMode.ClearStack();
		for (float i = 0f; i < 1f; i += TimeManager.UnscaledDeltaTime / delay)
		{
			yield return 0;
		}
		FullscreenFader.Whiteout = whiteout;
		if (transitionDuration > 0f)
		{
			for (float i = 0f; i < 1f; i += TimeManager.UnscaledDeltaTime / transitionDuration)
			{
				PlayerCharacterSpawn.SceneTransitionFade = i;
				yield return 0;
			}
		}
		PlayerCharacterSpawn.SceneTransitionFade = 1f;
		yield return 0;
		DepartureSequenceRunning = false;
		DepartureSequencePausingTime = false;
		TimeManager.UpdateTimeScale();
		PlayerCharacterSpawn.GoTo(destinationSceneName, PlayerCharacterSpawn.GetCurrentSceneName() + "_" + id);
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!(Time.timeSinceLevelLoad > 0f))
		{
			return;
		}
		PlayerCharacter component = c.gameObject.GetComponent<PlayerCharacter>();
		if (component != null && !component.InConduit)
		{
			string text = ((optionalIDToSpawnAt == "") ? id : optionalIDToSpawnAt);
			if (teleporterStyleDeparture)
			{
				PlayerCharacter.CutsceneHidden = true;
				DepartToScene(this, whiteout, transitionDuration, destinationSceneName, text, false, 2f);
			}
			else
			{
				DepartToScene(this, whiteout, transitionDuration, destinationSceneName, text, true, 0f);
			}
		}
	}

	protected override void onArrivalStart()
	{
		bool teleporterStyleDeparture2 = teleporterStyleDeparture;
	}
}
