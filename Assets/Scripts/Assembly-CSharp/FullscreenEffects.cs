using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class FullscreenEffects : MonoBehaviour
{
	[SerializeField]
	protected Material material;

	[SerializeField]
	protected bool overrideFullscreenFadeParams;

	[Range(0f, 1f)]
	[SerializeField]
	protected float overridingBlackoutParam;

	[Range(0f, 1f)]
	[SerializeField]
	protected float overridingWhiteoutParam;

	protected Material editorPlayModeMaterial;

	protected bool localFlip;

	protected Material materialToUse
	{
		get
		{
			return material;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (CameraController.Flip != localFlip)
		{
			localFlip = CameraController.Flip && Application.isPlaying;
			materialToUse.SetFloat("_flip", localFlip ? 1 : 0);
		}
		if (Application.isPlaying)
		{
			materialToUse.SetFloat("_letterboxParam", CameraController.ZTargetAmount);
			materialToUse.SetFloat("_hurtParam", CameraController.HurtEffectAmount);
			materialToUse.SetFloat("_radiationParam", Mathf.Clamp01(PlayerCharacter.RadiationShowAmount));
			materialToUse.SetFloat("_ghostParam", Mathf.Clamp01(PlayerCharacter.RadiationShowAmount));
			if (overrideFullscreenFadeParams)
			{
				materialToUse.SetFloat("_whiteout", overridingWhiteoutParam);
				materialToUse.SetFloat("_blackout", overridingBlackoutParam);
			}
			else if (FullscreenFader.Whiteout)
			{
				materialToUse.SetFloat("_whiteout", FullscreenFader.FullscreenFade);
				materialToUse.SetFloat("_blackout", 0f);
			}
			else
			{
				materialToUse.SetFloat("_whiteout", 0f);
				materialToUse.SetFloat("_blackout", FullscreenFader.FullscreenFade);
			}
		}
		Graphics.Blit(RuntimeQualityManager.WorldRenderTexture, destination, materialToUse);
	}
}
