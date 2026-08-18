using UnityEngine;

public class UVScroller : MonoBehaviour
{
	public Vector2 m_bgScrollRate;

	private Vector2 m_bgOffset;

	private Material material;

	public Vector2 BgOffset
	{
		get
		{
			return m_bgOffset;
		}
		set
		{
			m_bgOffset = value;
			SetTextureOffset(m_bgOffset);
		}
	}

	private void Start()
	{
		material = GetComponent<Renderer>().material;
		m_bgOffset = material.GetTextureOffset("_MainTex");
	}

	private void Update()
	{
		m_bgOffset += m_bgScrollRate * Time.deltaTime;
		SetTextureOffset(m_bgOffset);
	}

	private void SetTextureOffset(Vector2 offset)
	{
		m_bgOffset.x %= 1f;
		m_bgOffset.y %= 1f;
		material.SetTextureOffset("_MainTex", offset);
	}
}
