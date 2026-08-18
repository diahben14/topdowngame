using FMODUnity;
using UnityEngine;

public class SmashableObject : PermanentStateByPosition, IDamageable
{
	[SerializeField]
	protected GameObject smashedVersionRoot;

	[SerializeField]
	protected GameObject optionalSpawnOnSmash;

	[SerializeField]
	protected float destroySpawnAfterSeconds;

	[SerializeField]
	[EventRef]
	protected string hitSFX;

	[SerializeField]
	[EventRef]
	protected string smashSFX;

	[SerializeField]
	protected float fastScreenshakeToAdd;

	[SerializeField]
	protected bool destroyGameObjectOnSmash = true;

	[SerializeField]
	protected int hpToDestroy = 1;

	[SerializeField]
	protected float randomPhysicsForceMaxMagnitude;

	[SerializeField]
	protected float randomAngularForceMaxMagnitude;

	[SerializeField]
	protected float onBreakForceFactor = 1f;

	protected const float kInvincibilityTime = 0.2f;

	protected bool smashed;

	protected float lastHitTime;

	protected Animator cachedAnimator;

	protected int hp;

	public bool MercyInvincible
	{
		get
		{
			return lastHitTime + 0.2f > Time.time;
		}
	}

	bool IDamageable.ReceiveDamage(int damagePoints, int poisePoints, Vector3 physicsForce, float freezeTime, int poisonDamage)
	{
		if (!smashed)
		{
			if (damagePoints > 0)
			{
				SFX.PlayAudioClipHere(hitSFX, base.transform.position);
				hp -= damagePoints;
				lastHitTime = Time.time;
			}
			if (hp <= 0)
			{
				smashed = true;
				smash(physicsForce * onBreakForceFactor);
			}
			else
			{
				GetComponent<Rigidbody>().AddForce(physicsForce * 10f, ForceMode.Impulse);
			}
		}
		return false;
	}

	void IDamageable.BlockDamage(int sp, Vector3 physicsForce)
	{
	}

	protected override void PermanentStateByPosition_Start()
	{
		hp = hpToDestroy;
		cachedAnimator = GetComponent<Animator>();
	}

	protected void smash(Vector3 physicsForce)
	{
		onKilled();
		CameraController.AddFastScreenshake(fastScreenshakeToAdd);
		if (cachedAnimator != null)
		{
			cachedAnimator.SetTrigger("smash");
		}
		if ((bool)optionalSpawnOnSmash)
		{
			GameObject obj = Object.Instantiate(optionalSpawnOnSmash, base.transform.position, Quaternion.identity);
			if (destroySpawnAfterSeconds > 0f)
			{
				Object.Destroy(obj, destroySpawnAfterSeconds);
			}
		}
		SFX.PlayAudioClipHere(smashSFX, base.transform.position);
		if ((bool)smashedVersionRoot)
		{
			smashedVersionRoot.transform.parent = base.transform.parent;
			smashedVersionRoot.SetActive(true);
			Rigidbody[] componentsInChildren = smashedVersionRoot.GetComponentsInChildren<Rigidbody>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Vector3 zero = Vector3.zero;
				zero += Random.insideUnitSphere * randomPhysicsForceMaxMagnitude;
				componentsInChildren[i].angularVelocity += Random.insideUnitSphere * randomAngularForceMaxMagnitude;
				componentsInChildren[i].AddForce(physicsForce + zero, ForceMode.Impulse);
			}
		}
		if (destroyGameObjectOnSmash)
		{
			base.gameObject.SetActive(false);
		}
	}
}
