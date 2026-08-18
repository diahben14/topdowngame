using UnityEngine;

namespace FMODUnity
{
	[AddComponentMenu("FMOD Studio/FMOD Studio Listener")]
	public class StudioListener : MonoBehaviour
	{
		private Rigidbody rigidBody;

		private Rigidbody2D rigidBody2D;

		public GameObject attenuationObject;

		public int ListenerNumber = -1;

		private void OnEnable()
		{
			RuntimeUtils.EnforceLibraryOrder();
			rigidBody = base.gameObject.GetComponent<Rigidbody>();
			rigidBody2D = base.gameObject.GetComponent<Rigidbody2D>();
			ListenerNumber = RuntimeManager.AddListener(this);
		}

		private void OnDisable()
		{
			RuntimeManager.RemoveListener(this);
		}

		private void Update()
		{
			if (ListenerNumber >= 0 && ListenerNumber < 8)
			{
				SetListenerLocation();
			}
		}

		private void SetListenerLocation()
		{
			if ((bool)rigidBody)
			{
				RuntimeManager.SetListenerLocation(ListenerNumber, base.gameObject, rigidBody, attenuationObject);
			}
			else
			{
				RuntimeManager.SetListenerLocation(ListenerNumber, base.gameObject, rigidBody2D, attenuationObject);
			}
		}
	}
}
