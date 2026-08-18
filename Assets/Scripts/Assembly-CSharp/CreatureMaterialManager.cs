using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CreatureMaterialManager : MonoBehaviour
{
	public enum IceState
	{
		NO_ICE = 0,
		ICE = 1,
		RECOVERING = 2
	}

	protected enum flashMode
	{
		NORMAL = 0,
		DARK = 1,
		BRIGHT = 2,
		ICE = 3,
		ATTACK_FLASH = 4
	}

	protected const float minimumFlickerTime = 0.125f;

	protected bool _flickerEnabled;

	protected float lastFlickerEnableTime = float.NegativeInfinity;

	[SerializeField]
	protected float pulseLength = 0.05f;

	[SerializeField]
	protected float maxPulsePeriod = 1.5f;

	[SerializeField]
	protected float minPulsePeriod = 0.5f;

	[SerializeField]
	[EventRef]
	protected string pulseAudioClip;

	protected Renderer cachedRenderer;

	protected bool pulseOnPreviousFrame;

	protected Material[] originalMaterials;

	protected const string kPathToGlowMaterial = "Materials/hurt glow material";

	protected const string kPathToDarkMaterial = "Materials/hurt dark material";

	protected const string kPathToIceMaterial = "Materials/ice material";

	protected const string kPathToAttackFlashMaterial = "Materials/attack flash material";

	protected int attackFlashCounter;

	protected static Dictionary<flashMode, Dictionary<int, Material[]>> materialCatalogue = new Dictionary<flashMode, Dictionary<int, Material[]>>();

	public bool FlickerEnabled
	{
		get
		{
			if (!_flickerEnabled)
			{
				return Time.time < lastFlickerEnableTime + 0.125f;
			}
			return true;
		}
		set
		{
			if (_flickerEnabled && value)
			{
				lastFlickerEnableTime = Time.time;
			}
			_flickerEnabled = value;
		}
	}

	public bool PulseEnabled { get; set; }

	public float Ghostliness { get; set; }

	public IceState Icy { get; set; }

	public bool CurrentlyFlashedOn { get; set; }

	public float PulseRateParameter { get; set; }

	protected static Material[] getMaterialArray(flashMode mode, int length)
	{
		if (!materialCatalogue.ContainsKey(mode))
		{
			Debug.LogFormat("[CreatureMaterialManager] Creating length->array dictionary for {0}", mode.ToString());
			materialCatalogue[mode] = new Dictionary<int, Material[]>();
		}
		Dictionary<int, Material[]> dictionary = materialCatalogue[mode];
		if (dictionary.ContainsKey(length))
		{
			return dictionary[length];
		}
		Material material = (Material)Resources.Load(materialPathFromMode(mode));
		Debug.LogFormat("[CreatureMaterialManager] Creating array of material {0}; len:{1}", mode.ToString(), length);
		Material[] array = new Material[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = material;
		}
		dictionary[length] = array;
		return array;
	}

	protected static string materialPathFromMode(flashMode mode)
	{
		switch (mode)
		{
		case flashMode.NORMAL:
			return null;
		case flashMode.DARK:
			return "Materials/hurt dark material";
		case flashMode.BRIGHT:
			return "Materials/hurt glow material";
		case flashMode.ICE:
			return "Materials/ice material";
		case flashMode.ATTACK_FLASH:
			return "Materials/attack flash material";
		default:
			return null;
		}
	}

	protected void displayMeshes(flashMode mode)
	{
		if (mode == flashMode.NORMAL)
		{
			cachedRenderer.sharedMaterials = originalMaterials;
		}
		else
		{
			cachedRenderer.sharedMaterials = getMaterialArray(mode, originalMaterials.Length);
			clearPropertyBlock(cachedRenderer);
		}
		CurrentlyFlashedOn = mode == flashMode.BRIGHT;
	}

	protected void clearPropertyBlock(Renderer r)
	{
		r.SetPropertyBlock(null);
	}

	private void Awake()
	{
		cachedRenderer = GetComponent<Renderer>();
		originalMaterials = cachedRenderer.materials;
	}

	private void LateUpdate()
	{
		bool flag = false;
		attackFlashCounter--;
		if (PulseEnabled)
		{
			float length = Mathf.Lerp(maxPulsePeriod, minPulsePeriod, PulseRateParameter);
			flag = Mathf.Repeat(Time.time, length) < pulseLength;
		}
		if (flag && !pulseOnPreviousFrame)
		{
			SFX.PlayAudioClipHere(pulseAudioClip, base.transform.position);
		}
		if (Icy == IceState.ICE)
		{
			displayMeshes(flashMode.ICE);
		}
		else if (attackFlashCounter > 0 && attackFlashCounter / 2 % 3 > 0)
		{
			displayMeshes(flashMode.ATTACK_FLASH);
		}
		else if (FlickerEnabled || flag)
		{
			if (Time.frameCount / 2 % 2 == 0 || flag)
			{
				displayMeshes(flashMode.BRIGHT);
			}
			else if (Icy != 0)
			{
				displayMeshes(flashMode.ICE);
			}
			else
			{
				displayMeshes(flashMode.DARK);
			}
		}
		else
		{
			displayMeshes(flashMode.NORMAL);
		}
		pulseOnPreviousFrame = flag;
	}

	internal void AttackFlash()
	{
		attackFlashCounter = 12;
	}
}
