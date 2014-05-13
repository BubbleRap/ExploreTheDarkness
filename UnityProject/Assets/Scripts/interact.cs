﻿using UnityEngine;
using System.Collections;

public class interact : MonoBehaviour {

	public Transform firstPersonCamera;
	public Transform thirdPersonCamera;
	bool isFirstPerson = false;
	[HideInInspector]
	public bool isInteractMode = false;
	[HideInInspector]
	public HighlightedObject interactedObject;
	private Transitioner transitionContoller = null;

	private CameraInput camInput = null;
	private CharacterMotor charMotor = null;

	public float darknessDelay = 10f;

	void Awake()
	{
		transitionContoller = Component.FindObjectOfType(typeof(Transitioner)) as Transitioner;
		camInput = GetComponentInChildren<CameraInput>();
		charMotor = GetComponent<CharacterMotor>();
	}

	void Update () 
	{
		if(interactedObject != null)
		{
			if(interactedObject.StoppedPlaying() && interactedObject.soundClip != null)
			{
 				if(isFirstPerson && !interactedObject.transitionToDarkness) {

					firstPersonCamera.gameObject.SetActive(false);
					charMotor.enabled = true;
					transform.gameObject.GetComponent<MovementController>().canMove = true;
					camInput.enabled = true;
					isInteractMode = false;
				}
			}
		}
		if (isInteractMode && Input.GetKeyDown(KeyCode.E) && !interactedObject.transitionToDarkness)
		{
			if (SubtitleManager.Instance.isPlaying){
				SubtitleManager.Instance.Stop();
			}
			if (interactedObject != null && !interactedObject.StoppedPlaying()) {
				interactedObject.StopAudio();
			}

			if(isFirstPerson)
			{
				firstPersonCamera.gameObject.SetActive(false);
				charMotor.enabled = true;
			}
			transform.gameObject.GetComponent<MovementController>().canMove = true;
			camInput.enabled = true;
			isInteractMode = false;
		}
		else if(!isInteractMode)
		{
			if( interactedObject == null )
				return;

			if(Input.GetKeyDown(KeyCode.E))
			{
				if (SubtitleManager.Instance.isPlaying)
					SubtitleManager.Instance.Stop();

				if(interactedObject.nextLevel)
				{
					//(GameObject.FindObjectOfType<EndScreenController>() as EndScreenController).ShowLoadingScreen(Application.LoadLevelAsync(1));
					Application.LoadLevelAsync(1);
				}

				if(interactedObject.firstperson)
				{
					if( interactedObject.lookFromPoint != null )
					{
						Vector3 lookFrom = interactedObject.lookFromPoint.position;
						transform.position = new Vector3(lookFrom.x, transform.position.y, lookFrom.z);
					}

					charMotor.enabled = false;

					firstPersonCamera.gameObject.SetActive(true);
					isFirstPerson = true;
					firstPersonCamera.LookAt(interactedObject.transform);


					transform.gameObject.GetComponent<MovementController>().canMove = false;
					camInput.enabled = false;

					isInteractMode = true;
				}

				if(interactedObject.soundClip != null)
					interactedObject.PlayAudio();

				if(interactedObject.subtitlesToPlay != null && interactedObject.subtitlesToPlay.Length != 0)
					SubtitleManager.Instance.SendMessage(interactedObject.subtitlesToPlay);

				if (interactedObject.transitionToDarkness){
					Invoke("TriggerDarkness",darknessDelay);
				}

			}
		}
	}

	public void TriggerDarkness(){
		transitionContoller.doTransition(true);

		firstPersonCamera.gameObject.SetActive(false);
		interactedObject = null;
		charMotor.enabled = true;
		transform.gameObject.GetComponent<MovementController>().canMove = true;
		camInput.enabled = true;
		isInteractMode = false;
	}
}
