using System;
using System.Threading;
using FMOD.Studio;
using UnityEngine;

namespace FMODUnity
{
	[AddComponentMenu("FMOD Studio/FMOD Studio Event Emitter")]
	public class StudioEventEmitter : EventHandler
	{
		[EventRef]
		public string Event = "";

		public EmitterGameEvent PlayEvent;

		public EmitterGameEvent StopEvent;

		public bool AllowFadeout = true;

		public bool TriggerOnce;

		public bool Preload;

		public ParamRef[] Params = new ParamRef[0];

		public bool OverrideAttenuation;

		public float OverrideMinDistance = -1f;

		public float OverrideMaxDistance = -1f;

		protected EventDescription eventDescription;

		protected EventInstance instance;

		private bool hasTriggered;

		private bool isQuitting;

		private bool isOneshot;

		private const string SnapshotString = "snapshot";

		public EventDescription EventDescription
		{
			get
			{
				return eventDescription;
			}
		}

		public EventInstance EventInstance
		{
			get
			{
				return instance;
			}
		}

		private void Start()
		{
			RuntimeUtils.EnforceLibraryOrder();
			if (Preload)
			{
				Lookup();
				eventDescription.loadSampleData();
				RuntimeManager.StudioSystem.update();
				LOADING_STATE state;
				eventDescription.getSampleLoadingState(out state);
				while (state == LOADING_STATE.LOADING)
				{
					Thread.Sleep(1);
					eventDescription.getSampleLoadingState(out state);
				}
			}
			HandleGameEvent(EmitterGameEvent.ObjectStart);
		}

		private void OnApplicationQuit()
		{
			isQuitting = true;
		}

		private void OnDestroy()
		{
			if (isQuitting)
			{
				return;
			}
			HandleGameEvent(EmitterGameEvent.ObjectDestroy);
			if (instance.isValid())
			{
				RuntimeManager.DetachInstanceFromGameObject(instance);
				if (eventDescription.isValid() && isOneshot)
				{
					instance.release();
					instance.clearHandle();
				}
			}
			if (Preload)
			{
				eventDescription.unloadSampleData();
			}
		}

		protected override void HandleGameEvent(EmitterGameEvent gameEvent)
		{
			if (PlayEvent == gameEvent)
			{
				Play();
			}
			if (StopEvent == gameEvent)
			{
				Stop();
			}
		}

		private void Lookup()
		{
			eventDescription = RuntimeManager.GetEventDescription(Event);
			if (eventDescription.isValid())
			{
				for (int i = 0; i < Params.Length; i++)
				{
					PARAMETER_DESCRIPTION parameter;
					eventDescription.getParameterDescriptionByName(Params[i].Name, out parameter);
					Params[i].ID = parameter.id;
				}
			}
		}

		public void Play()
		{
			if ((TriggerOnce && hasTriggered) || string.IsNullOrEmpty(Event))
			{
				return;
			}
			if (!eventDescription.isValid())
			{
				Lookup();
			}
			if (!Event.StartsWith("snapshot", StringComparison.CurrentCultureIgnoreCase))
			{
				eventDescription.isOneshot(out isOneshot);
			}
			bool is3D;
			eventDescription.is3D(out is3D);
			if (!instance.isValid())
			{
				instance.clearHandle();
			}
			if (isOneshot && instance.isValid())
			{
				instance.release();
				instance.clearHandle();
			}
			if (!instance.isValid())
			{
				eventDescription.createInstance(out instance);
				if (is3D)
				{
					Rigidbody component = GetComponent<Rigidbody>();
					Rigidbody2D component2 = GetComponent<Rigidbody2D>();
					Transform component3 = GetComponent<Transform>();
					if ((bool)component)
					{
						instance.set3DAttributes(RuntimeUtils.To3DAttributes(base.gameObject, component));
						RuntimeManager.AttachInstanceToGameObject(instance, component3, component);
					}
					else
					{
						instance.set3DAttributes(RuntimeUtils.To3DAttributes(base.gameObject, component2));
						RuntimeManager.AttachInstanceToGameObject(instance, component3, component2);
					}
				}
			}
			ParamRef[] @params = Params;
			foreach (ParamRef paramRef in @params)
			{
				instance.setParameterByID(paramRef.ID, paramRef.Value);
			}
			if (is3D && OverrideAttenuation)
			{
				instance.setProperty(EVENT_PROPERTY.MINIMUM_DISTANCE, OverrideMinDistance);
				instance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, OverrideMaxDistance);
			}
			instance.start();
			hasTriggered = true;
		}

		public void Stop()
		{
			if (instance.isValid())
			{
				instance.stop((!AllowFadeout) ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				instance.release();
				instance.clearHandle();
			}
		}

		public void SetParameter(string name, float value, bool ignoreseekspeed = false)
		{
			if (instance.isValid())
			{
				instance.setParameterByName(name, value, ignoreseekspeed);
			}
		}

		public void SetParameter(PARAMETER_ID id, float value, bool ignoreseekspeed = false)
		{
			if (instance.isValid())
			{
				instance.setParameterByID(id, value, ignoreseekspeed);
			}
		}

		public bool IsPlaying()
		{
			if (instance.isValid())
			{
				PLAYBACK_STATE state;
				instance.getPlaybackState(out state);
				return state != PLAYBACK_STATE.STOPPED;
			}
			return false;
		}
	}
}
