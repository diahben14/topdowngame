using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DarkZone : PlayerTriggerZone<DarkZone>
{
	[SerializeField]
	protected float lanternIntensity = 0.55f;

	[SerializeField]
	protected float lanternBounce = 0.5f;

	[SerializeField]
	protected float lanternRange = 36f;

	[SerializeField]
	protected bool lanternShadows;

	public static float LanternIntensity
	{
		get
		{
			return PlayerTriggerZone<DarkZone>.GetAZonePlayerIsIn.lanternIntensity;
		}
	}

	public static float LanternBounce
	{
		get
		{
			return PlayerTriggerZone<DarkZone>.GetAZonePlayerIsIn.lanternBounce;
		}
	}

	public static float LanternRange
	{
		get
		{
			return PlayerTriggerZone<DarkZone>.GetAZonePlayerIsIn.lanternRange;
		}
	}

	public static bool LanternShadows
	{
		get
		{
			return PlayerTriggerZone<DarkZone>.GetAZonePlayerIsIn.lanternShadows;
		}
	}
}
