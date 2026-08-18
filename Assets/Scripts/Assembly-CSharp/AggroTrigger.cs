using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AggroTrigger : PlayerTriggerZone<AggroTrigger>
{
	protected List<Monster> monsters = new List<Monster>();

	public void Register(Monster m)
	{
		monsters.Add(m);
	}

	protected override void onChange()
	{
		if (base.PlayerInZone && Time.time > 0f)
		{
			aggroTheRegisteredMonsters();
		}
	}

	protected void aggroTheRegisteredMonsters()
	{
		for (int i = 0; i < monsters.Count; i++)
		{
			monsters[i].ReceiveAggroTriggerAggro();
		}
	}
}
