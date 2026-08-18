using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	[SerializeField]
	protected RectTransform healthCubeTemplate;

	[SerializeField]
	protected RectTransform staminaCubeTemplate;

	[SerializeField]
	protected float shakeIntensityScaler = 0.5f;

	[SerializeField]
	protected RectTransform healthBacking;

	[SerializeField]
	protected RectTransform staminaBacking;

	[SerializeField]
	protected Image healthFlasher;

	[SerializeField]
	protected Image staminaFlasher;

	[SerializeField]
	protected RectTransform healthBar;

	[SerializeField]
	protected RectTransform staminaBar;

	protected Image[] healthPips;

	protected Image[] staminaPips;

	protected Vector3 originalHealthCubeLocalPosition;

	protected const int kMaxDisplayableCubes = 24;

	protected void buildMeters()
	{
		buildMeter(healthCubeTemplate, ref healthPips);
		buildMeter(staminaCubeTemplate, ref staminaPips);
	}

	protected void buildMeter(RectTransform template, ref Image[] imageArray)
	{
		Transform parent = template.transform.parent;
		List<Image> list = new List<Image>();
		list.AddRange(template.GetComponentsInChildren<Image>());
		for (int i = 0; i < 23; i++)
		{
			RectTransform rectTransform = Object.Instantiate(template);
			rectTransform.SetParent(parent);
			rectTransform.localScale = template.localScale;
			rectTransform.localPosition = Vector3.zero;
			list.AddRange(rectTransform.GetComponentsInChildren<Image>());
		}
		imageArray = list.ToArray();
	}

	protected float backingHeightFromPipCount(int count)
	{
		int num = Mathf.CeilToInt((float)count / 8f);
		return 178 + (num - 1) * 80;
	}

	protected void Awake()
	{
		buildMeters();
	}

	protected void Update()
	{
		int hP = PlayerCharacter.HP;
		for (int i = 0; i < healthPips.Length; i++)
		{
			healthPips[i].enabled = i < hP;
		}
		healthBacking.sizeDelta = new Vector2(healthBacking.sizeDelta.x, backingHeightFromPipCount(PlayerCharacter.MaxHP));
		staminaBacking.sizeDelta = new Vector2(staminaBacking.sizeDelta.x, backingHeightFromPipCount((int)PlayerCharacter.MaxStamina));
		int num = Mathf.RoundToInt(PlayerCharacter.Stamina);
		for (int j = 0; j < staminaPips.Length; j++)
		{
			staminaPips[j].enabled = j < num;
		}
		healthBar.localScale = new Vector3((float)PlayerCharacter.HP / (float)PlayerCharacter.MaxHP, 1f, 1f);
		staminaBar.localScale = new Vector3(PlayerCharacter.Stamina / PlayerCharacter.MaxStamina, 1f, 1f);
		healthFlasher.enabled = PlayerCharacter.CurrentlyFlashedOn;
	}
}
