using UnityEngine;

public class MusicControlZone : PlayerTriggerZone<MusicControlZone>
{
	[SerializeField]
	protected bool muteZone;

	public static bool PlayerInMuteZone
	{
		get
		{
			if (PlayerTriggerZone<MusicControlZone>.PlayerInAnyZone)
			{
				return PlayerTriggerZone<MusicControlZone>.GetAZonePlayerIsIn.muteZone;
			}
			return false;
		}
	}

	protected override void onChange()
	{
		MusicManager.OnMusicControlZoneChange();
	}
}
