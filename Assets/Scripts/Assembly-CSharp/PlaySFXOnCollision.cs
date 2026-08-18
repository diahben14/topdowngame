using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class PlaySFXOnCollision : MonoBehaviour
{
	[SerializeField]
	[EventRef]
	protected string fmodEvent;

	[SerializeField]
	protected string paramterName = "impact_strength";

	[SerializeField]
	protected float minImpactImpulse;

	[SerializeField]
	protected float maxImpactImpulse;

	private EventInstance eventInstance;

	private PARAMETER_ID impactParameterID;

	protected void Awake()
	{
		SFX.SetupIfNecessary();
		eventInstance = RuntimeManager.CreateInstance(fmodEvent);
		EventDescription description;
		eventInstance.getDescription(out description);
		PARAMETER_DESCRIPTION parameter;
		description.getParameterDescriptionByName(paramterName, out parameter);
		RuntimeManager.AttachInstanceToGameObject(eventInstance, base.transform, GetComponent<Rigidbody>());
		impactParameterID = parameter.id;
	}

	protected void OnCollisionEnter(Collision c)
	{
		float magnitude = c.impulse.magnitude;
		float value = Mathf.Lerp(0f, 10f, Mathf.InverseLerp(minImpactImpulse, maxImpactImpulse, magnitude));
		eventInstance.setParameterByID(impactParameterID, value);
		eventInstance.start();
	}

	protected void OnDestroy()
	{
		eventInstance.release();
	}
}
