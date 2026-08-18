using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class FMODEventPlayableBehavior : PlayableBehaviour
{
	public string eventName;

	public STOP_MODE stopType;

	public ParamRef[] parameters = new ParamRef[0];

	public GameObject TrackTargetObject;

	public TimelineClip OwningClip;

	private bool isPlayheadInside;

	private EventInstance eventInstance;

	protected void PlayEvent()
	{
		if (string.IsNullOrEmpty(eventName))
		{
			return;
		}
		eventInstance = RuntimeManager.CreateInstance(eventName);
		if (Application.isPlaying && (bool)TrackTargetObject)
		{
			Rigidbody component = TrackTargetObject.GetComponent<Rigidbody>();
			if ((bool)component)
			{
				RuntimeManager.AttachInstanceToGameObject(eventInstance, TrackTargetObject.transform, component);
			}
			else
			{
				RuntimeManager.AttachInstanceToGameObject(eventInstance, TrackTargetObject.transform, TrackTargetObject.GetComponent<Rigidbody2D>());
			}
		}
		else
		{
			eventInstance.set3DAttributes(Vector3.zero.To3DAttributes());
		}
		ParamRef[] array = parameters;
		foreach (ParamRef paramRef in array)
		{
			eventInstance.setParameterByID(paramRef.ID, paramRef.Value);
		}
		eventInstance.start();
	}

	public void OnEnter()
	{
		if (!isPlayheadInside)
		{
			PlayEvent();
			isPlayheadInside = true;
		}
	}

	public void OnExit()
	{
		if (!isPlayheadInside)
		{
			return;
		}
		if (eventInstance.isValid())
		{
			if (stopType != STOP_MODE.None)
			{
				eventInstance.stop((stopType == STOP_MODE.Immediate) ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			eventInstance.release();
		}
		isPlayheadInside = false;
	}

	public void UpdateBehaviour(float time)
	{
		if ((double)time >= OwningClip.start && (double)time < OwningClip.end)
		{
			OnEnter();
		}
		else
		{
			OnExit();
		}
	}

	public override void OnGraphStop(Playable playable)
	{
		isPlayheadInside = false;
		if (eventInstance.isValid())
		{
			eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			eventInstance.release();
			RuntimeManager.StudioSystem.update();
		}
	}
}
