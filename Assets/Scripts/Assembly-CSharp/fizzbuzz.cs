using UnityEngine;

public class fizzbuzz : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i <= 100; i++)
		{
			bool flag = i % 5 == 0;
			bool flag2 = i % 3 == 0;
			if (flag2 && flag)
			{
				Debug.Log("FizzBuzz");
			}
			else if (flag)
			{
				Debug.Log("Buzz");
			}
			else if (flag2)
			{
				Debug.Log("Fizz");
			}
			else
			{
				Debug.Log(i.ToString());
			}
		}
	}
}
