using UnityEngine;

public static class Setup
{
	private static readonly string kStandardManagerPath = "GameInit/";

	private static bool setupDone;

	public static void DoStandardSetup()
	{
		if (!setupDone)
		{
			Debug.Log("[Standard Setup] Doing standard setup.");
			Object[] array = Resources.LoadAll(kStandardManagerPath);
			for (int i = 0; i < array.Length; i++)
			{
				Object.DontDestroyOnLoad(Object.Instantiate(array[i]));
			}
			Cursor.visible = false;
			setupDone = true;
		}
	}
}
