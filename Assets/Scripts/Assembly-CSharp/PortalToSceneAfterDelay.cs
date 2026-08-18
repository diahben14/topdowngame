using System.Collections;
using UnityEngine;

public class PortalToSceneAfterDelay : MonoBehaviour
{
	[SerializeField]
	protected float delay = 2f;

	[SerializeField]
	protected float transitionDuration = 2f;

	[SerializeField]
	protected bool whiteout;

	[SerializeField]
	protected string scene;

	[SerializeField]
	protected string id = "in";

	protected IEnumerator Start()
	{
		yield return new WaitForSeconds(delay);
		ScenePortal.DepartToScene(this, whiteout, transitionDuration, scene, id, true, 0f);
	}
}
