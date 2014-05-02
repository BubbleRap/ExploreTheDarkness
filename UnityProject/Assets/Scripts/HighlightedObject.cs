﻿using UnityEngine;
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

	private interact interactionScript = null;

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

		interactionScript = FindObjectOfType(typeof(interact)) as interact;
	}

	void Update () {

		if( !renderer.isVisible )
			return;

		Vector3 cameraRelativePosition = Camera.main.transform.InverseTransformPoint(transform.position);
		if( cameraRelativePosition.x < 0.5f * cameraRelativePosition.z && cameraRelativePosition.x > -0.5f * cameraRelativePosition.z )
			hitObject = true;
		else
			hitObject = false;


		if(hitObject)
		{
			if(!activated)
			{
				Color curColor = renderer.material.color;
				curColor.a = 1f;
				renderer.material.color = curColor;

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

				activated = false;
			}
		}

		if( (transform.position - Camera.main.transform.position).magnitude < 3f && activated)
		{
			if( !buttonPrompt.activeInHierarchy )
			{
				buttonPrompt.SetActive(true);
				buttonPrompt.transform.LookAt(Camera.main.gameObject.transform);

				interactionScript.interactedObject = this;
			}
		}
		else 
		{
			if( buttonPrompt.activeInHierarchy )
			{
				buttonPrompt.SetActive(false);

				interactionScript.interactedObject = null;
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
