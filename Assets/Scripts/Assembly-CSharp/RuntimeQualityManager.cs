using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class RuntimeQualityManager : MonoBehaviour
{
	protected enum QualityBlurSetting
	{
		NONE = 0,
		VIGNETTE = 1,
		DOF = 2
	}

	[SerializeField]
	protected GUISkin skin;

	[SerializeField]
	protected bool Debug_HQ;

	protected static RenderTexture _worldRenderTexture;

	protected static List<Camera> worldRTexCameras = new List<Camera>();

	protected static List<RuntimeCameraQualityController> runtimeCameraQualityControllers = new List<RuntimeCameraQualityController>();

	protected static readonly Vector2[] resolutions = new Vector2[5]
	{
		new Vector2(2880f, 1620f),
		new Vector2(1920f, 1080f),
		new Vector2(1280f, 720f),
		new Vector2(1024f, 576f),
		new Vector2(640f, 360f)
	};

	protected static readonly float[] DOFMaxBlurDistance = new float[5] { 4f, 4f, 4f, 0.15f, 0.15f };

	protected static int _gameRenderQuality = 1;

	protected static QualityBlurSetting _blurSetting = QualityBlurSetting.DOF;

	protected static bool _ssao = true;

	protected Vector2 scrollPosition;

	public const string kThruShadowOffsetProfileString = "thruShadowOffset";

	protected static Material atsm;

	public static bool DebugMenuOpen { get; set; }

	public static RenderTexture WorldRenderTexture
	{
		get
		{
			if (_worldRenderTexture == null || !_worldRenderTexture.IsCreated())
			{
				_worldRenderTexture = new RenderTexture((int)resolutions[gameRenderQuality].x, (int)resolutions[gameRenderQuality].y, 24);
				_worldRenderTexture.format = RenderTextureFormat.DefaultHDR;
				_worldRenderTexture.filterMode = FilterMode.Bilinear;
				_worldRenderTexture.name = "World Render Texture " + Random.Range(0, 999);
				_worldRenderTexture.Create();
				Debug.LogFormat("[World RTex] Creating render texture {0} x {1} x {2}", _worldRenderTexture.width, _worldRenderTexture.height, _worldRenderTexture.format);
			}
			return _worldRenderTexture;
		}
	}

	public static bool ShowFPS { get; set; }

	protected static int gameRenderQuality
	{
		get
		{
			return _gameRenderQuality;
		}
		set
		{
			if (_gameRenderQuality != value)
			{
				_gameRenderQuality = value;
				regenerateWorldRenderTexture();
			}
		}
	}

	protected static QualityBlurSetting Blur
	{
		get
		{
			return _blurSetting;
		}
		set
		{
			_blurSetting = value;
			for (int i = 0; i < runtimeCameraQualityControllers.Count; i++)
			{
				RuntimeCameraQualityController runtimeCameraQualityController = runtimeCameraQualityControllers[i];
				switch (value)
				{
				case QualityBlurSetting.NONE:
					if ((bool)runtimeCameraQualityController.dof)
					{
						runtimeCameraQualityController.dof.enabled = false;
					}
					if ((bool)runtimeCameraQualityController.vignette)
					{
						runtimeCameraQualityController.vignette.enabled = false;
					}
					break;
				case QualityBlurSetting.VIGNETTE:
					if ((bool)runtimeCameraQualityController.dof)
					{
						runtimeCameraQualityController.dof.enabled = false;
					}
					if ((bool)runtimeCameraQualityController.vignette)
					{
						runtimeCameraQualityController.vignette.enabled = true;
					}
					break;
				case QualityBlurSetting.DOF:
					if ((bool)runtimeCameraQualityController.dof)
					{
						runtimeCameraQualityController.dof.enabled = true;
					}
					if ((bool)runtimeCameraQualityController.vignette)
					{
						runtimeCameraQualityController.vignette.enabled = false;
					}
					break;
				}
			}
		}
	}

	protected static bool SSAO
	{
		get
		{
			return _ssao;
		}
		set
		{
			_ssao = value;
			for (int i = 0; i < runtimeCameraQualityControllers.Count; i++)
			{
				RuntimeCameraQualityController runtimeCameraQualityController = runtimeCameraQualityControllers[i];
				if ((bool)runtimeCameraQualityController.ssao)
				{
					runtimeCameraQualityController.ssao.enabled = value;
				}
			}
		}
	}

	public static Material AdjustableThruShadowMaterial
	{
		get
		{
			return atsm;
		}
		set
		{
			if (atsm == null)
			{
				atsm = value;
				int num = PlayerPrefs.GetInt("thruShadowOffset");
				if (num == 0)
				{
					num = -400000;
				}
				AdjustableThruShadowMaterial.SetInt("_Offset", num);
				Debug.LogFormat("[Runtime Quality Manager] Thrushadow material was null... setting it from disk; value is {0}", num);
			}
			else
			{
				int @int = atsm.GetInt("_Offset");
				atsm = value;
				atsm.SetInt("_Offset", @int);
				Debug.LogFormat("[Runtime Quality Manager] Thrushadow material changed; old offset maintained: {0}", @int);
			}
		}
	}

	public static void RegisterWorldRTexCamera(Camera camera)
	{
		if (!worldRTexCameras.Contains(camera))
		{
			worldRTexCameras.Add(camera);
		}
		camera.targetTexture = WorldRenderTexture;
	}

	public static void DeregisterWorldRTexCamera(Camera camera)
	{
		if (worldRTexCameras.Contains(camera))
		{
			worldRTexCameras.Remove(camera);
		}
	}

	public static void RegisterQualityController(RuntimeCameraQualityController rcqc)
	{
		if (!runtimeCameraQualityControllers.Contains(rcqc))
		{
			runtimeCameraQualityControllers.Add(rcqc);
		}
	}

	public static void DeregisterQualityController(RuntimeCameraQualityController rcqc)
	{
		if (runtimeCameraQualityControllers.Contains(rcqc))
		{
			runtimeCameraQualityControllers.Remove(rcqc);
		}
	}

	protected static void regenerateWorldRenderTexture()
	{
		for (int i = 0; i < worldRTexCameras.Count; i++)
		{
			worldRTexCameras[i].targetTexture = null;
		}
		_worldRenderTexture.Release();
		_worldRenderTexture = null;
		for (int j = 0; j < worldRTexCameras.Count; j++)
		{
			worldRTexCameras[j].targetTexture = WorldRenderTexture;
			DepthOfField component = worldRTexCameras[j].GetComponent<DepthOfField>();
			if ((bool)component)
			{
				component.maxBlurSize = DOFMaxBlurDistance[gameRenderQuality];
			}
		}
	}

	protected void Awake()
	{
		regenerateWorldRenderTexture();
		SceneManager.sceneLoaded += onLevelWasLoaded;
	}

	protected void Update()
	{
		if (Debug_HQ)
		{
			gameRenderQuality = 0;
			QualitySettings.SetQualityLevel(3, true);
			QualitySettings.vSyncCount = 0;
			Cheats.GUIHidden = true;
		}
	}

	protected void onLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		SSAO = SSAO;
		Blur = Blur;
	}

	protected void OnApplicationQuit()
	{
		for (int i = 0; i < worldRTexCameras.Count; i++)
		{
			worldRTexCameras[i].targetTexture = null;
		}
	}
}
