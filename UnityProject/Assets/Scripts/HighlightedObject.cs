using UnityEngine;
using System.Collections;

public class HighlightedObject : MonoBehaviour {

	public bool firstperson;
	public AudioClip soundClip; 
	private AudioSource audioSource;
	public string subtitlesToPlay;
	public bool internalPlay;
	public bool hitObject = false;
	public bool nextLevel = false;

 	private GameObject buttonPrompt;

	private bool activated = false;

	void Start () {

		if(internalPlay)
		{
			audioSource = transform.gameObject.GetComponent<AudioSource>();
		}
		else
		{
			audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
		}

		buttonPrompt = Instantiate(Resources.Load<GameObject>("buttonPrompt"), new Vector3(transform.position.x + 0.3f, transform.position.y + 0.2f, transform.position.z + 0.2f), transform.rotation) as GameObject;
		buttonPrompt.GetComponent<ButtonPrompt> ().highlightedObject = this;
		buttonPrompt.SetActive(false);
	}

	void Update () {
		if(hitObject)
		{
			if(!activated)
			{
				Color curColor = renderer.material.color;
				curColor.a = 1f;
				renderer.material.color = curColor;

				buttonPrompt.SetActive(true);
				buttonPrompt.transform.LookAt(Camera.main.gameObject.transform);

				activated = true;
			}
		}
		else
		{
			if(activated)
			{
				Color curColor = renderer.material.color;
				curColor.a = 0f;
				renderer.material.color = curColor;

				buttonPrompt.SetActive(false);

				activated = false;
			}
		}
		hitObject = false;
	}

	public void PlayAudio () {
		audioSource.clip = soundClip;
		audioSource.Play();
	}

	public void StopAudio () {
		audioSource.Stop();
	}

	public bool StoppedPlaying () {
		if(!audioSource.isPlaying)
		{
			return true;
		}
		return false;
	}
}
