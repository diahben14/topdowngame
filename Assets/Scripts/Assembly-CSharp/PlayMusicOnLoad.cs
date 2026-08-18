using System.Collections;
using FMODUnity;
using UnityEngine;

public class PlayMusicOnLoad : MonoBehaviour
{
	[SerializeField]
	[EventRef]
	protected string track;

	[SerializeField]
	[EventRef]
	protected string noPlayerTrack;

	private IEnumerator Start()
	{
		Setup.DoStandardSetup();
		yield return 0;
		yield return 0;
		if (!PlayerCharacter.Instanced)
		{
			if (!string.IsNullOrEmpty(noPlayerTrack))
			{
				MusicManager.StopCurrentAndStartNew(noPlayerTrack);
			}
		}
		else if (!string.IsNullOrEmpty(track))
		{
			MusicManager.StopCurrentAndStartNew(track);
		}
	}
}
