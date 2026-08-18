using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthHUD : MonoBehaviour
{
	[SerializeField]
	protected Canvas canvas;

	[SerializeField]
	protected GameObject setActiveRoot;

	[SerializeField]
	protected RectTransform enemyHealthbarRoot;

	[SerializeField]
	protected RectTransform enemyHealthbarBody;

	[SerializeField]
	protected Image enemyPoiseBody;

	protected void Update()
	{
		Vector3? zTargetWorldPositionOrNull = PlayerCharacter.ZTargetWorldPositionOrNull;
		if (zTargetWorldPositionOrNull.HasValue)
		{
			float? zTargetHealthFactor = PlayerCharacter.ZTargetHealthFactor;
			if (zTargetHealthFactor.HasValue)
			{
				setActiveRoot.SetActive(true);
				float x = zTargetHealthFactor ?? 0f;
				float num = PlayerCharacter.ZTargetPoiseFactor ?? 0f;
				Vector2 sizeDelta = enemyHealthbarBody.sizeDelta;
				sizeDelta.x = x;
				enemyHealthbarBody.anchorMax = new Vector2(x, 1f);
				enemyPoiseBody.fillAmount = 1f - num;
				Vector3 vector = CameraController.WorldToViewportPoint(zTargetWorldPositionOrNull ?? Vector3.zero);
				Vector2 anchoredPosition = Vector2.Scale(b: canvas.GetComponent<RectTransform>().sizeDelta, a: vector);
				enemyHealthbarRoot.anchoredPosition = anchoredPosition;
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
