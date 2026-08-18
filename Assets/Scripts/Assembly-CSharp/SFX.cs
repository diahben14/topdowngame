using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SFX : MonoBehaviour
{
	protected static float listenerAttenuationMin = 10f;

	protected static float listenerAttenuationMax = 15f;

	protected static bool listenerAttenuationSet;

	protected static bool setupDone;

	public static bool PauseAudioBus
	{
		set
		{
			RuntimeManager.GetBus("bus:/sfx/main/pause").setPaused(value);
		}
	}

	public static void SetFMODListenerAttenuationMinMax(float min, float max)
	{
		listenerAttenuationMin = min;
		listenerAttenuationMax = max;
		listenerAttenuationSet = true;
	}

	public static float customRolloffCallback(IntPtr channelcontrol, float distance)
	{
		UnityEngine.Debug.Log("--------------- called custom rolloff function --------------");
		return Mathf.Clamp01(Mathf.InverseLerp(listenerAttenuationMax, listenerAttenuationMin, distance));
	}

	public static void SetupIfNecessary()
	{
		if (!setupDone)
		{
			RESULT rESULT = RuntimeManager.CoreSystem.set3DRolloffCallback(customRolloffCallback);
			if (rESULT > RESULT.OK)
			{
				UnityEngine.Debug.LogError("[FMOD] Tried to set custom 3D rolloff and got error " + rESULT);
			}
			else
			{
				UnityEngine.Debug.Log("[SFX] Set up custom 3D rolloff; result was " + rESULT);
			}
			setupDone = true;
		}
	}

	internal static bool EventIsPlaying(EventInstance spinloopFMODEventInstance)
	{
		PLAYBACK_STATE state;
		spinloopFMODEventInstance.getPlaybackState(out state);
		return state != PLAYBACK_STATE.STOPPED;
	}

	public static void PlayAudioClipAtFox(string fmodEvent)
	{
		if (PlayerCharacter.Instanced)
		{
			PlayAudioClipHere(fmodEvent, PlayerCharacter.WorldPosition);
		}
		else
		{
			PlayAudioClip(fmodEvent);
		}
	}

	public static void PlayAudioClip(string fmodEvent)
	{
		playAudioClip(fmodEvent, Vector3.zero);
	}

	public static void PlayAudioClipHere(string fmodEvent, Vector3 position)
	{
		playAudioClip(fmodEvent, position);
	}

	protected static void playAudioClip(string fmodEvent, Vector3 position)
	{
		SetupIfNecessary();
		if (validateFmodEvent(fmodEvent))
		{
			try
			{
				RuntimeManager.PlayOneShot(fmodEvent, position);
			}
			catch (EventNotFoundException)
			{
			}
		}
	}

	public static EventInstance GetInstance(string fmodEvent, Transform attachTransform, Rigidbody attachRigidbody)
	{
		SetupIfNecessary();
		EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEvent);
		RuntimeManager.AttachInstanceToGameObject(eventInstance, attachTransform, attachRigidbody);
		return eventInstance;
	}

	public static PARAMETER_ID GetParamID(EventInstance eventInstance, string paramName)
	{
		EventDescription description;
		eventInstance.getDescription(out description);
		PARAMETER_DESCRIPTION parameter;
		description.getParameterDescriptionByName(paramName, out parameter);
		return parameter.id;
	}

	public static void playAudioClipWithParam(string fmodEvent, Vector3 position, string param, float paramValue)
	{
		SetupIfNecessary();
		if (validateFmodEvent(fmodEvent))
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEvent);
			PARAMETER_ID paramID = GetParamID(eventInstance, param);
			eventInstance.setParameterByID(paramID, paramValue);
			VECTOR position2 = default(VECTOR);
			position2.x = position.x;
			position2.y = position.y;
			position2.z = position.z;
			VECTOR velocity = default(VECTOR);
			velocity.x = (velocity.y = (velocity.z = 0f));
			VECTOR up = default(VECTOR);
			up.x = 0f;
			up.y = 1f;
			up.z = 0f;
			VECTOR forward = default(VECTOR);
			forward.x = 0f;
			forward.y = 0f;
			forward.z = 1f;
			ATTRIBUTES_3D attributes = default(ATTRIBUTES_3D);
			attributes.position = position2;
			attributes.velocity = velocity;
			attributes.up = up;
			attributes.forward = forward;
			eventInstance.set3DAttributes(attributes);
			eventInstance.start();
			eventInstance.release();
			eventInstance.clearHandle();
		}
	}

	protected static bool validateFmodEvent(string fmodEvent)
	{
		if (string.IsNullOrEmpty(fmodEvent))
		{
			UnityEngine.Debug.LogWarning("[SFX] empty event string - check stack trace to find out what doesn't have its event set.");
			return false;
		}
		return true;
	}
}
