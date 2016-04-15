using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour {

	public DialogChoices dialogManager;
	private AudioSource audioSource;

	public int dialogNumberStart = 0;
	public float secondsToStart = 0.0f;

	private bool activate = false;

	// Use this for initialization
	void Start () {
		audioSource = transform.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(dialogManager.getID() <= dialogNumberStart && !audioSource.isPlaying && !activate)
		{
			StartCoroutine(WaitAndPlay(secondsToStart));
			activate = true;
		}
	}

	IEnumerator WaitAndPlay(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		audioSource.Play();
	}


}
