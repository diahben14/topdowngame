using UnityEngine;

public class SharedMaterialUVScroll : MonoBehaviour
{
	[SerializeField]
	private SharedMaterialUVScrollPair[] materials;

	protected static SharedMaterialUVScroll instance;

	protected static Vector2[] originalOffsets;

	public void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Duplicate SharedMaterialUVScroll. There is only supposed to be one.");
		}
		originalOffsets = new Vector2[materials.Length];
		for (int i = 0; i < originalOffsets.Length; i++)
		{
			originalOffsets[i] = materials[i].material.mainTextureOffset;
		}
	}

	public void OnApplicationQuit()
	{
		for (int i = 0; i < originalOffsets.Length; i++)
		{
			materials[i].material.mainTextureOffset = originalOffsets[i];
		}
	}

	public void Update()
	{
		SharedMaterialUVScrollPair[] array = materials;
		foreach (SharedMaterialUVScrollPair sharedMaterialUVScrollPair in array)
		{
			sharedMaterialUVScrollPair.material.mainTextureOffset += sharedMaterialUVScrollPair.scrollRate * Time.deltaTime;
		}
	}
}
