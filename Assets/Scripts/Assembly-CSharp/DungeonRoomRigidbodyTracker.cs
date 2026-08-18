using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomRigidbodyTracker : MonoBehaviour
{
	protected List<DungeonRoom> roomsWithin = new List<DungeonRoom>();

	protected bool _visible;

	protected List<GameObject> defaultLayerGameObjects;

	public bool Visible
	{
		get
		{
			return _visible;
		}
		set
		{
			if (defaultLayerGameObjects == null)
			{
				getGameObjectsArray();
			}
			for (int i = 0; i < defaultLayerGameObjects.Count; i++)
			{
				if (defaultLayerGameObjects[i] != null)
				{
					defaultLayerGameObjects[i].layer = ((!value) ? 15 : 0);
				}
			}
			_visible = value;
		}
	}

	public bool InCurrentRoom { get; set; }

	public void AddRoom(DungeonRoom room)
	{
		roomsWithin.Add(room);
		updateVisibility();
	}

	public void RemoveRoom(DungeonRoom room)
	{
		if (roomsWithin.Contains(room))
		{
			roomsWithin.Remove(room);
			updateVisibility();
		}
		else
		{
			Debug.LogError("Tried to remove room from rigidbody tracker but it hadn't registered that room.");
		}
	}

	protected void updateVisibility()
	{
		bool visible = false;
		bool inCurrentRoom = false;
		for (int i = 0; i < roomsWithin.Count; i++)
		{
			if (roomsWithin[i].isMeshVisible)
			{
				visible = true;
			}
			if (roomsWithin[i] == DungeonRoom.CurrentRoom)
			{
				inCurrentRoom = true;
			}
		}
		Visible = visible;
		InCurrentRoom = inCurrentRoom;
	}

	protected void Awake()
	{
		ZTarget component = GetComponent<ZTarget>();
		if (component != null)
		{
			component.RoomTracker = this;
		}
	}

	protected void getGameObjectsArray()
	{
		defaultLayerGameObjects = new List<GameObject>();
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject.layer == 0 || componentsInChildren[i].gameObject.layer == 15)
			{
				defaultLayerGameObjects.Add(componentsInChildren[i].gameObject);
			}
		}
	}
}
