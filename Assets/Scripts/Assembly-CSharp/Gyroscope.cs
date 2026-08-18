using UnityEngine;

public class Gyroscope : MonoBehaviour
{
	[SerializeField]
	protected float timeScale = 1f;

	[SerializeField]
	protected Transform[] rotators_RGB;

	[SerializeField]
	protected StateVariable[] placedStateVars_RGB;

	[SerializeField]
	protected GameObject[] laserRoots_RGB;

	[SerializeField]
	protected bool isSpiritGyro;

	[SerializeField]
	protected float settleDuration = 6f;

	protected bool[] internalState_RGB;

	protected Quaternion restingRotation = Quaternion.Euler(90f, 0f, 0f);

	protected float settleTime;

	private void Awake()
	{
		internalState_RGB = new bool[placedStateVars_RGB.Length];
		for (int i = 0; i < rotators_RGB.Length; i++)
		{
			placedStateVars_RGB[i].SubscribeToChanges(onStateChange);
		}
		onStateChange();
	}

	private void OnDestroy()
	{
		for (int i = 0; i < placedStateVars_RGB.Length; i++)
		{
			placedStateVars_RGB[i].UnSubscribeToChanges(onStateChange);
		}
	}

	private void onStateChange()
	{
		Debug.Log("on state change");
		for (int i = 0; i < placedStateVars_RGB.Length; i++)
		{
			if (!internalState_RGB[i] && placedStateVars_RGB[i].BoolValue)
			{
				triggerGyroDeactiveAnimation(i);
			}
			internalState_RGB[i] = placedStateVars_RGB[i].BoolValue;
		}
	}

	private void Update()
	{
		for (int i = 0; i < rotators_RGB.Length; i++)
		{
			if (!placedStateVars_RGB[i].BoolValue)
			{
				setRotation(rotators_RGB[i], i, 0f);
			}
			laserRoots_RGB[i].SetActive(!placedStateVars_RGB[i].BoolValue);
		}
	}

	protected void triggerGyroDeactiveAnimation(int index)
	{
		Debug.Log("Triggering gyro ring deactivation");
		rotators_RGB[index].rotation = Quaternion.Euler(90f, 0f, 0f);
		rotators_RGB[index].localPosition = Vector3.up * -6f;
	}

	protected void setRotation(Transform root, int index, float st)
	{
		float num = ((st > 0f) ? st : (Time.time * timeScale));
		int num2 = 45;
		Quaternion quaternion = Quaternion.Euler(0f, (float)num2 * (num + (float)index), 0f) * Quaternion.Euler((float)num2 * (num - (float)index), 0f, 0f) * Quaternion.Euler(0f, 0f, (float)num2 * num);
		quaternion.Normalize();
		if (st > 0f)
		{
			float t = Mathf.Clamp01((Time.time - st) / settleDuration);
			t = Mathf.SmoothStep(0f, 1f, t);
			quaternion = Quaternion.Slerp(quaternion, restingRotation, t);
		}
		root.rotation = quaternion;
	}
}
