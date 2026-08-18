using UnityEngine;

[AddComponentMenu("ResonanceAudio/FmodResonanceAudioRoom")]
public class FmodResonanceAudioRoom : MonoBehaviour
{
	public enum SurfaceMaterial
	{
		Transparent = 0,
		AcousticCeilingTiles = 1,
		BrickBare = 2,
		BrickPainted = 3,
		ConcreteBlockCoarse = 4,
		ConcreteBlockPainted = 5,
		CurtainHeavy = 6,
		FiberglassInsulation = 7,
		GlassThin = 8,
		GlassThick = 9,
		Grass = 10,
		LinoleumOnConcrete = 11,
		Marble = 12,
		Metal = 13,
		ParquetOnConcrete = 14,
		PlasterRough = 15,
		PlasterSmooth = 16,
		PlywoodPanel = 17,
		PolishedConcreteOrTile = 18,
		Sheetrock = 19,
		WaterOrIceSurface = 20,
		WoodCeiling = 21,
		WoodPanel = 22
	}

	public SurfaceMaterial leftWall = SurfaceMaterial.ConcreteBlockCoarse;

	public SurfaceMaterial rightWall = SurfaceMaterial.ConcreteBlockCoarse;

	public SurfaceMaterial floor = SurfaceMaterial.ParquetOnConcrete;

	public SurfaceMaterial ceiling = SurfaceMaterial.PlasterRough;

	public SurfaceMaterial backWall = SurfaceMaterial.ConcreteBlockCoarse;

	public SurfaceMaterial frontWall = SurfaceMaterial.ConcreteBlockCoarse;

	public float reflectivity = 1f;

	public float reverbGainDb;

	public float reverbBrightness;

	public float reverbTime = 1f;

	public Vector3 size = Vector3.one;

	private void OnEnable()
	{
		FmodResonanceAudio.UpdateAudioRoom(this, FmodResonanceAudio.IsListenerInsideRoom(this));
	}

	private void OnDisable()
	{
		FmodResonanceAudio.UpdateAudioRoom(this, false);
	}

	private void Update()
	{
		FmodResonanceAudio.UpdateAudioRoom(this, FmodResonanceAudio.IsListenerInsideRoom(this));
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, size);
	}
}
