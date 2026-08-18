using UnityEngine;
using UnityEngine.SceneManagement;

public class BloodstainManager : MonoBehaviour
{
	[SerializeField]
	protected BloodstainChest bloodstainChestPrefab;

	protected static BloodstainManager instance;

	private void Awake()
	{
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.sceneLoaded += onLevelWasLoaded;
	}

	private void onLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		if (SceneManager.GetActiveScene().name == Profile.GetBloodstainScene())
		{
			Vector3 bloodstainLocation = Profile.GetBloodstainLocation();
			Object.Instantiate(bloodstainChestPrefab, bloodstainLocation, Quaternion.identity);
		}
	}
}
