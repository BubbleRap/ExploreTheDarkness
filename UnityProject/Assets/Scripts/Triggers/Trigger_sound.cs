﻿using UnityEngine;
using System.Collections;

public class Trigger_sound : MonoBehaviour {

	private Transform thirdPersonCamera;
	private AudioSource audioSource;
	private AudioSource audioSource2;

	public AudioClip audioClip;
	public Transform soundSource;
	public bool soundFromHead;
	public bool playMultipleTimes;

	public string SubtitlesToPlay;

	private bool isTrigger = false;

	[Range(0f, 1f)]
	public float volume = 1f;

	void Start () {
		thirdPersonCamera = Camera.main.transform;
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			if(isTrigger && !playMultipleTimes)
			return;

			if (soundFromHead)
				soundSource = GameObject.Find("HeadAudioSource").transform;

			if( soundSource == null || soundSource == transform)
			{
				AudioSource.PlayClipAtPoint(audioClip, transform.position, volume);
				if (SubtitlesToPlay.Length >0)
					SubtitleManager.Instance.SendMessage(SubtitlesToPlay);
			}
			else
			if( !soundSource.audio.isPlaying )
			{
				soundSource.audio.PlayOneShot(audioClip, volume);
				isTrigger = true;

				if (SubtitlesToPlay.Length >0)
					SubtitleManager.Instance.SendMessage(SubtitlesToPlay);
			}
		}
	}
}
