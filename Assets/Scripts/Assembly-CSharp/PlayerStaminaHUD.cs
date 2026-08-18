using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaHUD : MonoBehaviour
{
	[SerializeField]
	protected Canvas canvas;

	[SerializeField]
	protected GameObject setActiveRoot;

	[SerializeField]
	protected RectTransform staminaBarRoot;

	[SerializeField]
	protected RectTransform staminaBarBody;

	[SerializeField]
	protected Image staminaCircle;

	protected void Update()
	{
		Vector3? worldPositionOrNull = PlayerCharacter.WorldPositionOrNull;
		if (worldPositionOrNull.HasValue && !PlayerCharacter.InstanceIsDead)
		{
			float num = PlayerCharacter.Stamina / PlayerCharacter.MaxStamina;
			if (num < 1f && PlayerCharacter.ShieldUp)
			{
				setActiveRoot.SetActive(true);
				Vector2 sizeDelta = staminaBarBody.sizeDelta;
				sizeDelta.x = num;
				staminaBarBody.anchorMax = new Vector2(num, 1f);
				staminaCircle.fillAmount = num;
				Vector3 vector = CameraController.WorldToViewportPoint(worldPositionOrNull ?? Vector3.zero);
				Vector2 anchoredPosition = Vector2.Scale(b: canvas.GetComponent<RectTransform>().sizeDelta, a: vector);
				staminaBarRoot.anchoredPosition = anchoredPosition;
			}
			else
			{
				setActiveRoot.SetActive(false);
			}
		}
		else
		{
			setActiveRoot.SetActive(false);
		}
	}
}
