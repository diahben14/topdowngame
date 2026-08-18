using System.Collections;
using UnityEngine;

public class RotatingCubeClue : MonoBehaviour
{
	[SerializeField]
	protected string sequence;

	[SerializeField]
	protected float sequencePause = 1f;

	[SerializeField]
	protected float turnPause = 0.25f;

	[SerializeField]
	protected float turnTime = 0.25f;

	private IEnumerator Start()
	{
		while (true)
		{
			yield return new WaitForSeconds(sequencePause);
			for (int i = 0; i < sequence.Length; i++)
			{
				string text = sequence.Substring(i, 1);
				Vector3 axis = Vector3.zero;
				switch (text)
				{
				case "u":
					axis = base.transform.right;
					break;
				case "d":
					axis = -base.transform.right;
					break;
				case "l":
					axis = base.transform.up;
					break;
				case "r":
					axis = -base.transform.up;
					break;
				}
				if (turnTime > 0f)
				{
					for (float f = 0f; f < 1f; f += Time.deltaTime / turnTime)
					{
						base.transform.rotation = Quaternion.AngleAxis(f * 90f, axis);
						yield return 0;
					}
				}
				base.transform.rotation = Quaternion.identity;
				yield return new WaitForSeconds(turnPause);
			}
		}
	}
}
