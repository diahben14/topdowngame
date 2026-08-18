using UnityEngine;

public class ZTargetPreviewScreenspaceIndicator : MonoBehaviour
{
	[SerializeField]
	protected GameObject[] enableDisableObjects;

	[SerializeField]
	protected RectTransform canvasRectTransform;

	[SerializeField]
	protected RectTransform rectTransformTomove;

	private void Update()
	{
		if (PlayerCharacter.Instanced && !PlayerCharacter.ZTargetWorldPositionOrNull.HasValue && PlayerCharacter.PreviewZTargetWorldPositionOrNull.HasValue && PlayerCharacterSpawn.SceneTransitionFade == 0f)
		{
			setVisability(true);
			setPositionByWorld(PlayerCharacter.PreviewZTargetWorldPositionOrNull ?? Vector3.zero);
		}
		else
		{
			setVisability(false);
		}
	}

	protected void setVisability(bool visible)
	{
		for (int i = 0; i < enableDisableObjects.Length; i++)
		{
			enableDisableObjects[i].SetActive(visible);
		}
	}

	protected void setPositionByWorld(Vector3 worldPos)
	{
		Vector2 anchoredPosition = Vector2.Scale(CameraController.WorldToViewportPoint(worldPos), canvasRectTransform.sizeDelta);
		rectTransformTomove.anchoredPosition = anchoredPosition;
	}
}
