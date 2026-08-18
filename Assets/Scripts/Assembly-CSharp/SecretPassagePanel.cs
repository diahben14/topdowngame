using UnityEngine;

[RequireComponent(typeof(HitReceiver))]
public class SecretPassagePanel : MonoBehaviour, IDamageable
{
	[SerializeField]
	protected StateVariable stateVariable;

	[SerializeField]
	protected Transform debrisTransform;

	[SerializeField]
	protected GameObject debrisPrefab;

	private void Start()
	{
		if ((bool)stateVariable && stateVariable.BoolValue)
		{
			base.gameObject.SetActive(false);
		}
	}

	bool IDamageable.ReceiveDamage(int damagePoints, int poisePoints, Vector3 physicsForce, float freezeTime, int poisonDamage)
	{
		if ((bool)debrisPrefab)
		{
			Object.Destroy(Object.Instantiate(debrisPrefab, debrisTransform.position, debrisTransform.rotation), 2f);
		}
		base.gameObject.SetActive(false);
		stateVariable.BoolValue = true;
		return false;
	}

	void IDamageable.BlockDamage(int sp, Vector3 physicsForce)
	{
	}
}
