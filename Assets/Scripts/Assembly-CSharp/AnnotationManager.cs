using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AnnotationManager : MonoBehaviour
{
	protected static readonly float kFadeDuration = 0.25f;

	protected static AnnotationManager instance;

	protected static Dictionary<Annotation, TextMeshProUGUI> annotationBindings = new Dictionary<Annotation, TextMeshProUGUI>();

	protected static readonly HideFlags UIHideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;

	protected static bool _designerMode = false;

	protected RectTransform cachedRectTransform;

	protected static readonly string kPathToAnnotationTextPrefab = "AnnotationText";

	public static bool DesignerMode
	{
		get
		{
			return _designerMode;
		}
		set
		{
			_designerMode = value;
		}
	}

	public static void Register(Annotation a)
	{
		setup();
		if (!annotationBindings.ContainsKey(a))
		{
			annotationBindings.Add(a, null);
		}
	}

	public static void Deregister(Annotation a)
	{
		setup();
		if (annotationBindings.ContainsKey(a))
		{
			TextMeshProUGUI textMeshProUGUI = annotationBindings[a];
			if (textMeshProUGUI != null)
			{
				Object.Destroy(textMeshProUGUI.gameObject);
			}
			annotationBindings.Remove(a);
		}
	}

	protected static void setup()
	{
		if (instance == null)
		{
			GameObject obj = new GameObject("___AnnotationCanvas");
			obj.layer = LayerMask.NameToLayer("UI");
			obj.hideFlags = UIHideFlags;
			obj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
			obj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			instance = obj.AddComponent<AnnotationManager>();
		}
	}

	protected void OnEnable()
	{
		cachedRectTransform = GetComponent<RectTransform>();
	}

	protected static void updateText(Annotation annotation)
	{
		TextMeshProUGUI textFromAnnotation = getTextFromAnnotation(annotation);
		annotationBindings[annotation] = textFromAnnotation;
		textFromAnnotation.text = annotation.Message;
	}

	protected static void updatePosition(Annotation annotation)
	{
		getTextFromAnnotation(annotation).transform.localPosition = instance.transformPosition(annotation.transform.position);
	}

	protected static bool shouldShow(Annotation annotation)
	{
		if (annotation.DesignerOnly && !DesignerMode)
		{
			return false;
		}
		if (PageDisplay.Open)
		{
			return false;
		}
		if (InventoryDisplay.InventoryOpen)
		{
			return false;
		}
		if (!GUIMode.InGameGUIMode)
		{
			return false;
		}
		if (!annotation.OnlyShowWhenPlayerIsNear)
		{
			return true;
		}
		return Vector3.Distance(annotation.transform.position, PlayerCharacter.WorldPosition) < annotation.ShowDistance;
	}

	public static TextMeshProUGUI getTextFromAnnotation(Annotation a)
	{
		TextMeshProUGUI textMeshProUGUI = annotationBindings[a];
		if (textMeshProUGUI == null)
		{
			textMeshProUGUI = instance.makeText();
			annotationBindings[a] = textMeshProUGUI;
			updateText(a);
		}
		return textMeshProUGUI;
	}

	protected Vector3 transformPosition(Vector3 worldPos)
	{
		Vector3 a = CameraController.WorldToViewportPoint(worldPos);
		a.x -= 0.5f;
		a.y -= 0.5f;
		return Vector3.Scale(a, cachedRectTransform.sizeDelta);
	}

	protected TextMeshProUGUI makeText()
	{
		GameObject obj = (GameObject)Object.Instantiate(Resources.Load(kPathToAnnotationTextPrefab));
		obj.hideFlags = UIHideFlags;
		obj.transform.SetParent(base.transform);
		obj.transform.localScale = Vector3.one;
		TextMeshProUGUI component = obj.GetComponent<TextMeshProUGUI>();
		if (component == null)
		{
			Debug.LogError("No TMP object in instantiated prefab!");
		}
		if (Application.isPlaying)
		{
			component.CrossFadeAlpha(0f, 0f, true);
		}
		return component;
	}
}
