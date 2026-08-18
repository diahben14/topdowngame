using UnityEngine;

public class DemoTimer : MonoBehaviour
{
	protected static float resetTime;

	public static void ResetTimer()
	{
		resetTime = Time.time;
	}

	private void OnGUI()
	{
		Rect position = new Rect(0f, Screen.height - 20, Screen.width, 20f);
		int num = (int)(Time.time - resetTime) / 10 * 10;
		Color color = ((num < 300) ? Color.gray : ((num < 480) ? Color.green : ((num < 600) ? Color.yellow : Color.red)));
		float num2 = Mathf.Clamp01((float)num / 600f) * 0.5f + 0.5f;
		color.a = num2 * num2;
		GUI.color = color;
		GUI.Label(position, num / 60 + "." + num % 60 / 10);
		if (Input.GetKey(KeyCode.DownArrow))
		{
			resetTime -= 1f;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			resetTime += 1f;
		}
	}
}
