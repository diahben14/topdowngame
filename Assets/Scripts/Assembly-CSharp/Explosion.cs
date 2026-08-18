using UnityEngine;

public class Explosion : MonoBehaviour
{
	[SerializeField]
	protected string rowID = "explosion";

	[SerializeField]
	protected float radius = 3f;

	[SerializeField]
	protected bool unblockable;

	[SerializeField]
	protected LayerMask layerMask = -1;

	[SerializeField]
	protected float physicsExplosionForceUpwardsModifier = 1f;

	[Header("Effects")]
	[SerializeField]
	protected float screenShake = 1f;

	[SerializeField]
	protected float fastScreenShake = 1f;

	[SerializeField]
	protected ParticleSystem ps;

	[SerializeField]
	protected GameObject optionalSpawn;

	protected static RaycastHit[] staticHitResultArray = new RaycastHit[32];

	protected void Awake()
	{
		Dat dat = new Dat("dmg_type", rowID, typeof(HitType));
		Dat dat2 = new Dat("dmg_hp", rowID);
		Dat dat3 = new Dat("pp_pow", rowID);
		Dat dat4 = new Dat("block_pow", rowID);
		Dat dat5 = new Dat("freeze_time", rowID);
		Dat dat6 = new Dat("dmg_force", rowID);
		Dat dat7 = new Dat("poison", rowID);
		Object.Destroy(base.gameObject, 3f);
		CameraController.AddScreenshake(screenShake);
		CameraController.AddFastScreenshake(fastScreenShake);
		int num = Physics.SphereCastNonAlloc(base.transform.position, radius, Vector3.up, staticHitResultArray, 0.1f, layerMask, QueryTriggerInteraction.Ignore);
		for (int i = 0; i < num; i++)
		{
			bool flag = false;
			HitReceiver[] components = staticHitResultArray[i].collider.GetComponents<HitReceiver>();
			foreach (HitReceiver hitReceiver in components)
			{
				if ((bool)hitReceiver)
				{
					flag = true;
					hitReceiver.ReceiveHit((HitType)dat.EnumValue, dat2.IntValue, -1f, dat3.IntValue, dat4.IntValue, dat5.Value, dat7.Value, unblockable, false, base.transform.position, dat6.Value, base.transform.position, base.transform.rotation, false, null, null, -1);
				}
			}
			if (!flag)
			{
				Rigidbody rigidbody = staticHitResultArray[i].rigidbody;
				if ((bool)rigidbody)
				{
					Vector3 vector = rigidbody.transform.position - base.transform.position + physicsExplosionForceUpwardsModifier * Vector3.up;
					float num2 = 1f / vector.sqrMagnitude;
					rigidbody.AddForce(vector.normalized * dat6.Value * num2, ForceMode.Impulse);
				}
			}
		}
		if ((bool)optionalSpawn)
		{
			Object.Instantiate(optionalSpawn, base.transform.position, Quaternion.identity);
		}
	}

	public void EmitParticles()
	{
		ps.Play();
	}
}
