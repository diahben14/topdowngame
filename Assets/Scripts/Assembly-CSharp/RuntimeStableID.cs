using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeStableID : MonoBehaviour
{
	[SerializeField]
	protected int intraSceneID = -1;

	protected static readonly int kMaxCount = 1000000;

	public bool HasValidID
	{
		get
		{
			return intraSceneID != -1;
		}
	}

	public int ID
	{
		get
		{
			if (intraSceneID == -1)
			{
				return -1;
			}
			return SceneManager.GetActiveScene().buildIndex * kMaxCount + intraSceneID;
		}
	}
}
