using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NPCDialogue : GUIMode
{
	[SerializeField]
	protected float animationDuration;

	[SerializeField]
	protected MixedTextBuilder dialogueTextBuilder;

	[SerializeField]
	protected GameObject defaultSelection;

	[SerializeField]
	protected float charsPerSecond = 8f;

	protected static NPCDialogue instance;

	protected Animator cachedAnimator;

	protected LanguageLine[] lines;

	protected bool receivedInput;

	protected override bool pauseTime
	{
		get
		{
			return true;
		}
	}

	public static void DisplayDialogue(Image portrait, LanguageLine[] lines)
	{
		GUIMode.PushMode(instance);
		instance.lines = lines;
		EventSystem.current.SetSelectedGameObject(instance.defaultSelection);
	}

	protected override IEnumerator afterEnable()
	{
		cachedAnimator.SetBool("show", true);
		for (int i = 0; i < lines.Length; i++)
		{
			dialogueTextBuilder.InputLanguageLine = lines[i];
			dialogueTextBuilder.CharByCharClip = 0;
			bool skip = false;
			if (charsPerSecond > 0f)
			{
				float duration = (float)dialogueTextBuilder.CharLength / charsPerSecond;
				for (float t = 0f; t < duration; t += TimeManager.UnscaledDeltaTime)
				{
					if (skip)
					{
						break;
					}
					dialogueTextBuilder.CharByCharClip = Mathf.RoundToInt(t * charsPerSecond);
					if (consumeInput())
					{
						skip = true;
					}
					yield return 0;
				}
			}
			dialogueTextBuilder.CharByCharClip = int.MaxValue;
			while (!consumeInput())
			{
				yield return 0;
			}
		}
		GUIMode.PopMode(this);
	}

	protected bool consumeInput()
	{
		bool result = receivedInput;
		receivedInput = false;
		return result;
	}

	public void ButtonPress()
	{
		if (base.ReceiveInput)
		{
			receivedInput = true;
		}
	}

	protected override IEnumerator beforeDisable()
	{
		cachedAnimator.SetBool("show", false);
		for (float f = 0f; f < animationDuration; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
	}

	protected override void guiModeSetup()
	{
		instance = this;
		cachedAnimator = GetComponent<Animator>();
	}
}
