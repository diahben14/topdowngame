using UnityEngine;

public class BloodstainChest : MonoBehaviour
{
	protected void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<PlayerCharacter>() != null)
		{
			collectBloodstain();
		}
	}

	protected void collectBloodstain()
	{
		Inventory.RecoverSuppliesFromBloodstain();
		Profile.ClearBloodstain();
		Object.Destroy(base.gameObject);
	}
}
