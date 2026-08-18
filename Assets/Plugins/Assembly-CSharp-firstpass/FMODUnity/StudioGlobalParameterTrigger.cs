using FMOD;
using FMOD.Studio;
using UnityEngine;

namespace FMODUnity
{
	[AddComponentMenu("FMOD Studio/FMOD Studio Global Parameter Trigger")]
	public class StudioGlobalParameterTrigger : EventHandler
	{
		[ParamRef]
		public string parameter;

		public EmitterGameEvent TriggerEvent;

		public float value;

		private PARAMETER_DESCRIPTION parameterDescription;

		public PARAMETER_DESCRIPTION ParameterDesctription
		{
			get
			{
				return parameterDescription;
			}
		}

		private RESULT Lookup()
		{
			return RuntimeManager.StudioSystem.getParameterDescriptionByName(parameter, out parameterDescription);
		}

		private void Awake()
		{
			if (string.IsNullOrEmpty(parameterDescription.name))
			{
				Lookup();
			}
		}

		protected override void HandleGameEvent(EmitterGameEvent gameEvent)
		{
			if (TriggerEvent == gameEvent)
			{
				TriggerParameters();
			}
		}

		public void TriggerParameters()
		{
			if (!string.IsNullOrEmpty(parameter))
			{
				RESULT rESULT = RuntimeManager.StudioSystem.setParameterByID(parameterDescription.id, value);
				if (rESULT != 0)
				{
					UnityEngine.Debug.LogError(string.Format("[FMOD] StudioGlobalParameterTrigger failed to set parameter {0} : result = {1}", parameter, rESULT));
				}
			}
		}
	}
}
