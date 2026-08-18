using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
	[SerializeField]
	protected float monsterActiveDistance = 64f;

	protected static MonsterManager instance;

	protected List<Monster> monsters = new List<Monster>();

	protected float monsterActiveSquareDistance;

	public static int MonsterCount
	{
		get
		{
			return instance.monsters.Count;
		}
	}

	public static void RegisterMonster(Monster m)
	{
		if ((bool)instance)
		{
			instance.monsters.Add(m);
		}
		else
		{
			Debug.LogError("MonsterManager instance doesn't exist yet.");
		}
	}

	public static void DeregisterMonster(Monster m)
	{
		if (Application.isPlaying)
		{
			if (instance.monsters.Contains(m))
			{
				instance.monsters.Remove(m);
				return;
			}
			Debug.LogFormat("Monster {0} wasn't in list.", m);
		}
	}

	public static void BroadcastAggro(Vector3 position, float radius, int aggroLayer)
	{
		for (int i = 0; i < instance.monsters.Count; i++)
		{
			Monster monster = instance.monsters[i];
			if (monster != null && monster.AggroLayer == aggroLayer)
			{
				float num = Vector3.Distance(monster.transform.position, position);
				if (num < radius)
				{
					monster.ReceiveBroadcastAggro(num);
				}
			}
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Debug.LogError("MonsterManager already exists.");
		}
		monsterActiveSquareDistance = monsterActiveDistance * monsterActiveDistance;
	}

	private IEnumerator Start()
	{
		while (true)
		{
			for (int i = 0; i < monsters.Count; i++)
			{
				Monster monster = monsters[i];
				checkMonster(monster);
				if (Time.timeSinceLevelLoad > 1f)
				{
					yield return 0;
				}
			}
			yield return 0;
		}
	}

	protected void checkMonster(Monster monster)
	{
		if (!(monster != null) || !PlayerCharacter.Instanced)
		{
			return;
		}
		bool num = (monster.transform.position - PlayerCharacter.WorldPosition).sqrMagnitude < monsterActiveSquareDistance;
		bool sleepingDueToPlayerDistance = monster.SleepingDueToPlayerDistance;
		if (num || monster.IsAggroed)
		{
			if (sleepingDueToPlayerDistance)
			{
				monster.SleepingDueToPlayerDistance = false;
			}
		}
		else if (!sleepingDueToPlayerDistance)
		{
			monster.SleepingDueToPlayerDistance = true;
		}
	}

	public static Monster GetClosestMonster(Vector3 position, Monster exclude)
	{
		float num = float.PositiveInfinity;
		Monster result = null;
		for (int i = 0; i < instance.monsters.Count; i++)
		{
			if (instance.monsters[i] != exclude)
			{
				float sqrMagnitude = (position - instance.monsters[i].transform.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					result = instance.monsters[i];
					num = sqrMagnitude;
				}
			}
		}
		return result;
	}
}
