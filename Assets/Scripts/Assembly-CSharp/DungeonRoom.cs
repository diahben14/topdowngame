using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DungeonRoom : MonoBehaviour
{
	[SerializeField]
	protected DungeonRoom[] farRooms;

	[SerializeField]
	protected GameObject maskObject;

	[SerializeField]
	protected bool ignoreWhenInConduit;

	[SerializeField]
	protected bool instantRoomTransition;

	protected GameObject[] defaultLayerGameObjects;

	protected string[] defaultLayerGameObjectNames;

	protected Light[] lights;

	protected static DungeonRoom _currentRoom;

	protected static Stack<DungeonRoom> previousRooms = new Stack<DungeonRoom>();

	protected static List<DungeonRoom> rooms = new List<DungeonRoom>();

	public const int kDefaultLayer = 0;

	public const int kHiddenLayer = 15;

	protected List<DungeonRoomRigidbodyTracker> trackedRigidbodies = new List<DungeonRoomRigidbodyTracker>();

	private int _numberOfMyTriggersPlayerIsIn;

	public static bool TransitionSequenceRunning { get; protected set; }

	public static float TransitionFade { get; protected set; }

	public static DungeonRoom CurrentRoom
	{
		get
		{
			return _currentRoom;
		}
		protected set
		{
			if (value != _currentRoom)
			{
				DungeonRoom currentRoom = _currentRoom;
				previousRooms.Push(_currentRoom);
				_currentRoom = value;
				_currentRoom.StartCoroutine(_currentRoom.changeToThisRoom(currentRoom));
			}
		}
	}

	public bool isMeshVisible { get; protected set; }

	protected int numberOfMyTriggersPlayerIsIn
	{
		get
		{
			return _numberOfMyTriggersPlayerIsIn;
		}
		set
		{
			if (_numberOfMyTriggersPlayerIsIn == 0 && value == 1)
			{
				onPlayerEnterCompositeTrigger();
			}
			if (_numberOfMyTriggersPlayerIsIn > 0 && value == 0)
			{
				onPlayerExitCompositeTrigger();
			}
			_numberOfMyTriggersPlayerIsIn = value;
		}
	}

	protected static void register(DungeonRoom dr)
	{
		if (!rooms.Contains(dr))
		{
			rooms.Add(dr);
		}
	}

	protected static void deregister(DungeonRoom dr)
	{
		if (rooms.Contains(dr))
		{
			rooms.Remove(dr);
		}
		else
		{
			Debug.LogError("Tried to deregister DungeonRoom but it wasn't registered.");
		}
	}

	protected void recursiveSetFarRoomsVisible(List<DungeonRoom> visited)
	{
		for (int i = 0; i < farRooms.Length; i++)
		{
			DungeonRoom dungeonRoom = farRooms[i];
			if (!visited.Contains(dungeonRoom))
			{
				dungeonRoom.setVisible(true, false);
				dungeonRoom.recursiveSetFarRoomsVisible(visited);
			}
		}
	}

	protected void setVisible(bool mesh, bool mask)
	{
		if (!(this != null))
		{
			return;
		}
		if (Application.isPlaying)
		{
			for (int i = 0; i < defaultLayerGameObjects.Length; i++)
			{
				if (defaultLayerGameObjects[i] != null)
				{
					defaultLayerGameObjects[i].layer = ((!mesh) ? 15 : 0);
					continue;
				}
				Debug.LogFormat("Setting room `{0}` {1} but object `{2}` was null. Should it be possible to destroy it?", base.name, mesh ? "VISIBLE" : "INVISIBLE", defaultLayerGameObjectNames[i]);
			}
			for (int j = 0; j < lights.Length; j++)
			{
				if (lights[j] != null)
				{
					lights[j].enabled = mesh;
					continue;
				}
				Debug.LogFormat("Setting room `{0}` {1} but light `{2}` was null. Should it be possible to destroy it?", base.name, mesh ? "VISIBLE" : "INVISIBLE", defaultLayerGameObjectNames[j]);
			}
			for (int k = 0; k < trackedRigidbodies.Count; k++)
			{
				trackedRigidbodies[k].Visible = mesh;
				trackedRigidbodies[k].InCurrentRoom = mask;
			}
			if ((bool)maskObject)
			{
				maskObject.SetActive(mask);
			}
		}
		else
		{
			Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
			for (int l = 0; l < componentsInChildren.Length; l++)
			{
				if (componentsInChildren[l].gameObject.layer == 0 || componentsInChildren[l].gameObject.layer == 15)
				{
					componentsInChildren[l].gameObject.layer = ((!mesh) ? 15 : 0);
				}
			}
			Light[] componentsInChildren2 = GetComponentsInChildren<Light>();
			for (int m = 0; m < componentsInChildren2.Length; m++)
			{
				componentsInChildren2[m].enabled = mesh;
			}
			if ((bool)maskObject)
			{
				maskObject.SetActive(true);
			}
		}
		isMeshVisible = mesh;
	}

	protected IEnumerator changeToThisRoom(DungeonRoom previousRoom)
	{
		while (TransitionSequenceRunning)
		{
			yield return 0;
		}
		TransitionSequenceRunning = true;
		float transitionTime = ((instantRoomTransition | (previousRoom != null && previousRoom.instantRoomTransition)) ? 0f : CameraController.MaskTransitionTime);
		if (transitionTime > 0f && CameraController.MaskTransitionTime > 0f)
		{
			for (float f = 0f; f < 1f; f += TimeManager.UnscaledDeltaTime / transitionTime)
			{
				DrawCameraMask.MaskFade = f * CameraController.MaskMaxFadeAmount;
				yield return 0;
			}
		}
		DrawCameraMask.MaskFade = 1f;
		rooms.RemoveAll((DungeonRoom item) => item == null);
		for (int i = 0; i < rooms.Count; i++)
		{
			rooms[i].setVisible(false, false);
		}
		setVisible(true, true);
		List<DungeonRoom> list = new List<DungeonRoom>();
		list.Add(this);
		recursiveSetFarRoomsVisible(list);
		if (transitionTime > 0f && CameraController.MaskTransitionTime > 0f)
		{
			for (float f = 0f; f < 1f; f += TimeManager.UnscaledDeltaTime / transitionTime)
			{
				DrawCameraMask.MaskFade = (1f - f) * CameraController.MaskMaxFadeAmount;
				yield return 0;
			}
		}
		DrawCameraMask.MaskFade = 0f;
		TransitionSequenceRunning = false;
	}

	private void Start()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		register(this);
		base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if ((componentsInChildren[i].gameObject.layer == 0 || componentsInChildren[i].gameObject.layer == 15) && componentsInChildren[i].GetComponentInParent<Creature>() == null && componentsInChildren[i].GetComponentInParent<Rigidbody>() == null)
			{
				list.Add(componentsInChildren[i].gameObject);
			}
		}
		defaultLayerGameObjects = list.ToArray();
		defaultLayerGameObjectNames = new string[defaultLayerGameObjects.Length];
		for (int j = 0; j < defaultLayerGameObjects.Length; j++)
		{
			defaultLayerGameObjectNames[j] = defaultLayerGameObjects[j].name;
		}
		lights = GetComponentsInChildren<Light>();
		setVisible(false, false);
	}

	private void OnDestry()
	{
		if (Application.isPlaying)
		{
			deregister(this);
		}
	}

	private void OnDisable()
	{
		if (Application.isPlaying && CurrentRoom == this)
		{
			TransitionSequenceRunning = false;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!c.isTrigger)
		{
			if (c.GetComponent<PlayerCharacter>() != null)
			{
				numberOfMyTriggersPlayerIsIn++;
			}
			else if ((bool)c.attachedRigidbody)
			{
				DungeonRoomRigidbodyTracker dungeonRoomRigidbodyTracker = addOrGetExistingTracker(c);
				trackedRigidbodies.Add(dungeonRoomRigidbodyTracker);
				dungeonRoomRigidbodyTracker.AddRoom(this);
			}
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if (!c.isTrigger)
		{
			if (c.GetComponent<PlayerCharacter>() != null)
			{
				numberOfMyTriggersPlayerIsIn--;
			}
			else if ((bool)c.attachedRigidbody)
			{
				DungeonRoomRigidbodyTracker dungeonRoomRigidbodyTracker = addOrGetExistingTracker(c);
				trackedRigidbodies.Remove(dungeonRoomRigidbodyTracker);
				dungeonRoomRigidbodyTracker.RemoveRoom(this);
			}
		}
	}

	private DungeonRoomRigidbodyTracker addOrGetExistingTracker(Collider c)
	{
		DungeonRoomRigidbodyTracker dungeonRoomRigidbodyTracker = c.gameObject.GetComponent<DungeonRoomRigidbodyTracker>();
		if (!dungeonRoomRigidbodyTracker)
		{
			dungeonRoomRigidbodyTracker = c.gameObject.AddComponent<DungeonRoomRigidbodyTracker>();
			dungeonRoomRigidbodyTracker.Visible = false;
		}
		return dungeonRoomRigidbodyTracker;
	}

	protected void onPlayerEnterCompositeTrigger()
	{
		if (!ignoreWhenInConduit || !PlayerCharacter.InstanceInConduit)
		{
			CurrentRoom = this;
		}
	}

	private void onPlayerExitCompositeTrigger()
	{
		if (this == CurrentRoom)
		{
			DungeonRoom dungeonRoom = null;
			while (previousRooms.Count > 0 && (dungeonRoom == null || dungeonRoom.numberOfMyTriggersPlayerIsIn == 0))
			{
				dungeonRoom = previousRooms.Pop();
			}
			if (dungeonRoom != null && dungeonRoom.numberOfMyTriggersPlayerIsIn > 0)
			{
				CurrentRoom = dungeonRoom;
			}
		}
	}
}
