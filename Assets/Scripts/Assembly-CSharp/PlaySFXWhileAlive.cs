using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class PlaySFXWhileAlive : MonoBehaviour
{
	[SerializeField]
	[EventRef]
	protected string fmodEvent;

	private EventInstance eventInstance;

	protected void Awake()
	{
		eventInstance = RuntimeManager.CreateInstance(fmodEvent);
		eventInstance.start();
		RuntimeManager.AttachInstanceToGameObject(eventInstance, base.transform, GetComponent<Rigidbody>());
	}

	protected void OnDestroy()
	{
		eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		eventInstance.release();
	}
}
