using UnityEngine;
using System.Collections;

public class Trigger_sound : MonoBehaviour {

	private AudioSource audioSource;
	private AudioSource audioSource2;

	public AudioClip[] audioClip;
	public Transform soundSource;
	private int randomNumber = 0;
	private int lastNumber = 0;
	public float DelayTimeToNextSound = 0.0f;
	private float audioTimer = 0.0f;
	public bool soundFromHead;
	public bool playMultipleTimes;

	public string SubtitlesToPlay;

	private bool isTrigger = false;

	[Range(0f, 1f)]
	public float volume = 1f;

	void Start () {

	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			if(isTrigger && !playMultipleTimes || audioTimer > 0)
			return;

			if(playMultipleTimes)
			{
				while(randomNumber == lastNumber)
				{
					randomNumber = Random.Range(0, audioClip.Length);
				}
			}

			if (soundFromHead)
				soundSource = GameObject.Find("HeadAudioSource").transform;

			if( soundSource == null || soundSource == transform)
			{
				AudioSource.PlayClipAtPoint(audioClip[randomNumber], transform.position, volume);
				if (SubtitlesToPlay.Length >0)
					SubtitleManager.Instance.SendMessage(SubtitlesToPlay);
			}
			else
			if( !soundSource.GetComponent<AudioSource>().isPlaying )
			{
				soundSource.GetComponent<AudioSource>().PlayOneShot(audioClip[randomNumber], volume);
				isTrigger = true;

				if (SubtitlesToPlay.Length >0)
					SubtitleManager.Instance.SendMessage(SubtitlesToPlay);
			}

			if(playMultipleTimes)
			{
				lastNumber = randomNumber;
				audioTimer = DelayTimeToNextSound;
			}
		}
	}

	void Update () {
		if(audioTimer > 0)
		{
			audioTimer -= Time.deltaTime;
		}
	}
}
