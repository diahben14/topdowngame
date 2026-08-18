using UnityEngine;

public class PlayerCharacterSpiritSpawn : PlayerCharacterSpawn, IInteractionReceiver
{
	[Header("Destination Scene In Real World")]
	[SerializeField]
	protected string destinationScene;

	[SerializeField]
	protected GameObject activeFX;

	public override string FullID
	{
		get
		{
			return GetSpiritWorldSpawnID(destinationScene, id);
		}
	}

	protected bool isUnlocked
	{
		get
		{
			return Profile.IsSpiritWorldSpawnUnlocked(FullID);
		}
	}

	public static string GetSpiritWorldSpawnID(string realWorldSceneName, string id)
	{
		return realWorldSceneName + "_" + id;
	}

	public bool CanReceiveInteration()
	{
		return isUnlocked;
	}

	public bool Interact(Item i)
	{
		PlayerCharacterSpawn.GoTo(destinationScene, id);
		return true;
	}

	public bool ShouldShowGenericInteractPrompt()
	{
		return true;
	}

	protected void Awake()
	{
		activeFX.SetActive(isUnlocked);
	}
}
