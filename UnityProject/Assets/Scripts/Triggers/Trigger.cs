﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trigger : MonoBehaviour {

	private Transitioner transitionContoller = null;
	public bool DarkMode;

	public DoorOpen doorController;
	public bool OpenDoor;

	public Respawn spawnController;
	private Color black = Color.black;

	public List<Transform> animationsToTrigger;
	public List<float> animationDelays;

	public string messageToSendToTheObject;

	public bool victoryTrigger;

	// Use this for initialization
	void Awake () {
		transitionContoller = Component.FindObjectOfType(typeof(Transitioner)) as Transitioner;
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			if( !string.IsNullOrEmpty(messageToSendToTheObject) )
				other.SendMessage(messageToSendToTheObject, SendMessageOptions.RequireReceiver);

			transitionContoller.doTransition(DarkMode);
			if (doorController != null)
				doorController.openDoor(OpenDoor);
			if (spawnController != null)
				spawnController.SetRespawnPosition(transform.position);
			if (victoryTrigger)
				(GameObject.FindObjectOfType<EndScreenController>() as EndScreenController).ShowEndScreen(true);

			for (int i=0; i<animationsToTrigger.Count; ++i){
				StartCoroutine("DelayAndPlay",i);
			}

			collider.enabled = false;
		}
	}
	
	public IEnumerator DelayAndPlay(int index){
		if (animationDelays.Count > index)
			yield return new WaitForSeconds(animationDelays[index]);

		animationsToTrigger[index].animation.Play();

		if(index == 2)
		{
			RenderSettings.ambientLight = black;
		}
	}
}
