using UnityEngine;
using UnityEngine.UI;

public class MoneyGoUp : MonoBehaviour
{
	[SerializeField]
	protected RectTransform canvasRectTransform;

	[SerializeField]
	protected RectTransform moneyGoUpRectTransform;

	[SerializeField]
	protected float altitude = 3f;

	[SerializeField]
	protected Text moneyText;

	[SerializeField]
	protected float accumulatedValueResetTimeout = 2f;

	[SerializeField]
	protected Image radialProgressImage;

	protected static MoneyGoUp instance;

	protected Animator cachedAnimator;

	protected int accumulatedValue;

	protected float accumulatedValueResetTime;

	public static void IncrementMoneyGoUp(int value)
	{
		instance.incrementMoneyGoUp(value);
	}

	public void incrementMoneyGoUp(int value)
	{
		accumulatedValue += value;
		moneyText.text = accumulatedValue.ToString();
		instance.cachedAnimator.SetTrigger("go");
		accumulatedValueResetTime = Time.time + accumulatedValueResetTimeout;
	}

	private void Awake()
	{
		instance = this;
		cachedAnimator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (CameraController.Instanced)
		{
			if (Time.time > accumulatedValueResetTime)
			{
				accumulatedValue = 0;
			}
			if (PlayerCharacter.Instanced && !PlayerCharacter.InstanceIsDead)
			{
				Vector2 anchoredPosition = Vector2.Scale(CameraController.WorldToViewportPoint(PlayerCharacter.WorldPosition + Vector3.up * altitude), canvasRectTransform.sizeDelta);
				moneyGoUpRectTransform.anchoredPosition = anchoredPosition;
				radialProgressImage.fillAmount = (float)SmallMoneyItem.PlayerQuantity / (float)SmallMoneyItem.ConversionThreshold;
			}
		}
	}
}
