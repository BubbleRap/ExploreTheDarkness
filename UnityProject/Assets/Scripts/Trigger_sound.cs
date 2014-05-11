using UnityEngine;
using System.Collections;

public class Trigger_sound : MonoBehaviour {

	private Transform thirdPersonCamera;
	private AudioSource audioSource;
	private AudioSource audioSource2;

	public AudioClip audioClip;
	public Transform soundSource;
	public bool playMultipleTimes;

	private bool isTrigger = false;

	[Range(0f, 1f)]
	public float volume = 1f;

	void Start () {
		thirdPersonCamera = Camera.main.transform;
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			//audioSource = soundSource.GetComponent<AudioSource>();
			//audioSource2 = thirdPersonCamera.gameObject.GetComponent<AudioSource>();
			//if(audioSource == null)
			//{
			//	audioSource = audioSource2;
			//}

			if(isTrigger && !playMultipleTimes)
			return;

			if( soundSource == null || soundSource == transform)
			{
				AudioSource.PlayClipAtPoint(audioClip, transform.position, volume);
			}
			else
			//if(!audioSource2.isPlaying && !audioSource.isPlaying)	
			if( !soundSource.audio.isPlaying )
			{
				soundSource.audio.PlayOneShot(audioClip, volume);
				isTrigger = true;
			}
		}
	}
}
