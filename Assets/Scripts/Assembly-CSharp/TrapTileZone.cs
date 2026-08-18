using UnityEngine;

public class TrapTileZone : MonoBehaviour
{
	protected delegate void PositionFunction(Vector3 pos);

	[SerializeField]
	private bool activatedByPlayer;

	[SerializeField]
	protected TrapTile prefab;

	[SerializeField]
	protected float tileWidth = 8f;

	private readonly Vector3 kFlat = new Vector3(1f, 0f, 1f);

	protected Vector3 tileSizeVector;

	protected Color orange = new Color(1f, 0.3f, 0f);

	private void Awake()
	{
		byGrid(instantiatePrefab);
	}

	protected void OnDrawGizmos()
	{
		Gizmos.color = (activatedByPlayer ? orange : Color.magenta);
		byGrid(drawGizmoBox);
		Gizmos.color *= 0.5f;
		Gizmos.DrawWireCube(base.transform.position, Vector3.Scale(kFlat, base.transform.localScale));
	}

	protected void byGrid(PositionFunction executePerCellInGrid)
	{
		tileSizeVector = new Vector3(tileWidth, 0f, tileWidth);
		Vector3 tileCount = getTileCount();
		Vector3 vector = base.transform.position - tileWidth * (tileCount - kFlat) / 2f;
		for (int i = 0; (float)i < tileCount.z; i++)
		{
			for (int j = 0; (float)j < tileCount.x; j++)
			{
				Vector3 pos = vector + Vector3.Scale(tileSizeVector, new Vector3(j, 0f, i));
				executePerCellInGrid(pos);
			}
		}
	}

	protected void drawGizmoBox(Vector3 pos)
	{
		Gizmos.DrawWireCube(pos, tileSizeVector);
	}

	protected void instantiatePrefab(Vector3 pos)
	{
		Object.Instantiate(prefab, pos, Quaternion.identity).activatedByPlayer = activatedByPlayer;
	}

	protected Vector3 getTileCount()
	{
		return new Vector3(Mathf.Round(base.transform.localScale.x / tileWidth), 0f, Mathf.Round(base.transform.localScale.z / tileWidth));
	}
}
