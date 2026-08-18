using FMODUnity;
using UnityEngine;

public class PlaySFXOnAwake : MonoBehaviour
{
	[SerializeField]
	[EventRef]
	protected string fmodEvent;

	private void Awake()
	{
		SFX.PlayAudioClipHere(fmodEvent, base.transform.position);
	}
}
