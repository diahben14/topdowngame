using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class FMODEventPlayable : PlayableAsset, ITimelineClipAsset
{
	public FMODEventPlayableBehavior template = new FMODEventPlayableBehavior();

	public float eventLength;

	private FMODEventPlayableBehavior behavior;

	[EventRef]
	[SerializeField]
	public string eventName;

	[SerializeField]
	public STOP_MODE stopType;

	[SerializeField]
	public ParamRef[] parameters = new ParamRef[0];

	[NonSerialized]
	public bool cachedParameters;

	public GameObject TrackTargetObject { get; set; }

	public override double duration
	{
		get
		{
			if (eventName == null)
			{
				return base.duration;
			}
			return eventLength;
		}
	}

	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.None;
		}
	}

	public TimelineClip OwningClip { get; set; }

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		if (!cachedParameters && !string.IsNullOrEmpty(eventName))
		{
			EventDescription _event;
			RuntimeManager.StudioSystem.getEvent(eventName, out _event);
			for (int i = 0; i < parameters.Length; i++)
			{
				PARAMETER_DESCRIPTION parameter;
				_event.getParameterDescriptionByName(parameters[i].Name, out parameter);
				parameters[i].ID = parameter.id;
			}
			cachedParameters = true;
		}
		ScriptPlayable<FMODEventPlayableBehavior> scriptPlayable = ScriptPlayable<FMODEventPlayableBehavior>.Create(graph, template);
		behavior = scriptPlayable.GetBehaviour();
		behavior.TrackTargetObject = TrackTargetObject;
		behavior.eventName = eventName;
		behavior.stopType = stopType;
		behavior.parameters = parameters;
		behavior.OwningClip = OwningClip;
		return scriptPlayable;
	}
}
