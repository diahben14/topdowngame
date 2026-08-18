using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
	[SerializeField]
	protected bool freeControl;

	[SerializeField]
	protected CameraBehaviourData cameraBehaviourData;

	[SerializeField]
	protected CameraBehaviourData[] extraCameraBehaviourData;

	[SerializeField]
	protected bool useMask;

	[SerializeField]
	protected float maskTransitionTime = 0.2f;

	[SerializeField]
	protected float maskMaxFadeAmount = 0.2f;

	[SerializeField]
	protected float maxSweepOrthoSize = 50f;

	[SerializeField]
	protected Vector3 sweepRotation = new Vector3(0f, 0f, 0f);

	[SerializeField]
	protected Vector3 sweepOffsetVector;

	[SerializeField]
	protected Camera perspectiveCamera;

	[SerializeField]
	protected Camera effectsCamera;

	[SerializeField]
	protected float sweepDistanceBoost;

	[SerializeField]
	protected Transform audioListener;

	[SerializeField]
	protected Vector2 inventoryCameraPositionOffset = new Vector2(1.6f, -1.6f);

	[SerializeField]
	protected float inventoryCameraPositionOffsetExponent = 1.3f;

	[Header("Asset References")]
	[Header("Animatable")]
	[Range(0f, 1f)]
	[SerializeField]
	protected float sweepParameter;

	protected const float kStaminaShowChangeRate = 5f;

	protected const float playerVelocityScale = 0.25f;

	protected const float zTargettingZoomAmountMinimum = 0f;

	protected const float zTargetEffectsTransitionTime = 0.25f;

	protected const float inventoryOpenZoomAmount = 0f;

	protected const float inventoryOpenPositionSmoothingTime = 0.1f;

	protected const float inventoryOrthoSizeDampeningFactor = 1E-05f;

	protected const float cameraShakeDampening = 0.1f;

	protected const float fastCameraShakeReduction = 10f;

	protected const float damageEffectCoolDuration = 1f;

	protected const float damageZoomScale = 3f;

	protected const float inventoryFadeAmount = 0f;

	protected const int kMaskRTexDownsample = 2;

	protected const float sfxFixedListenerHeight = 8f;

	protected const float sfxAttenuationBandWidth = 5f;

	protected static bool warpCameraNextFrame;

	protected static bool updateInvoked;

	protected static CameraController instance;

	protected Camera cachedCamera;

	protected CameraBehaviourDataStack cameraBehaviourDataStack = new CameraBehaviourDataStack();

	protected Vector3 velocityRef;

	protected float zTargetFactor;

	protected float screenshakeCapacitor;

	protected float fastScreenshakeCapacitor;

	protected float damageEffectCapacitor;

	protected float lastParryTime = float.NegativeInfinity;

	protected float smoothedOrthoSize;

	protected float smoothedNearClipPlane;

	protected float smoothedFarClipPlane;

	protected float smoothedCameraDistance;

	protected Vector3 localShakeOffset;

	protected float pivotRotationParameter;

	protected Vector3 previousSmoothedTargetPosition;

	protected Vector3 previousDesiredCameraPosition;

	protected float zTargetFactorVelocityRef;

	protected float smoothedYRotation;

	protected float smoothedYRotationVelocity;

	protected float smoothedXRotation;

	protected float zTargetSmoothedXRotation;

	protected float smoothedXRotationVelocity;

	protected float zTargetSmoothedXRotationVelocity;

	protected float staminaVignetteEffectAmount;

	protected float orthoSizeVelocity;

	protected float inventoryZoom;

	protected Camera maskCamera;

	protected float nearClipPlaneVelocity;

	protected float farClipPlaneVelocity;

	protected float cameraDistanceVelocity;

	protected DepthOfField cachedDepthOfField;

	protected Vector3 lastAutoPos;

	protected Quaternion lastAutoRot;

	protected float lerpGuess;

	public static bool Instanced
	{
		get
		{
			return instance != null;
		}
	}

	public static Vector3 MainCameraRight
	{
		get
		{
			if (instance == null)
			{
				Debug.LogError("No CameraController instance yet!");
			}
			return instance.transform.right;
		}
	}

	public static Vector3 MainCameraForward
	{
		get
		{
			if (instance == null)
			{
				Debug.LogError("No CameraController instance yet!");
			}
			return instance.transform.forward;
		}
	}

	public static bool Flip { get; set; }

	public static float CutsceneFade { get; set; }

	public static float CutsceneZoom { get; set; }

	public static float ZTargetAmount
	{
		get
		{
			if ((bool)instance)
			{
				return instance.zTargetFactor;
			}
			return 0f;
		}
	}

	public static float HurtEffectAmount { get; protected set; }

	public static float MaskTransitionTime
	{
		get
		{
			return instance.maskTransitionTime;
		}
	}

	public static float MaskMaxFadeAmount
	{
		get
		{
			return instance.maskMaxFadeAmount;
		}
	}

	public static float MaxZTargetDistance
	{
		get
		{
			if ((bool)instance)
			{
				return instance.cameraBehaviourDataStack.maxZTargetDistance;
			}
			Debug.LogError("No CameraController instance yet!");
			return 0f;
		}
	}

	public static RenderTexture MaskTexture
	{
		get
		{
			if ((bool)instance && (bool)instance.maskCamera && (bool)instance.maskCamera.targetTexture)
			{
				return instance.maskCamera.targetTexture;
			}
			return null;
		}
	}

	protected Vector3 pivotEulerAngles
	{
		get
		{
			return new Vector3(cameraBehaviourDataStack.normalCameraEulerX, cameraBehaviourDataStack.eulerY, 0f);
		}
	}

	protected Vector3 zTargetPivotEulerAngles
	{
		get
		{
			return new Vector3(cameraBehaviourDataStack.zTargetCameraEulerX, cameraBehaviourDataStack.eulerY, 0f);
		}
	}

	public static Vector3 WorldToViewportPoint(Vector3 pos)
	{
		Camera camera = null;
		if (instance != null)
		{
			camera = instance.cachedCamera;
		}
		if (camera == null)
		{
			Debug.LogError("No CameraController instance yet!");
			return Vector3.zero;
		}
		Vector3 result = camera.WorldToViewportPoint(pos);
		if (Flip)
		{
			float x = 1f - result.x;
			result.x = x;
		}
		return result;
	}

	public static bool IsWithinScreenBounds(Vector3 position)
	{
		Vector3 vector = WorldToViewportPoint(position);
		if (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f)
		{
			return vector.y <= 1f;
		}
		return false;
	}

	public static void AddScreenshake(float f)
	{
		instance.screenshakeCapacitor = Mathf.Max(instance.screenshakeCapacitor, f);
	}

	public static void AddFastScreenshake(float f)
	{
		instance.fastScreenshakeCapacitor = Mathf.Max(instance.fastScreenshakeCapacitor, f);
	}

	public static void AddDamageEffect(float f)
	{
		instance.damageEffectCapacitor += f;
	}

	public static void AddParryEffect()
	{
		instance.lastParryTime = Time.time;
	}

	public static void AddCameraBehaviourDataLayer(CameraBehaviourDataLayer layer)
	{
		instance.cameraBehaviourDataStack.AddLayer(layer);
	}

	public static void RemoveCameraBehaviourDataLayer(CameraBehaviourDataLayer layer)
	{
		instance.cameraBehaviourDataStack.RemoveLayer(layer);
	}

	public static void SnapToPlayer()
	{
		Vector3? worldPositionOrNull = PlayerCharacter.WorldPositionOrNull;
		if (worldPositionOrNull.HasValue)
		{
			instance.focusOn(worldPositionOrNull ?? Vector3.zero);
		}
		else
		{
			Debug.LogError("SnapToPlayer() but no Player instance.");
		}
	}

	public static void WarpCamera()
	{
		warpCameraNextFrame = true;
	}

	public static void UpdateGameviewPreview()
	{
		updateInvoked = true;
	}

	protected Quaternion getRotation()
	{
		Vector3 euler = pivotEulerAngles;
		Vector3 euler2 = zTargetPivotEulerAngles;
		if (Application.isPlaying)
		{
			euler.y = smoothedYRotation;
			euler2.y = smoothedYRotation;
			euler.x = smoothedXRotation;
			euler2.x = zTargetSmoothedXRotation;
		}
		Quaternion a = Quaternion.Euler(euler);
		Quaternion b = Quaternion.Euler(euler2);
		return Quaternion.Lerp(Quaternion.Lerp(a, b, zTargetFactor), Quaternion.Euler(sweepRotation), sweepParameter);
	}

	protected Vector3 getPosition(Vector3 position)
	{
		Quaternion rotation = getRotation();
		float num = smoothedCameraDistance + Mathf.Lerp(0f, sweepDistanceBoost, sweepParameter);
		Vector3 vector = Vector3.Lerp(Vector3.zero, sweepOffsetVector, sweepParameter * sweepParameter);
		return position + rotation * (Vector3.back * num + localShakeOffset) + vector;
	}

	protected void focusOn(Vector3 position)
	{
		if (!freeControl && !(this == null))
		{
			base.transform.position = getPosition(position);
			base.transform.rotation = getRotation();
			if (cachedCamera == null)
			{
				cachedCamera = GetComponent<Camera>();
			}
			cachedCamera.nearClipPlane = smoothedNearClipPlane;
			cachedCamera.farClipPlane = smoothedFarClipPlane;
			if ((bool)perspectiveCamera)
			{
				perspectiveCamera.fieldOfView = cameraBehaviourDataStack.perspectiveFOV;
				float num = cachedCamera.farClipPlane - cachedCamera.orthographicSize / Mathf.Tan(perspectiveCamera.fieldOfView / 2f * ((float)Math.PI / 180f));
				perspectiveCamera.transform.localPosition = Vector3.forward * num;
				perspectiveCamera.nearClipPlane = cachedCamera.farClipPlane - num;
				perspectiveCamera.farClipPlane = cameraBehaviourDataStack.perspectiveFarClipPlane;
				perspectiveCamera.fieldOfView = cameraBehaviourDataStack.perspectiveFOV;
			}
		}
	}

	protected void setUpComponents()
	{
		if (cachedDepthOfField == null)
		{
			cachedDepthOfField = GetComponent<DepthOfField>();
		}
		if ((bool)cachedDepthOfField && !cachedCamera.orthographic)
		{
			if (PlayerCharacter.Instanced)
			{
				cachedDepthOfField.focalTransform = PlayerCharacter.Transform;
			}
			cachedDepthOfField.focalLength = cameraBehaviourDataStack.cameraDistance;
		}
	}

	protected void setUpMaskCamera()
	{
		Camera camera = new GameObject("Mask Camera").AddComponent<Camera>();
		int width = RuntimeQualityManager.WorldRenderTexture.width / 2;
		int height = RuntimeQualityManager.WorldRenderTexture.height / 2;
		camera.targetTexture = new RenderTexture(width, height, 0, RenderTextureFormat.DefaultHDR);
		camera.cullingMask = LayerMask.GetMask("Mask");
		camera.orthographic = true;
		camera.clearFlags = CameraClearFlags.Color;
		camera.backgroundColor = Color.black;
		Blur blur = camera.gameObject.AddComponent<Blur>();
		blur.blurShader = Shader.Find("Hidden/BlurEffectConeTap");
		blur.iterations = 2;
		blur.blurSpread = 0.6f;
		maskCamera = camera;
	}

	protected void addCameraBehaviourDataToStack()
	{
		cameraBehaviourDataStack.AddLayer(cameraBehaviourData.dataLayer);
		for (int i = 0; i < extraCameraBehaviourData.Length; i++)
		{
			cameraBehaviourDataStack.AddLayer(extraCameraBehaviourData[i].dataLayer);
		}
	}

	protected void Awake()
	{
		if (Application.isPlaying)
		{
			if (instance != null)
			{
				Debug.LogWarning("Already a registered CameraController..?");
			}
			instance = this;
			cachedCamera = GetComponent<Camera>();
			SceneManager.sceneLoaded += onLevelWasLoaded;
			addCameraBehaviourDataToStack();
			smoothedOrthoSize = cameraBehaviourDataStack.orthographicSize;
			smoothedCameraDistance = cameraBehaviourDataStack.cameraDistance;
			smoothedNearClipPlane = cameraBehaviourDataStack.orthographicNearClipPlane;
			smoothedFarClipPlane = cameraBehaviourDataStack.orthographicNearClipPlane + cameraBehaviourDataStack.orthographicDepth;
			smoothedYRotation = cameraBehaviourDataStack.eulerY;
			setUpComponents();
			if (useMask)
			{
				setUpMaskCamera();
			}
		}
	}

	protected void OnDestroy()
	{
		if ((bool)maskCamera && (bool)maskCamera.targetTexture)
		{
			maskCamera.targetTexture.Release();
			UnityEngine.Object.Destroy(maskCamera.targetTexture);
		}
	}

	protected void onLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		CutsceneZoom = 0f;
	}

	protected void LateUpdate()
	{
		if (Application.isPlaying)
		{
			if (freeControl)
			{
				lerpGuess += Time.deltaTime;
				base.transform.position = Vector3.Lerp(lastAutoPos, base.transform.position, Mathf.SmoothStep(0f, 1f, lerpGuess));
				base.transform.rotation = Quaternion.Lerp(lastAutoRot, base.transform.rotation, Mathf.SmoothStep(0f, 1f, lerpGuess));
			}
			else
			{
				lerpGuess = 0f;
			}
		}
	}

	protected void Update()
	{
		if (Application.isPlaying && PageDisplay.Open)
		{
			cachedCamera.enabled = true;
		}
		if (Time.deltaTime > 0f)
		{
			localShakeOffset = UnityEngine.Random.insideUnitCircle.normalized * (screenshakeCapacitor + fastScreenshakeCapacitor);
			screenshakeCapacitor *= Mathf.Pow(0.1f, Time.deltaTime);
			fastScreenshakeCapacitor -= 10f * Time.deltaTime;
			if (screenshakeCapacitor < 0.0001f)
			{
				screenshakeCapacitor = 0f;
			}
			if (fastScreenshakeCapacitor < 0.0001f)
			{
				fastScreenshakeCapacitor = 0f;
			}
		}
		damageEffectCapacitor -= Time.deltaTime / 1f;
		if (damageEffectCapacitor < 0.0001f)
		{
			damageEffectCapacitor = 0f;
		}
		if (!freeControl)
		{
			float target = pivotEulerAngles.y;
			Transform eulerYTarget = cameraBehaviourDataStack.eulerYTarget;
			if (eulerYTarget != null)
			{
				Vector3 vector = ((!cameraBehaviourDataStack.faceAwayFromEulerYTarget) ? (eulerYTarget.position - PlayerCharacter.WorldPosition) : ((PlayerCharacter.WorldPositionOrNull ?? eulerYTarget.position) - eulerYTarget.position));
				vector.y = 0f;
				target = 0f - Geometry.Vector2Angle(vector);
				Debug.DrawLine(PlayerCharacter.WorldPosition, PlayerCharacter.WorldPosition + vector, Color.cyan);
			}
			smoothedYRotation = Mathf.SmoothDampAngle(smoothedYRotation, target, ref smoothedYRotationVelocity, (Time.timeSinceLevelLoad == 0f) ? 0f : cameraBehaviourDataStack.eulerYSmoothTime, (Time.timeSinceLevelLoad == 0f) ? float.PositiveInfinity : cameraBehaviourDataStack.eulerYMaxSmoothSpeed, Time.unscaledDeltaTime);
			smoothedXRotation = Mathf.SmoothDampAngle(smoothedXRotation, cameraBehaviourDataStack.normalCameraEulerX, ref smoothedXRotationVelocity, (Time.timeSinceLevelLoad == 0f) ? 0f : cameraBehaviourDataStack.xRotationOverrideSmoothTime, (Time.timeSinceLevelLoad == 0f) ? float.PositiveInfinity : cameraBehaviourDataStack.xRotationOverrideMaxSpeed, Time.unscaledDeltaTime);
			zTargetSmoothedXRotation = Mathf.SmoothDampAngle(zTargetSmoothedXRotation, cameraBehaviourDataStack.zTargetCameraEulerX, ref zTargetSmoothedXRotationVelocity, (Time.timeSinceLevelLoad == 0f) ? 0f : cameraBehaviourDataStack.xRotationOverrideSmoothTime, (Time.timeSinceLevelLoad == 0f) ? float.PositiveInfinity : cameraBehaviourDataStack.xRotationOverrideMaxSpeed, Time.unscaledDeltaTime);
			Vector3 vector2 = PlayerCharacter.WorldPositionOrNull ?? previousDesiredCameraPosition;
			bool flag = false;
			RaycastHit hitInfo;
			if (!Physics.Raycast(vector2 + Vector3.up, Vector3.down, out hitInfo, 1000f, -1))
			{
				flag = true;
			}
			vector2 = (flag ? previousSmoothedTargetPosition : vector2);
			Vector3 vector3 = vector2;
			float num = cameraBehaviourDataStack.positionSmoothTime;
			if (Time.timeSinceLevelLoad == 0f)
			{
				num = 0f;
			}
			if (warpCameraNextFrame)
			{
				num = 0f;
				warpCameraNextFrame = false;
			}
			float target2 = 0f;
			if (!cameraBehaviourDataStack.smoothCamera)
			{
				base.transform.position = vector3;
				return;
			}
			Vector3? zTargetWorldPositionOrNull = PlayerCharacter.ZTargetWorldPositionOrNull;
			if (!PlayerCharacter.Instanced)
			{
				vector3 = CameraTarget.Position ?? vector3;
				target2 = 0f;
			}
			else if (PlayerCharacter.InstanceIsDead)
			{
				vector3 = PlayerCharacter.WorldPosition;
			}
			else if (zTargetWorldPositionOrNull.HasValue)
			{
				target2 = 1f;
				Vector3 vector4 = (zTargetWorldPositionOrNull ?? Vector3.zero) - vector2;
				vector3 = vector2 + vector4 * cameraBehaviourDataStack.moveCameraToZTargetParameter;
				num = cameraBehaviourDataStack.zTargettingPositionSmoothingTime;
			}
			else if (CameraPositionOverrideTrigger.PlayerInTrigger)
			{
				CameraPositionOverrideTrigger cameraPositionOverrideTrigger = CameraPositionOverrideTrigger.BestTrigger(vector2);
				vector3 = ((!(cameraPositionOverrideTrigger != null)) ? previousDesiredCameraPosition : cameraPositionOverrideTrigger.MapPosition(vector2));
				if (Time.timeSinceLevelLoad == 0f)
				{
					num = 0f;
				}
				target2 = ((!zTargetWorldPositionOrNull.HasValue && !PlayerCharacter.StrafeTargetting) ? 0f : 1f);
			}
			else if (PlayerCharacter.StrafeTargetting)
			{
				target2 = 1f;
			}
			else
			{
				float num2 = (PlayerCharacter.InstanceIsDead ? 0f : 0.25f);
				vector3 = vector2 + PlayerCharacter.LaterialVelocity * num2;
				target2 = 0f;
			}
			if (PlayerCharacter.InstanceInConduit)
			{
				num = 0.1f;
				vector3 = PlayerCharacter.WorldPosition;
			}
			if (PlayerCharacter.Instanced && PlayerCharacter.DoingHyperdash)
			{
				num /= 4f;
			}
			zTargetFactor = Mathf.SmoothDamp(zTargetFactor, target2, ref zTargetFactorVelocityRef, 0.25f, float.PositiveInfinity, TimeManager.UnscaledDeltaTime);
			Vector3 vector5 = (previousSmoothedTargetPosition = Vector3.SmoothDamp(previousSmoothedTargetPosition, vector3, ref velocityRef, num, float.PositiveInfinity, TimeManager.UnscaledDeltaTime));
			previousDesiredCameraPosition = vector3;
			float num3 = 0f;
			if (zTargetWorldPositionOrNull.HasValue)
			{
				if (cameraBehaviourDataStack.parameterizeZTargettingZoomByDistance)
				{
					float t = Vector3.Distance(PlayerCharacter.WorldPosition, PlayerCharacter.ZTargetWorldPositionOrNull ?? Vector3.zero) / cameraBehaviourDataStack.maxZTargetDistance;
					num3 = Mathf.Lerp(cameraBehaviourDataStack.zTargettingZoomAmount, 0f, t);
				}
				else
				{
					num3 = cameraBehaviourDataStack.zTargettingZoomAmount;
				}
			}
			float num4 = cameraBehaviourDataStack.orthographicSize;
			float num5 = cameraBehaviourDataStack.orthographicSizeSmoothTime;
			if (PlayerCharacter.Instanced && PlayerCharacter.InstanceIsDead)
			{
				num4 = 6f;
				num5 = 3f;
			}
			float num6 = num4 - num3;
			smoothedOrthoSize = Mathf.SmoothDamp(smoothedOrthoSize, num6, ref orthoSizeVelocity, (Time.timeSinceLevelLoad == 0f) ? 0f : num5, float.PositiveInfinity, TimeManager.UnscaledDeltaTime);
			smoothedNearClipPlane = Mathf.SmoothDamp(cachedCamera.nearClipPlane, cameraBehaviourDataStack.orthographicNearClipPlane, ref nearClipPlaneVelocity, num5, float.PositiveInfinity);
			smoothedFarClipPlane = Mathf.SmoothDamp(cachedCamera.farClipPlane, cameraBehaviourDataStack.orthographicNearClipPlane + cameraBehaviourDataStack.orthographicDepth, ref farClipPlaneVelocity, num5, float.PositiveInfinity);
			smoothedCameraDistance = Mathf.SmoothDamp(smoothedCameraDistance, cameraBehaviourDataStack.cameraDistance, ref cameraDistanceVelocity, num5, float.PositiveInfinity);
			float num7 = 0f;
			if (InventoryDisplay.InventoryOpen)
			{
				num7 = 0f;
			}
			float num8 = (num7 - inventoryZoom) * (1f - Mathf.Pow(1E-05f, TimeManager.UnscaledDeltaTime));
			inventoryZoom += num8;
			if (sweepParameter > 0f)
			{
				smoothedOrthoSize = Mathf.Lerp(num6, maxSweepOrthoSize, sweepParameter);
			}
			float num9 = Mathf.Lerp(0f, 3f, damageEffectCapacitor);
			float b = 1f;
			float b2 = 0.5f;
			float num10 = Mathf.Lerp(1f, b2, 1f - Mathf.Clamp01(Mathf.InverseLerp(0f, b, Time.time - lastParryTime)));
			float num11 = Mathf.Lerp(1f, 0.5f, Mathf.Clamp01(PlayerCharacter.SlowdownEffect));
			if (PlayerCharacter.Instanced && PlayerCharacter.InstanceIsDead)
			{
				num9 = 0f;
			}
			cachedCamera.orthographicSize = smoothedOrthoSize - num9 - CutsceneZoom - inventoryZoom;
			cachedCamera.orthographicSize *= num10;
			cachedCamera.orthographicSize *= num11;
			audioListener.position = vector5 + Vector3.up * 8f;
			float num12 = (smoothedOrthoSize - CutsceneZoom - inventoryZoom) / 4f;
			SFX.SetFMODListenerAttenuationMinMax(num12, num12 + 5f);
			Vector3 vector6 = -MainCameraRight * ((!Flip) ? 1 : (-1)) * (cachedCamera.orthographicSize / cachedCamera.aspect * inventoryCameraPositionOffset.x) + cachedCamera.transform.up * inventoryCameraPositionOffset.y;
			vector5 += vector6 * Mathf.SmoothStep(0f, 1f, Mathf.Pow(InventoryDisplay.InventoryOpenParam, inventoryCameraPositionOffsetExponent));
			if (PlayerCharacter.Instanced)
			{
				vector5 = Vector3.Lerp(vector5, PlayerCharacter.WorldPosition, damageEffectCapacitor);
			}
			focusOn(vector5);
			lastAutoPos = base.transform.position;
			lastAutoRot = base.transform.rotation;
		}
		PauseCamera.Match(cachedCamera);
		if ((bool)maskCamera)
		{
			maskCamera.transform.position = cachedCamera.transform.position;
			maskCamera.transform.rotation = cachedCamera.transform.rotation;
			maskCamera.orthographicSize = cachedCamera.orthographicSize;
			maskCamera.nearClipPlane = cameraBehaviourDataStack.orthographicNearClipPlane;
			maskCamera.farClipPlane = cameraBehaviourDataStack.perspectiveFarClipPlane + cameraBehaviourDataStack.orthographicDepth + cameraBehaviourDataStack.orthographicNearClipPlane;
		}
		float num13 = ((!InventoryDisplay.InventoryOpen) ? PlayerCharacterSpawn.SceneTransitionFade : 0f);
		FullscreenFader.FullscreenFade = num13 + CutsceneFade;
		float num14 = 0f;
		if (PlayerCharacter.Instanced)
		{
			if (PlayerCharacter.InstanceIsDead)
			{
				num14 = 1f;
			}
			else if (PlayerCharacter.HP == 0)
			{
				num14 = (Mathf.Sin(Time.time * (float)Math.PI * 3f) + 1f) / 2f * 0.25f + 0.25f;
			}
		}
		HurtEffectAmount = damageEffectCapacitor * 2f + num14;
		if (PlayerCharacter.Instanced)
		{
			staminaVignetteEffectAmount = Mathf.Clamp01(staminaVignetteEffectAmount + (float)((PlayerCharacter.Stamina == 0f) ? 1 : (-1)) * 5f * Time.deltaTime);
		}
		else
		{
			staminaVignetteEffectAmount = 0f;
		}
	}
}
