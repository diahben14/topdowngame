using UnityEngine;

public class StaminaSettings : MonoBehaviour
{
	public enum StaminaRequirementMode
	{
		ALLOWED_WHEN_STAMINA_IS_ZERO = 0,
		WEAK_WHEN_STAMINA_IS_ZERO = 1,
		NOT_ALLOWED_WHEN_STAMINA_IS_ZERO = 2
	}

	public static bool AttacksCostStamina;

	public static bool AttackBoostAtFullStamina;

	public static bool HitsRestoreStamina;

	public static float ZeroSPPoiseDmgFactor;

	public static bool MegaFlinches;

	public static float MegaFlinchThresholdFactor;

	public static StaminaRequirementMode DodgeRequirements = StaminaRequirementMode.WEAK_WHEN_STAMINA_IS_ZERO;

	public static StaminaRequirementMode AttackRequirements = StaminaRequirementMode.ALLOWED_WHEN_STAMINA_IS_ZERO;

	public static float RechargeDelay;

	public static float DepleatedRechargeDelay;

	public static float StaminaRechargeSpeed_Fast;

	public static float StaminaRechargeSpeed_Slow;

	public static float DodgeStaminaCost;

	public static float DamageFactorAtZeroSP;

	public static bool AlwaysMegaflinchWhenRunning;

	public static float PoiseFractionOfMaxHP;

	public static bool ShieldUpResetsRefillDelay;

	[SerializeField]
	protected GUISkin skin;

	[SerializeField]
	protected Item staminaItem;

	protected void Awake()
	{
		setDefaults();
	}

	protected void setDefaults()
	{
		AttacksCostStamina = false;
		AttackBoostAtFullStamina = false;
		DodgeRequirements = StaminaRequirementMode.WEAK_WHEN_STAMINA_IS_ZERO;
		AttackRequirements = StaminaRequirementMode.ALLOWED_WHEN_STAMINA_IS_ZERO;
		HitsRestoreStamina = false;
		ZeroSPPoiseDmgFactor = 4f;
		PoiseFractionOfMaxHP = 0.25f;
		MegaFlinches = true;
		MegaFlinchThresholdFactor = 2.5f;
		RechargeDelay = 1.25f;
		DepleatedRechargeDelay = 1.9f;
		StaminaRechargeSpeed_Fast = 100f;
		StaminaRechargeSpeed_Slow = 20f;
		DodgeStaminaCost = 25f;
		ShieldUpResetsRefillDelay = false;
		DamageFactorAtZeroSP = 1.5f;
		AlwaysMegaflinchWhenRunning = true;
		staminaItem.Quantity = 0;
	}

	protected void gap()
	{
		GUILayout.Space(0f);
	}

	protected float rangeSlider(string msg, float value, float min, float max)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(msg);
		GUILayout.Label(value.ToString(), GUILayout.Width(64f));
		GUILayout.EndHorizontal();
		float num = GUILayout.HorizontalSlider(value, min, max);
		if (max < 10f)
		{
			return Mathf.Round(num * 20f) / 20f;
		}
		return Mathf.Round(num);
	}
}
