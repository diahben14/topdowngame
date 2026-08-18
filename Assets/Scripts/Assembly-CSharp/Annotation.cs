using UnityEngine;

[ExecuteInEditMode]
public class Annotation : MonoBehaviour
{
	[SerializeField]
	public bool OnlyShowWhenPlayerIsNear;

	[SerializeField]
	public float ShowDistance = 16f;

	[SerializeField]
	public bool DesignerOnly = true;

	[TextArea(8, 32)]
	[SerializeField]
	protected string message = "New Annotation";

	public string Message
	{
		get
		{
			return message;
		}
		set
		{
			if (Application.isPlaying)
			{
				runtimeDirtyText = true;
			}
			message = value;
		}
	}

	public bool runtimeDirtyText { get; set; }

	public bool IsVisible { get; set; }

	protected void OnEnable()
	{
		AnnotationManager.Register(this);
	}

	protected void OnDisable()
	{
		AnnotationManager.Deregister(this);
	}
}
