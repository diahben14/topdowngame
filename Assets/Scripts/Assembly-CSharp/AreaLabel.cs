using UnityEngine;

public class AreaLabel : MonoBehaviour
{
	[SerializeField]
	protected MixedTextBuilder topLine;

	[SerializeField]
	protected MixedTextBuilder bottomLine;

	protected static AreaLabel instance;

	protected Animator cachedAnimator;

	public static void ShowLabel(AreaData area)
	{
		instance.showLabel(area);
	}

	protected void showLabel(AreaData area)
	{
		topLine.InputLanguageLine = area.topLine;
		cachedAnimator.SetTrigger("show");
	}

	private void Start()
	{
		instance = this;
		cachedAnimator = GetComponent<Animator>();
	}
}
