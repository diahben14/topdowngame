using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RandomTorqueOnAwake : MonoBehaviour
{
	[SerializeField]
	protected float torqueMagnitude;

	private void Awake()
	{
		Rigidbody component = GetComponent<Rigidbody>();
		component.AddTorque(Random.onUnitSphere * torqueMagnitude, ForceMode.Impulse);
		component.AddForce(-base.transform.forward, ForceMode.Impulse);
	}
}
