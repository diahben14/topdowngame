using UnityEngine;

public class ProximityDoor : MonoBehaviour
{
	protected void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<PlayerCharacter>() != null)
		{
			GetComponent<Animator>().SetTrigger("open");
		}
	}
}
