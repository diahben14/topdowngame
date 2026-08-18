using UnityEngine;

public class MPRegenTrigger : MonoBehaviour
{
	[SerializeField]
	protected float mpPerSecond = 10f;

	protected float mpAccumulation;

	private void OnTriggerStay(Collider c)
	{
		if (c.GetComponent<PlayerCharacter>() != null)
		{
			mpAccumulation += mpPerSecond * Time.deltaTime;
			if (mpAccumulation > 1f)
			{
				int num = Mathf.FloorToInt(mpAccumulation);
				PlayerCharacter.GiveMP(num, 2);
				mpAccumulation -= num;
			}
		}
	}
}
