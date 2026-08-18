using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelector : MonoBehaviour
{
	[SerializeField]
	protected float smoothTime = 0.125f;

	[SerializeField]
	protected float tiltFactor = 1f;

	[SerializeField]
	protected float speedForMaxScaling = 1f;

	[SerializeField]
	protected float scaleFactor = 1.1f;

	protected RectTransform rectTransform;

	protected Image image;

	protected Vector3 velocity;

	protected GUIMode guiMode;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		image = GetComponent<Image>();
		guiMode = GetComponentInParent<GUIMode>();
	}

	private void OnEnable()
	{
		image.enabled = false;
	}

	private void Update()
	{
		EventSystem current = EventSystem.current;
		if (current != null)
		{
			GameObject currentSelectedGameObject = current.currentSelectedGameObject;
			if (currentSelectedGameObject != null && currentSelectedGameObject.activeInHierarchy && guiMode != null && guiMode.ReceiveInput)
			{
				image.enabled = true;
				Vector3 position = currentSelectedGameObject.transform.position;
				Vector3 position2 = Vector3.SmoothDamp(rectTransform.position, position, ref velocity, smoothTime, float.PositiveInfinity, TimeManager.UnscaledDeltaTime);
				rectTransform.position = position2;
				rectTransform.localRotation = Quaternion.Euler(0f, 0f, velocity.x * (0f - tiltFactor));
				float t = Mathf.InverseLerp(0f, speedForMaxScaling, velocity.magnitude);
				rectTransform.localScale = Vector3.one * Mathf.Lerp(1f, scaleFactor, t);
			}
			else
			{
				image.enabled = false;
			}
		}
		else
		{
			image.enabled = false;
		}
	}
}
