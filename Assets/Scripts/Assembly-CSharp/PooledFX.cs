using UnityEngine;
using UnityEngine.SceneManagement;

public class PooledFX : MonoBehaviour
{
	[SerializeField]
	protected GameObject templateObject;

	[SerializeField]
	protected int poolsize = 16;

	[SerializeField]
	protected bool dontDestroyOnLoad;

	[SerializeField]
	protected bool setUpOnAwake;

	protected GameObject[] pool;

	protected int nextPointer;

	public static PooledFX NewPool(string name, GameObject template, int poolsize, bool ddol)
	{
		PooledFX pooledFX = new GameObject("Pool - " + name).AddComponent<PooledFX>();
		pooledFX.templateObject = template;
		pooledFX.poolsize = poolsize;
		pooledFX.dontDestroyOnLoad = ddol;
		pooledFX.createPool();
		return pooledFX;
	}

	public static T GetNext<T>(Vector3 pos, Quaternion rot, ref PooledFX poolObj, string name, GameObject template, int poolsize, bool ddol)
	{
		if (poolObj == null)
		{
			poolObj = NewPool(name, template, poolsize, ddol);
		}
		return poolObj.GetNext<T>(pos, rot);
	}

	public GameObject GetNext()
	{
		return GetNext(Vector3.zero, Quaternion.identity);
	}

	public T GetNext<T>(Vector3 position)
	{
		return GetNext<T>(position, Quaternion.identity);
	}

	public T GetNext<T>(Vector3 position, Quaternion rotation)
	{
		GameObject next = GetNext(position, rotation);
		if (next == null)
		{
			return default(T);
		}
		return next.GetComponent<T>();
	}

	public GameObject GetNext(Vector3 position)
	{
		return GetNext(position, Quaternion.identity);
	}

	public GameObject GetNext(Vector3 position, Quaternion rotation)
	{
		if (pool == null)
		{
			createPool();
		}
		int num = poolsize;
		int num2 = 0;
		while (pool[nextPointer].gameObject.activeSelf && num2 < num)
		{
			nextPointer = (nextPointer + 1) % num;
			num2++;
		}
		if (num2 == num)
		{
			Debug.LogErrorFormat("Pool `{1}` ran out. I guess {0} wasn't enough!", num, base.name);
			return null;
		}
		GameObject obj = pool[nextPointer];
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		obj.gameObject.SetActive(true);
		return obj;
	}

	protected void createPool()
	{
		if (dontDestroyOnLoad)
		{
			base.transform.parent = null;
			Object.DontDestroyOnLoad(this);
			SceneManager.sceneLoaded += onSceneLoaded;
		}
		pool = new GameObject[poolsize];
		for (int i = 0; i < pool.Length; i++)
		{
			pool[i] = Object.Instantiate(templateObject);
			if (dontDestroyOnLoad)
			{
				Object.DontDestroyOnLoad(pool[i].gameObject);
			}
			pool[i].gameObject.SetActive(false);
		}
	}

	private void Awake()
	{
		if (templateObject != null)
		{
			templateObject.SetActive(false);
		}
		if (setUpOnAwake)
		{
			createPool();
		}
	}

	private void onSceneLoaded(Scene scene, LoadSceneMode sceneMode)
	{
		for (int i = 0; i < pool.Length; i++)
		{
			pool[i].SetActive(false);
		}
	}
}
