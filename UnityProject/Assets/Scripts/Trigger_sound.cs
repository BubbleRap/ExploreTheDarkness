using UnityEngine;
using System.Collections;

public class Trigger_sound : MonoBehaviour {

	public Transform thirdPersonCamera;
	private AudioSource audioSource;
	public AudioClip audioClip;
	private bool isTrigger = false;
	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			audioSource = thirdPersonCamera.gameObject.GetComponent<AudioSource>();
			if(!isTrigger)
			{
				if(!audioSource.isPlaying)
				{
					audioSource.clip = audioClip;
					audioSource.Play();
					isTrigger = true;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
