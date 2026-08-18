using System;
using System.Collections;
using UnityEngine;

public class BossAnnounceOnAggro : MonoBehaviour
{
	[SerializeField]
	protected LanguageLine bossTitleTopLine;

	[SerializeField]
	protected LanguageLine bossTitleBottomLine;

	[SerializeField]
	protected float initialDelay;

	[SerializeField]
	protected float bossTitleDuration = 4f;

	protected Monster monster;

	protected void Start()
	{
		monster = GetComponent<Monster>();
		Monster obj = monster;
		obj.OnAggroChangeCallbacks = (Action)Delegate.Combine(obj.OnAggroChangeCallbacks, new Action(onAggroChange));
	}

	protected void onAggroChange()
	{
		if (monster.IsAggroed)
		{
			StartCoroutine(titleSequence());
		}
	}

	protected IEnumerator titleSequence()
	{
		yield return new WaitForSeconds(initialDelay);
		BossAnnouncer.ShowAnnouncement(bossTitleTopLine, bossTitleBottomLine, monster, bossTitleDuration);
	}
}
