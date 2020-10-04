using UnityEngine;

public class CrashSounds : MonoBehaviour
{
	public AudioSource audioSource;

	public SimpleAudioEvent CrashSound;

	private bool _crashSounds = false;
	public void ToggleCrashSounds(bool value)
	{
		_crashSounds = value;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (_crashSounds && collision.relativeVelocity.magnitude > 10f)
		{
			CrashSound.Play(audioSource);
		}
	}
}
