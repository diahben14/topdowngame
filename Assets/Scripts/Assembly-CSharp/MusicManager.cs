using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public static class MusicManager
{
	private static bool _muted = false;

	private static float musicFadeFactor = 1f;

	private static float targetMusicFade = 1f;

	private static CoroutineRunner fadeCoroutineRunner;

	private static bool fadeCoroutineRunning;

	private static string playingEventString;

	private static EventInstance fmodEventInstance;

	public static bool Muted
	{
		get
		{
			return _muted;
		}
		set
		{
			if (_muted != value)
			{
				_muted = value;
				updateVolume();
			}
		}
	}

	public static void OnMusicControlZoneChange()
	{
		if (MusicControlZone.PlayerInMuteZone)
		{
			targetMusicFade = 0f;
			startFadeCoroutine();
		}
		else
		{
			targetMusicFade = 1f;
			startFadeCoroutine();
		}
	}

	private static void startFadeCoroutine()
	{
		if (fadeCoroutineRunner == null)
		{
			fadeCoroutineRunner = new GameObject("music fade coroutine runner").AddComponent<CoroutineRunner>();
		}
		fadeCoroutineRunner.StartCoroutine(fadeCoroutine());
	}

	private static IEnumerator fadeCoroutine()
	{
		fadeCoroutineRunning = true;
		while (true)
		{
			if (targetMusicFade > musicFadeFactor)
			{
				musicFadeFactor += Time.deltaTime;
			}
			else if (targetMusicFade < musicFadeFactor)
			{
				musicFadeFactor -= Time.deltaTime;
			}
			musicFadeFactor = Mathf.Clamp01(musicFadeFactor);
			updateVolume();
			if (musicFadeFactor == targetMusicFade)
			{
				break;
			}
			yield return 0;
		}
		fadeCoroutineRunning = false;
	}

	private static void updateVolume()
	{
		float volume = musicFadeFactor;
		if (Muted)
		{
			volume = 0f;
		}
		fmodEventInstance.setVolume(volume);
	}

	public static void StopCurrentAndStartNew(string fmodEventString)
	{
		if (playingEventString != fmodEventString)
		{
			Stop();
			if (!string.IsNullOrEmpty(fmodEventString))
			{
				fmodEventInstance = RuntimeManager.CreateInstance(fmodEventString);
				updateVolume();
				fmodEventInstance.start();
				playingEventString = fmodEventString;
			}
		}
	}

	public static void Stop()
	{
		fmodEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		playingEventString = string.Empty;
	}

	public static void StopImmediate()
	{
		fmodEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		playingEventString = string.Empty;
	}
}
