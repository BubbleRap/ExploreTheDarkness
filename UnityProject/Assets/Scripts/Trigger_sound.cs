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
	// Use this for initialization
	void Start () {
		thirdPersonCamera = Camera.main.transform;
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			audioSource = soundSource.GetComponent<AudioSource>();
			audioSource2 = thirdPersonCamera.gameObject.GetComponent<AudioSource>();
			if(audioSource == null)
			{
				audioSource = audioSource2;
			}

			if(isTrigger && !playMultipleTimes)
			return;

				if(!audioSource2.isPlaying && !audioSource.isPlaying)
				{
					audioSource.clip = audioClip;
					audioSource.Play();
					isTrigger = true;
				}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
