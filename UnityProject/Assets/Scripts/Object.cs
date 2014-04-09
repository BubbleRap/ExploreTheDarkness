using UnityEngine;
using System.Collections;

public class Object : MonoBehaviour {

	public bool firstperson;
	public AudioClip soundClip; 
	private AudioSource audioSource;
	public bool internalPlay;

	// Use this for initialization
	void Start () {
		if(internalPlay)
		{
			audioSource = transform.gameObject.GetComponent<AudioSource>();
		}
		else
		{
			audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayAudio () {
		audioSource.clip = soundClip;
		audioSource.Play();
	}
}
