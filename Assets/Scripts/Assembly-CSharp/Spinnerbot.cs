using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Spinnerbot : NavigatingMonster
{
	[SerializeField]
	[EventRef]
	protected string sfx_spinLoop;

	protected EventInstance spinloopFMODEventInstance;

	protected PARAMETER_ID spinLoopInWaterParamID;

	protected PARAMETER_ID spinLoopEndParamID;

	protected override bool removeZTargetOnDeath
	{
		get
		{
			return false;
		}
	}

	protected override void monster_Start()
	{
		spinloopFMODEventInstance = SFX.GetInstance(sfx_spinLoop, base.transform, cachedRigidbody);
		spinLoopInWaterParamID = SFX.GetParamID(spinloopFMODEventInstance, "in_water");
		spinLoopEndParamID = SFX.GetParamID(spinloopFMODEventInstance, "end");
	}

	protected override void monster_OnDestroy()
	{
		spinloopFMODEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		spinloopFMODEventInstance.release();
	}

	protected override void LocomotingBehaviourUpdate()
	{
		if (PlayerCharacter.Instanced)
		{
			base.locomotionTarget = PlayerCharacter.WorldPosition + PlayerCharacter.Forward * 1f;
			base.facingTarget = base.transform.position + base.desiredDirection;
		}
	}

	protected void LateUpdate()
	{
		if (cachedAnimator.GetFloat("AP_spinSFX") == 1f && !base.Frozen)
		{
			if (!SFX.EventIsPlaying(spinloopFMODEventInstance))
			{
				RuntimeManager.AttachInstanceToGameObject(spinloopFMODEventInstance, base.transform, cachedRigidbody);
				spinloopFMODEventInstance.setParameterByID(spinLoopEndParamID, 0f);
				spinloopFMODEventInstance.start();
			}
			spinloopFMODEventInstance.setParameterByID(spinLoopInWaterParamID, base.InWater ? 1 : 0);
		}
		else if (spinloopFMODEventInstance.isValid() && SFX.EventIsPlaying(spinloopFMODEventInstance))
		{
			spinloopFMODEventInstance.setParameterByID(spinLoopEndParamID, 1f);
		}
	}
}
