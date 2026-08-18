using System.Collections;
using UnityEngine;

public class PlayerCharacterBeachSpawn : PlayerCharacterSpawn
{
	[Header("Beach Spawn")]
	[SerializeField]
	protected float delayBeforeGetUpAnimation = 8f;

	[SerializeField]
	protected float delayBeforeGUI = 4f;

	[SerializeField]
	protected bool sweepOnSpawn;

	protected override bool doCameraSweepOnSpawn
	{
		get
		{
			return sweepOnSpawn;
		}
	}

	protected override void onArrivalStart()
	{
		StartCoroutine(sequence());
	}

	protected IEnumerator sequence()
	{
		PlayerCharacter.AnimationGotoState("lying down");
		yield return new WaitForSeconds(delayBeforeGetUpAnimation);
		PlayerCharacter.AnimationSetTrigger("get up");
		yield return new WaitForSeconds(delayBeforeGUI);
		GUIMode.ClearStack();
		GUIMode.PushMode(GUIMode.GetMode<GameMode>());
	}
}
