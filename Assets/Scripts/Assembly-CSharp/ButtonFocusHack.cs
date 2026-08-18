using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFocusHack : MonoBehaviour
{
	protected GameObject lastSelected;

	protected void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null && lastSelected != null)
		{
			EventSystem.current.SetSelectedGameObject(lastSelected);
		}
		else
		{
			lastSelected = EventSystem.current.currentSelectedGameObject;
		}
	}
}
