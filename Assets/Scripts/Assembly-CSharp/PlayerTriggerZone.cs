using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerTriggerZone<T> : MonoBehaviour where T : PlayerTriggerZone<T>
{
	protected static List<T> registered = new List<T>();

	private int _playerInTriggerCount;

	public bool PlayerInZone
	{
		get
		{
			return playerInTriggerCount > 0;
		}
	}

	public static T GetAZonePlayerIsIn
	{
		get
		{
			int count = registered.Count;
			for (int i = 0; i < count; i++)
			{
				if (registered[i].PlayerInZone)
				{
					return registered[i];
				}
			}
			return null;
		}
	}

	public static bool PlayerInAnyZone
	{
		get
		{
			int count = registered.Count;
			for (int i = 0; i < count; i++)
			{
				if (registered[i].PlayerInZone)
				{
					return true;
				}
			}
			return false;
		}
	}

	protected int playerInTriggerCount
	{
		get
		{
			return _playerInTriggerCount;
		}
		set
		{
			_playerInTriggerCount = value;
			if (_playerInTriggerCount < 0)
			{
				Debug.LogError("playerInTriggerCount < 0", this);
			}
		}
	}

	protected virtual void onChange()
	{
	}

	protected void registerIfNeeded()
	{
		T item = (T)this;
		if (!registered.Contains(item))
		{
			registered.Add(item);
		}
	}

	protected void OnDestroy()
	{
		if (Application.isPlaying)
		{
			T item = (T)this;
			if (registered.Contains(item))
			{
				registered.Remove(item);
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (Application.isPlaying)
		{
			registerIfNeeded();
			if (c.gameObject.GetComponent<PlayerCharacter>() != null)
			{
				playerInTriggerCount++;
				onChange();
			}
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if (Application.isPlaying)
		{
			registerIfNeeded();
			if (c.gameObject.GetComponent<PlayerCharacter>() != null)
			{
				playerInTriggerCount--;
				onChange();
			}
		}
	}
}
