using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour {

	public DialogChoices dialogManager;
	private AudioSource audioSource;

	public int startAtDialogNumber = 0;
	public float secondsToStart = 0.0f;
	public int endAtDialogNumber = 1;

	private bool activate = false;
	private bool activateEnd = false;
	private bool activateTrail = false;

	private Coroutine coroutineSound;

	public AudioSource trailSource;

	// Use this for initialization
	void Start () {
		audioSource = transform.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(dialogManager.getID() >= startAtDialogNumber && dialogManager.getID() < endAtDialogNumber && !audioSource.isPlaying && !activate)
		{
			coroutineSound = StartCoroutine(WaitAndPlay(secondsToStart));
			activate = true;
		}

		if(dialogManager.getID() >= endAtDialogNumber && activate)
		{
			StopCoroutine(coroutineSound);
		}

		if(dialogManager.getID() >= endAtDialogNumber && audioSource.isPlaying && !activateEnd)
		{
			audioSource.loop = false;
			activateEnd = true;

            trailSource.PlayScheduled( AudioSettings.dspTime + audioSource.clip.length - audioSource.time);
		}

		//if(dialogManager.getID() >= endAtDialogNumber && !audioSource.isPlaying && activateEnd && !activateTrail)
		//{
		//	trailSource.Play();
		//	activateTrail = true;
		//}
	}

	IEnumerator WaitAndPlay(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		audioSource.Play();
	}


}
