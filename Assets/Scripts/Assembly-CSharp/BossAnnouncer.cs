using System.Collections;
using UnityEngine;

public class BossAnnouncer : MonoBehaviour
{
	[SerializeField]
	protected MixedTextBuilder topLineTitle;

	[SerializeField]
	protected MixedTextBuilder topLineHealthBar;

	[SerializeField]
	protected MixedTextBuilder bottomLineTitle;

	[SerializeField]
	protected MixedTextBuilder bottomLineHealthBar;

	[SerializeField]
	protected RectTransform monsterHPBar;

	[SerializeField]
	protected float barWidthMax;

	[SerializeField]
	protected float barWidthMin;

	[SerializeField]
	protected float testValue = 1f;

	protected static BossAnnouncer instance;

	protected float duration;

	protected Animator cachedAnimator;

	protected Monster currentMonster;

	protected float HPBarFill
	{
		set
		{
			Vector2 sizeDelta = monsterHPBar.sizeDelta;
			sizeDelta.x = 2000f * value;
			monsterHPBar.sizeDelta = sizeDelta;
		}
	}

	public static void ShowAnnouncement(LanguageLine topLine, LanguageLine bottomLine, Monster monster, float duration)
	{
		instance.StartCoroutine(instance.showAnnouncementSequence(topLine, bottomLine, monster, duration));
	}

	protected void Awake()
	{
		instance = this;
		cachedAnimator = GetComponent<Animator>();
	}

	protected IEnumerator showAnnouncementSequence(LanguageLine topLine, LanguageLine bottomLine, Monster monster, float duration)
	{
		topLineTitle.InputLanguageLine = topLine;
		bottomLineTitle.InputLanguageLine = bottomLine;
		topLineHealthBar.InputLanguageLine = topLine;
		bottomLineHealthBar.InputLanguageLine = bottomLine;
		currentMonster = monster;
		cachedAnimator.SetTrigger("show");
		yield return new WaitForSeconds(duration);
		cachedAnimator.SetTrigger("minimize");
		for (float f = 0f; f < 1f; f += Time.deltaTime)
		{
			HPBarFill = f * monster.NormalizeHP;
			yield return 0;
		}
		while ((monster != null) & (monster.NormalizeHP > 0f))
		{
			HPBarFill = monster.NormalizeHP;
			yield return 0;
		}
		HPBarFill = 0f;
		cachedAnimator.SetTrigger("hide");
	}
}
