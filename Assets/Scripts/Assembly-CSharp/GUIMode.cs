using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIMode : MonoBehaviour
{
	private static List<GUIMode> guiModes = new List<GUIMode>();

	private static List<GUIMode> stack = new List<GUIMode>();

	private static MonoBehaviour _coroutineMonobehaviour;

	protected bool setupDone;

	public static bool InGameGUIMode
	{
		get
		{
			if (!(topMode == GetMode<GameMode>()))
			{
				return topMode == null;
			}
			return true;
		}
	}

	public static bool PauseTime
	{
		get
		{
			if (stack.Count > 0 && topMode != null)
			{
				return topMode.pauseTime;
			}
			return false;
		}
	}

	private static GUIMode topMode
	{
		get
		{
			if (stack.Count > 0)
			{
				return stack[stack.Count - 1];
			}
			return null;
		}
	}

	private static MonoBehaviour coroutineGameObject
	{
		get
		{
			if (!_coroutineMonobehaviour)
			{
				_coroutineMonobehaviour = new GameObject("CoroutineMonobehaviour").AddComponent<CoroutineRunner>();
				Object.DontDestroyOnLoad(_coroutineMonobehaviour);
			}
			return _coroutineMonobehaviour;
		}
	}

	protected virtual bool pauseTime
	{
		get
		{
			return false;
		}
	}

	public bool ReceiveInput { get; protected set; }

	public static GUIMode GetMode<guiModeType>() where guiModeType : GUIMode
	{
		for (int i = 0; i < guiModes.Count; i++)
		{
			if (guiModes[i].GetType() == typeof(guiModeType))
			{
				return guiModes[i];
			}
		}
		return null;
	}

	public static void ClearStack()
	{
		Debug.Log("[GUI] Clearing Stack");
		stack.Clear();
		updateActive();
	}

	public static void PushMode(GUIMode gm)
	{
		Debug.Log("[GUI] Push mode " + gm.name);
		stack.Add(gm);
		updateActive();
	}

	public static bool IsCurrent(GUIMode gm)
	{
		return gm == topMode;
	}

	public static void PopMode(GUIMode gm)
	{
		if (topMode == gm)
		{
			Debug.Log("[GUI] Pop mode " + gm.name);
			stack.RemoveAt(stack.Count - 1);
			updateActive();
		}
		else
		{
			Debug.LogErrorFormat("Trying to pop GUIMode {0} but {1} is on the top.", gm.name, topMode.name);
		}
	}

	private static void updateActive()
	{
		for (int i = 0; i < guiModes.Count; i++)
		{
			GUIMode gUIMode = guiModes[i];
			if (topMode == gUIMode)
			{
				coroutineGameObject.StartCoroutine(gUIMode.enableSequence());
				continue;
			}
			gUIMode.ReceiveInput = false;
			if (gUIMode.gameObject.activeSelf)
			{
				gUIMode.StartCoroutine(gUIMode.disableSequence());
			}
		}
	}

	protected static void rebootEventSystem()
	{
		if ((bool)EventSystem.current)
		{
			EventSystem current = EventSystem.current;
			current.enabled = false;
			current.enabled = true;
		}
	}

	protected IEnumerator enableSequence()
	{
		beforeEnable();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (topMode == this)
		{
			base.gameObject.SetActive(true);
			ReceiveInput = true;
			StartCoroutine(afterEnable());
		}
	}

	protected IEnumerator disableSequence()
	{
		if (Time.time > 0f)
		{
			yield return StartCoroutine(beforeDisable());
		}
		if (!IsCurrent(this))
		{
			base.gameObject.SetActive(false);
		}
	}

	protected virtual void beforeEnable()
	{
	}

	protected virtual IEnumerator afterEnable()
	{
		yield break;
	}

	protected virtual IEnumerator beforeDisable()
	{
		yield break;
	}

	private void Awake()
	{
		setupIfNeeded();
	}

	public void setupIfNeeded()
	{
		if (!setupDone)
		{
			setupDone = true;
			guiModes.Add(this);
			guiModeSetup();
			Object.DontDestroyOnLoad(coroutineGameObject);
		}
	}

	protected virtual void guiModeSetup()
	{
	}
}
