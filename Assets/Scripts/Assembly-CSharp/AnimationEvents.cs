using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem particleSystemToFire;

	[SerializeField]
	protected Animator animator;

	[SerializeField]
	protected GameObject[] gameObjectsToActivate;

	public void AddScreenshake(float f)
	{
		Debug.LogWarning("old AddScreenshake usage... update to __AddScreenshake");
		__AddScreenshake(f);
	}

	public void __AddScreenshake(float f)
	{
		CameraController.AddScreenshake(f);
	}

	public void __AddFastScreenshake(float f)
	{
		CameraController.AddFastScreenshake(f);
	}

	public void __PlayAudioClipHere(AnimationEvent ae)
	{
		SFX.PlayAudioClipHere(ae.stringParameter, base.transform.position);
	}

	public void __FireParticleSystem(AnimationEvent ae)
	{
		particleSystemToFire.Play(true);
	}

	private void __setTrigger(string s)
	{
		animator.SetTrigger(s);
	}

	private void __activateGameObject(string name)
	{
		for (int i = 0; i < gameObjectsToActivate.Length; i++)
		{
			if (gameObjectsToActivate[i].name == name)
			{
				gameObjectsToActivate[i].SetActive(true);
			}
		}
	}
}
