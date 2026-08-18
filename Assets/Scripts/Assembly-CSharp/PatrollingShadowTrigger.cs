using UnityEngine;

public class PatrollingShadowTrigger : MonoBehaviour
{
	[SerializeField]
	protected StateVariable stateVar;

	[SerializeField]
	protected Transform orbitRoot;

	[SerializeField]
	protected float minStartAngle;

	[SerializeField]
	protected float maxStartAngle;

	[SerializeField]
	protected float onTriggerStartAngle;

	[SerializeField]
	protected float degreesPerSecondSlow;

	[SerializeField]
	protected float degreesPerSecondFast;

	[SerializeField]
	protected float slowRangeStart;

	[SerializeField]
	protected float slowRangeEnd;

	protected void Start()
	{
		if (stateVar.BoolValue)
		{
			orbitRoot.gameObject.SetActive(true);
			float y = Random.Range(minStartAngle, maxStartAngle);
			orbitRoot.transform.rotation = Quaternion.Euler(0f, y, 0f);
		}
		else
		{
			orbitRoot.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (orbitRoot.gameObject.activeSelf)
		{
			float y = orbitRoot.rotation.eulerAngles.y;
			float y2 = ((y > slowRangeStart && y < slowRangeEnd) ? degreesPerSecondSlow : degreesPerSecondFast);
			Vector3 vector = new Vector3(0f, y2, 0f);
			orbitRoot.Rotate(vector * Time.deltaTime);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!stateVar.BoolValue && c.GetComponent<PlayerCharacter>() != null)
		{
			stateVar.BoolValue = true;
			orbitRoot.gameObject.SetActive(true);
			orbitRoot.transform.rotation = Quaternion.Euler(0f, onTriggerStartAngle, 0f);
		}
	}
}
