using System;
using UnityEngine;

public class FairyCollection : MonoBehaviour
{
	[SerializeField]
	protected StateVariable[] fairyStateVariables;

	[SerializeField]
	protected StateVariable allFairiesFound;

	[SerializeField]
	protected Renderer[] indicatorPips;

	[SerializeField]
	protected Material onMaterial;

	[SerializeField]
	protected Material offMaterial;

	[SerializeField]
	protected GameObject fairyTemplate;

	[SerializeField]
	private float orbitSpeed;

	[SerializeField]
	private float radius;

	[SerializeField]
	private float offsetScale;

	[SerializeField]
	private float timeScale = 0.24f;

	protected GameObject[] fairyInstances;

	protected void Awake()
	{
		fairyInstances = new GameObject[fairyStateVariables.Length];
		for (int i = 0; i < fairyStateVariables.Length; i++)
		{
			fairyStateVariables[i].SubscribeToChanges(OnChangeCallback);
			GameObject gameObject = UnityEngine.Object.Instantiate(fairyTemplate);
			fairyInstances[i] = gameObject;
		}
		if (fairyStateVariables.Length != indicatorPips.Length)
		{
			Debug.LogError("[Fairy Collection] pip count != statevar count");
		}
		UnityEngine.Object.Destroy(fairyTemplate.gameObject);
		updateDisplay();
	}

	public void OnChangeCallback()
	{
		updateDisplay();
	}

	protected void OnDestroy()
	{
		for (int i = 0; i < fairyStateVariables.Length; i++)
		{
			fairyStateVariables[i].UnSubscribeToChanges(OnChangeCallback);
		}
	}

	protected void updateDisplay()
	{
		int fairyCount = getFairyCount();
		if (fairyCount == fairyStateVariables.Length)
		{
			allFairiesFound.BoolValue = true;
		}
		for (int i = 0; i < fairyStateVariables.Length; i++)
		{
			fairyInstances[i].SetActive(fairyCount > i);
			indicatorPips[i].material = ((fairyCount > i) ? onMaterial : offMaterial);
		}
	}

	protected int getFairyCount()
	{
		int num = 0;
		for (int i = 0; i < fairyStateVariables.Length; i++)
		{
			num += (fairyStateVariables[i].BoolValue ? 1 : 0);
		}
		return num;
	}

	protected void Update()
	{
		int fairyCount = getFairyCount();
		float num = Time.time * timeScale;
		for (int i = 0; i < fairyCount; i++)
		{
			Vector3 position = base.transform.position;
			float num2 = (float)i / (float)fairyCount;
			float y = num2 * 360f + num * orbitSpeed;
			Vector3 vector = Quaternion.Euler(0f, y, 0f) * (Vector3.forward * radius);
			float num3 = num2 * (float)Math.PI;
			Vector3 vector2 = Vector3.up * Mathf.Sin(num * (1f + num3 * 2f));
			vector2 += Vector3.right * (Mathf.Sin(num * 1.4f + num3 * 2f) + Mathf.Cos(num * 0.5f - num2) * 0.2f);
			vector2 += Vector3.forward * (Mathf.Cos(num * (1f + num3)) + Mathf.Sin(num * 0.25f + num2) * 0.2f);
			Vector3 position2 = position + vector + vector2.normalized * offsetScale;
			fairyInstances[i].transform.position = position2;
			fairyInstances[i].transform.rotation = Quaternion.identity;
		}
	}
}
