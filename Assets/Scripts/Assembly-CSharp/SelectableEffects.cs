using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableEffects : MonoBehaviour
{
	[EventRef]
	[SerializeField]
	protected string onSelectSFX;

	[EventRef]
	[SerializeField]
	protected string onClickSFX;

	protected void Awake()
	{
		EventTrigger eventTrigger = base.gameObject.AddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.Select;
		entry.callback.AddListener(delegate
		{
			onSelect();
		});
		eventTrigger.triggers.Add(entry);
		EventTrigger eventTrigger2 = base.gameObject.AddComponent<EventTrigger>();
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerClick;
		entry2.callback.AddListener(delegate
		{
			onClick();
		});
		eventTrigger2.triggers.Add(entry2);
	}

	protected void onSelect()
	{
		if (Time.timeSinceLevelLoad != 0f)
		{
			SFX.PlayAudioClip(onSelectSFX);
		}
	}

	protected void onClick()
	{
		SFX.PlayAudioClip(onClickSFX);
	}
}
