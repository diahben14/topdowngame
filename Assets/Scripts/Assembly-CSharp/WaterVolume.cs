using UnityEngine;

public class WaterVolume : MonoBehaviour
{
	[SerializeField]
	protected bool shallow;

	[SerializeField]
	protected GameObject splashPrefab;

	private const float kCreatureVelocityThreshold = 3f;

	private const float kCreatureMaxSplashHeightVelocity = 10f;

	protected Collider cachedCollider;

	protected float waterAltitude;

	protected void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		cachedCollider = GetComponent<Collider>();
		if (!cachedCollider.isTrigger)
		{
			Debug.LogWarning("WaterVolume has collider that isn't a trigger.", base.gameObject);
		}
		waterAltitude = cachedCollider.bounds.center.y + cachedCollider.bounds.size.y / 2f;
	}

	protected void OnTriggerEnter(Collider c)
	{
		waterInteraction(c, true);
	}

	protected void OnTriggerExit(Collider c)
	{
		waterInteraction(c, false);
	}

	private void waterInteraction(Collider c, bool entering)
	{
		BombFlask component = c.GetComponent<BombFlask>();
		if ((bool)component)
		{
			component.EnterWater();
		}
		else if (c.GetComponent<HitTrigger>() != null || c.GetComponent<Pickup>() != null)
		{
			return;
		}
		Creature componentInParent = c.GetComponentInParent<Creature>();
		if (!componentInParent)
		{
			if (!shallow)
			{
				Object.Destroy(c.gameObject, 3f);
			}
		}
		else if (entering)
		{
			componentInParent.WaterTriggerCount++;
			componentInParent.WaterWorldAltitude = waterAltitude;
		}
		else
		{
			componentInParent.WaterTriggerCount--;
		}
		if (!Time.timeSinceLevelLoad.IsApproxZero() && entering)
		{
			splash(c);
		}
	}

	private void splash(Collider c)
	{
		Vector3 vector = c.transform.position + Vector3.up;
		Vector3 vector2 = Vector3.zero;
		float num = float.PositiveInfinity;
		Vector3 vector3 = cachedCollider.ClosestPointOnBounds(vector);
		float num2 = Vector3.Distance(vector, vector3);
		if (num2 < num)
		{
			num = num2;
			vector2 = vector3;
		}
		Vector3 position = vector2;
		Bounds bounds = c.bounds;
		WaterSplashSize component = c.GetComponent<WaterSplashSize>();
		float num3 = 0f;
		num3 = ((!(component == null)) ? component.SplashSize : Mathf.Min(bounds.size.x, bounds.size.y, bounds.size.z));
		Vector3 localScale = Vector3.one * num3;
		Creature component2 = c.GetComponent<Creature>();
		float num4 = 1f;
		if ((bool)component2)
		{
			if (component2.Velocity.magnitude < 3f)
			{
				return;
			}
			num4 = Mathf.Clamp01(Mathf.InverseLerp(3f, 10f, component2.Velocity.magnitude));
			localScale.y *= num4;
		}
		GameObject obj = Object.Instantiate(splashPrefab, position, Quaternion.identity);
		float paramValue = Mathf.Clamp(num3 * num4 * 5f, 0f, 10f);
		SFX.playAudioClipWithParam("event:/main/general/prop/object/water_splash", position, "object_size", paramValue);
		obj.transform.localScale = localScale;
		Object.Destroy(obj, 3f);
	}
}
