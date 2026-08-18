using UnityEngine;
using UnityEngine.UI;

public class MetersHUD : MonoBehaviour
{
	[SerializeField]
	protected float shakeIntensityScaler = 0.5f;

	[SerializeField]
	protected Image healthBackingImage;

	[SerializeField]
	protected Image staminaBackingImage;

	[SerializeField]
	protected Image[] manaBackingImage;

	[SerializeField]
	protected Image healthMeterImage;

	[SerializeField]
	protected Image staminaMeterImage;

	[SerializeField]
	protected Image[] manaMeterImage;

	[SerializeField]
	protected ParticleSystem[] mpRechargeParticleSystem;

	[SerializeField]
	protected GameObject healthFlasher;

	[SerializeField]
	protected GameObject staminaFlasher;

	[SerializeField]
	protected ParticleSystem healthRechargeParticleSystem;

	[SerializeField]
	protected float hpRechageDisplayRate = 6f;

	[SerializeField]
	protected float hpDrainDisplayRate = 3f;

	[SerializeField]
	protected float hpDrainHoldTime = 1f;

	[SerializeField]
	protected GameObject healthMeterHeroGlow;

	[SerializeField]
	protected GameObject staminaMeterHeroGlow;

	[SerializeField]
	protected GameObject manaMeterHeroGlow;

	[SerializeField]
	protected Sprite redBarSprite;

	[SerializeField]
	protected Sprite greenBarSprite;

	[SerializeField]
	protected Sprite blueBarSprite;

	[SerializeField]
	protected Sprite greyBarSprite;

	[Header("Transform settings")]
	[SerializeField]
	protected float meterImageHeightAtZero = 52f;

	[SerializeField]
	protected float meterImageHeightPerUnit = 1.5f;

	[SerializeField]
	protected float backingImageHeightAtZero = 158f;

	[SerializeField]
	protected float backingImageHeightPerUnit = 1.5f;

	[Header("Statevars for Meters")]
	[SerializeField]
	protected StateVariable showManaStateVariable;

	[Header("Simple Heart UI")]
	[SerializeField]
	protected Image[] heartPips;

	[SerializeField]
	protected Image[] staminaPips;

	[SerializeField]
	protected Text simpleSPText;

	[SerializeField]
	protected Text simpleSPTextBacking;

	protected float displayedHP;

	protected float displayedSP;

	protected float displayedHPTimer;

	protected float displayedSPTimer;

	protected float[] displayedMP = new float[3];

	protected ParticleSystem.EmissionModule cachedHealthRechargeParticleSystemEmitter;

	protected ParticleSystem.EmissionModule[] cachedManaRechargeParticleSystemEmitter = new ParticleSystem.EmissionModule[3];

	protected void Awake()
	{
		cachedHealthRechargeParticleSystemEmitter = healthRechargeParticleSystem.emission;
		for (int i = 0; i < mpRechargeParticleSystem.Length; i++)
		{
			cachedManaRechargeParticleSystemEmitter[i] = mpRechargeParticleSystem[i].emission;
		}
	}

	protected void Update()
	{
		if (!PlayerCharacter.Instanced)
		{
			return;
		}
		PlayerCharacter.StatState statState_HP = PlayerCharacter.StatState_HP;
		PlayerCharacter.StatState statState_SP = PlayerCharacter.StatState_SP;
		PlayerCharacter.StatState statState_MP = PlayerCharacter.StatState_MP;
		healthMeterImage.sprite = ((statState_HP == PlayerCharacter.StatState.SUPRESSED) ? greyBarSprite : redBarSprite);
		staminaMeterImage.sprite = ((statState_SP == PlayerCharacter.StatState.SUPRESSED) ? greyBarSprite : greenBarSprite);
		manaMeterImage[2].sprite = ((statState_MP == PlayerCharacter.StatState.SUPRESSED) ? greyBarSprite : blueBarSprite);
		healthMeterHeroGlow.SetActive(statState_HP == PlayerCharacter.StatState.HERO);
		staminaMeterHeroGlow.SetActive(statState_SP == PlayerCharacter.StatState.HERO);
		manaMeterHeroGlow.SetActive(statState_MP == PlayerCharacter.StatState.HERO);
		if (displayedHP < (float)PlayerCharacter.HP && Time.timeSinceLevelLoad > 0f)
		{
			displayedHP += hpRechageDisplayRate * TimeManager.UnscaledDeltaTime;
			cachedHealthRechargeParticleSystemEmitter.enabled = true;
		}
		else
		{
			displayedHP = PlayerCharacter.HP;
			cachedHealthRechargeParticleSystemEmitter.enabled = false;
		}
		healthMeterImage.rectTransform.SetHeight(meterImageHeightAtZero + meterImageHeightPerUnit * displayedHP);
		healthBackingImage.rectTransform.SetHeight(backingImageHeightAtZero + backingImageHeightPerUnit * (float)PlayerCharacter.MaxHP);
		staminaMeterImage.rectTransform.SetHeight(meterImageHeightAtZero + meterImageHeightPerUnit * PlayerCharacter.Stamina);
		staminaBackingImage.rectTransform.SetHeight(backingImageHeightAtZero + backingImageHeightPerUnit * PlayerCharacter.MaxStamina);
		for (int i = 0; i < 3; i++)
		{
			if (displayedMP[i] < PlayerCharacter.GetMP(i) && Time.timeSinceLevelLoad > 0f)
			{
				displayedMP[i] += hpRechageDisplayRate * TimeManager.UnscaledDeltaTime;
				cachedManaRechargeParticleSystemEmitter[i].enabled = true;
			}
			else
			{
				displayedMP[i] = PlayerCharacter.GetMP(i);
				cachedManaRechargeParticleSystemEmitter[i].enabled = false;
			}
			if (PlayerCharacter.GetMaxMP(i) > 0 && !Profile.GetDemoMode() && showManaStateVariable.BoolValue)
			{
				manaBackingImage[i].gameObject.SetActive(true);
				float num = displayedMP[i];
				if (num > 0f)
				{
					manaMeterImage[i].rectTransform.SetHeight(meterImageHeightAtZero + meterImageHeightPerUnit * num);
					manaMeterImage[i].rectTransform.gameObject.SetActive(true);
				}
				else
				{
					manaMeterImage[i].gameObject.SetActive(false);
				}
				manaBackingImage[i].rectTransform.SetHeight(backingImageHeightAtZero + backingImageHeightPerUnit * (float)PlayerCharacter.GetMaxMP(i));
			}
			else
			{
				manaBackingImage[i].gameObject.SetActive(false);
			}
		}
		healthFlasher.SetActive(PlayerCharacter.CurrentlyFlashedOn);
		staminaFlasher.SetActive(PlayerCharacter.Stamina <= 0f);
	}
}
