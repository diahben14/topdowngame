using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class RuntimeCameraQualityController : MonoBehaviour
{
	[HideInInspector]
	public DepthOfField dof;

	[HideInInspector]
	public VignetteAndChromaticAberration vignette;

	[HideInInspector]
	public ScreenSpaceAmbientOcclusion ssao;

	private void Awake()
	{
		RuntimeQualityManager.RegisterQualityController(this);
		dof = GetComponent<DepthOfField>();
		vignette = GetComponent<VignetteAndChromaticAberration>();
		ssao = GetComponent<ScreenSpaceAmbientOcclusion>();
	}

	private void OnDestroy()
	{
		RuntimeQualityManager.DeregisterQualityController(this);
	}
}
