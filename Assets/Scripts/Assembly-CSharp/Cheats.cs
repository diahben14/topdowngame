using System.Collections;
using UnityEngine;

public class Cheats : MonoBehaviour
{
	[SerializeField]
	protected GUISkin skin;

	protected static Cheats instance;

	protected Camera guiCamera;

	protected Vector2 warpScrollPos;

	protected static bool _guiHidden;

	protected const string kTemplateSaveFilePath = "SaveFileTemplates";

	protected string[] saveFileList;

	protected string[] templateSaveFileList;

	protected string userTypedFileName;

	protected Vector2 saveFileScrollPosition;

	protected Vector2 templateSaveFileScrollPosition;

	protected Color[] debugColor = new Color[3]
	{
		new Color(0.9f, 0.7f, 0.7f),
		new Color(0.7f, 0.9f, 0.7f),
		new Color(0.7f, 0.7f, 0.9f)
	};

	public static bool DebugDisplay { get; protected set; }

	public static bool GUIHidden
	{
		get
		{
			return _guiHidden;
		}
		set
		{
			_guiHidden = value;
			instance.guiCamera.enabled = !_guiHidden;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		guiCamera = GameObject.Find("_GameGUI(Clone)").GetComponent<Camera>();
		if (guiCamera == null)
		{
			Debug.LogError("Couldn't find game gui gameobject gamera");
		}
	}

	public static void CheatWarpToPortal(string scene, string id)
	{
		RuntimeQualityManager.DebugMenuOpen = false;
		GUIMode.ClearStack();
		MusicManager.Stop();
		PlayerCharacterSpawn.GoTo(scene, id);
	}

	protected void loadSavefileLists()
	{
		saveFileList = SaveFile.GetSaveFileNameList();
		string path = Application.streamingAssetsPath + "/SaveFileTemplates";
		templateSaveFileList = SaveFile.GetSaveFilePathListFromPath(path);
	}

	protected void giveLotsOfItems()
	{
		Inventory.GetItemByName("Stick").Quantity = 1;
		Inventory.GetItemByName("Sword").Quantity = 1;
		Inventory.GetItemByName("Shield").Quantity = 1;
		Inventory.GetItemByName("Flask Container").Quantity = 4;
		Inventory.GetItemByName("Key").Quantity = 6;
		Inventory.GetItemByName("Firecracker").Quantity = 99;
		Inventory.GetItemByName("Ice Bomb").Quantity = 99;
	}

	protected IEnumerator goToCombatDemo()
	{
		PlayerCharacter.AnimationGotoState("teleport");
		GUIMode.ClearStack();
		MusicManager.Stop();
		yield return new WaitForSeconds(3f);
		for (float f = 0f; f < 1f; f += TimeManager.UnscaledDeltaTime)
		{
			CameraController.CutsceneFade = f;
			yield return 0;
		}
		PlayerCharacter.RebootCharacter();
		Profile.SetLastSpawn("Overworld", "demo start");
		Profile.SetCampfire("Overworld", "demo start");
		PlayerCharacterSpawn.GoTo("Overworld", "demo start");
	}

	protected void swampWarp()
	{
		Profile.Reset();
		Profile.SetLastSpawn("Swamp 1", "start");
		Profile.SetCampfire("Swamp 1", "start");
		Inventory.GetItemByName("Sword").Quantity = 1;
		Inventory.GetItemByName("Shield").Quantity = 1;
		Inventory.GetItemByName("Lantern").Quantity = 1;
		Inventory.GetItemByName("Techbow").Quantity = 1;
		Inventory.GetItemByName("Level Up - Health").Quantity = 2;
		Inventory.GetItemByName("Flask Container").Quantity = 2;
		StateVariable.GetStateVariableByName("Is Night").BoolValue = true;
		PlayerCharacter.RebootCharacter();
		CheatWarpToPortal("Swamp 1", "start");
	}
}
