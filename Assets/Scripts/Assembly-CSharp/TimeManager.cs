using System.Collections;
using InControl;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	[SerializeField]
	protected bool debug_slowMo;

	[SerializeField]
	protected float freezeFrameDuration = 0.5f;

	protected const float kDefaultTimeStep = 0.0166f;

	protected static float _cutsceneTimeScale = 1f;

	protected static TimeManager instance;

	protected InControlManager inControlManager;

	protected bool freeze;

	public static float CutsceneTimeScale
	{
		get
		{
			return _cutsceneTimeScale;
		}
		set
		{
			_cutsceneTimeScale = value;
		}
	}

	public static bool UseFixedUpdateForInput
	{
		get
		{
			return instance.inControlManager.useFixedUpdate;
		}
		set
		{
			if (instance == null)
			{
				Debug.LogError("No TimeManager instance.");
				return;
			}
			if (instance.inControlManager == null)
			{
				InControlManager component = instance.GetComponent<InControlManager>();
				if (component != null)
				{
					instance.inControlManager = component;
				}
				else
				{
					Debug.LogError("TimeManager and InControlManager should be on the same object.");
				}
			}
			instance.inControlManager.useFixedUpdate = value;
		}
	}

	public static bool CameraFreeze
	{
		get
		{
			return false;
		}
	}

	public static float UnscaledDeltaTime
	{
		get
		{
			return Time.unscaledDeltaTime * debugSlowMoFactor;
		}
	}

	protected static float debugSlowMoFactor
	{
		get
		{
			if (!instance.debug_slowMo)
			{
				return 1f;
			}
			return 0.33f;
		}
	}

	public static void FreezeFrame()
	{
		FreezeFrame(instance.freezeFrameDuration);
	}

	public static void FreezeFrame(float duration)
	{
		instance.StartCoroutine(instance.freezeFrame(duration));
	}

	public static void UpdateTimeScale()
	{
		instance.Update();
	}

	protected IEnumerator freezeFrame(float duration)
	{
		yield return new WaitForFixedUpdate();
		for (float f = 0f; f < duration; f += UnscaledDeltaTime)
		{
			freeze = true;
			yield return 0;
		}
		freeze = false;
	}

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		UseFixedUpdateForInput = !InventoryDisplay.InventoryOpen && !GUIMode.PauseTime;
		updateTimeScale();
	}

	private void updateTimeScale()
	{
		float num2 = (Time.timeScale = (RuntimeQualityManager.DebugMenuOpen ? 0f : (ScenePortal.DepartureSequencePausingTime ? 0f : (PlayerCharacterSpawn.ArrivalLoadingSpinlock ? 0f : ((PlayerCharacter.Instanced && PlayerCharacter.InstanceIsDead) ? (freeze ? 0f : 0.75f) : (GUIMode.PauseTime ? 0f : (freeze ? 0.1f : 1f)))))) * debugSlowMoFactor * CutsceneTimeScale * Mathf.Lerp(1f, 0.1f, Mathf.Clamp01(PlayerCharacter.SlowdownEffect)));
		if (num2 == 0f)
		{
			Time.fixedDeltaTime = 0.0166f;
		}
		else
		{
			Time.fixedDeltaTime = num2 * 0.0166f;
		}
	}
}
