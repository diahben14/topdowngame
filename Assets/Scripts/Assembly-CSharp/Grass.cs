using System.Collections;
using FMODUnity;
using UnityEngine;

[SelectionBase]
public class Grass : PermanentStateByPosition, IDamageable
{
	[SerializeField]
	protected float maxTiltInDegrees = 30f;

	[SerializeField]
	protected MeshFilter meshFilter;

	[SerializeField]
	protected Mesh cutGrassMesh;

	[SerializeField]
	protected Rigidbody clippingBody;

	[SerializeField]
	protected Vector3 clippingForce;

	[SerializeField]
	protected float clippingTorque;

	[SerializeField]
	protected float clippingLifetime;

	[SerializeField]
	protected float normalizedShrinkStartAge;

	[SerializeField]
	protected Transform shrinkTransform;

	[SerializeField]
	protected Component[] destroyThese;

	[SerializeField]
	protected Material materialForBase;

	[SerializeField]
	[EventRef]
	private string cutSFX;

	[SerializeField]
	protected SphereCollider tiltSphereTrigger;

	protected Quaternion initialRotation;

	protected void Awake()
	{
		initialRotation = base.transform.rotation;
	}

	protected void OnTriggerStay(Collider c)
	{
		if (c.attachedRigidbody == null)
		{
			Debug.LogWarning("Grass trigger detected a collider with no rigidbody. Shouldn't moving colliders have rigidbodies?");
			return;
		}
		Vector3 lhs = c.transform.position - base.transform.position;
		Vector3 axis = Vector3.Cross(lhs, Vector3.up);
		float angle = Mathf.Clamp01(1f - lhs.magnitude / tiltSphereTrigger.radius) * maxTiltInDegrees;
		base.transform.rotation = initialRotation * Quaternion.AngleAxis(angle, axis);
	}

	protected void OnTriggerExit(Collider c)
	{
		base.transform.rotation = initialRotation;
	}

	protected IEnumerator cleanup()
	{
		yield return new WaitForSeconds(clippingLifetime * normalizedShrinkStartAge);
		float duration = clippingLifetime * (1f - normalizedShrinkStartAge);
		if (duration > 0f)
		{
			for (float f = 0f; f < 1f; f += Time.deltaTime / duration)
			{
				if (shrinkTransform == null)
				{
					yield break;
				}
				shrinkTransform.localScale = Vector3.one * (1f - f);
				yield return 0;
			}
		}
		clippingBody.gameObject.SetActive(false);
	}

	protected override void goToDeadState()
	{
		ShowAsClipped();
	}

	protected void ShowAsClipped()
	{
		meshFilter.mesh = cutGrassMesh;
		if ((bool)materialForBase)
		{
			meshFilter.GetComponent<MeshRenderer>().material = materialForBase;
		}
		for (int i = 0; i < destroyThese.Length; i++)
		{
			Object.Destroy(destroyThese[i]);
		}
	}

	bool IDamageable.ReceiveDamage(int damagePoints, int poisePoints, Vector3 physicsForce, float freezeTime, int poisonDamage)
	{
		if (damagePoints > 0)
		{
			ShowAsClipped();
			SFX.PlayAudioClipHere(cutSFX, base.transform.position);
			clippingBody.gameObject.SetActive(true);
			clippingBody.AddForce(Quaternion.Euler(0f, Random.Range(0, 360), 0f) * clippingForce, ForceMode.Impulse);
			clippingBody.AddTorque(Random.onUnitSphere * clippingTorque, ForceMode.Impulse);
			onKilled();
			StartCoroutine(cleanup());
		}
		return false;
	}

	void IDamageable.BlockDamage(int sp, Vector3 physicsForce)
	{
	}
}
