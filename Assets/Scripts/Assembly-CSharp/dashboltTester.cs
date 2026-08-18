using System.Collections.Generic;
using UnityEngine;

public class dashboltTester : MonoBehaviour
{
	private List<Renderer> frames = new List<Renderer>();

	private void Start()
	{
		foreach (Transform item in base.transform)
		{
			if (item.name.Contains("dashbolt_frame"))
			{
				frames.Add(item.GetComponent<Renderer>());
			}
		}
	}

	private void Update()
	{
		int num = 60;
		if (Time.frameCount % num == 0)
		{
			GetComponentInChildren<ParticleSystem>().Emit(40);
		}
		for (int i = 0; i < frames.Count; i++)
		{
			frames[i].enabled = Time.frameCount % num == i;
		}
	}
}
