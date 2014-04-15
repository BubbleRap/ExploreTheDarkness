﻿using UnityEngine;
using System.Collections;

public class Transitioner : MonoBehaviour {

	public Color adventureModeAmbientLight;
	public Color shadowModeAmbientLight;
	[HideInInspector]
	public bool darkMode = false;
	public Transform directionalLight;
	private GameObject[] exposedLights;
	public Transform firstPersonCamera;
	public Transform thirdPersonCamera;
	public Transform thirdPersonCharacterModel;
	public Transform lightRoom;
	public Transform darkRoom;

	public Transform lilBroTransform = null;

	public Transform oneHandJoint;
	public Transform twoHandsJoint;

	private Animator siljasAnimator = null;

	void Awake()
	{
		siljasAnimator = GetComponentInChildren<Animator>();
	}

	// Use this for initialization
	void Start () {
        exposedLights = GameObject.FindGameObjectsWithTag("ExposedLight");
        foreach (GameObject expolight in exposedLights) {
			expolight.SetActive(false);
		}

		StartCoroutine(switchToDarkMode(2.0F));

		doTransition(darkMode);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.T))
		{
			doTransition(!darkMode);
		}
	}

	public void doTransition (bool theDarkMode)
	{
		if(theDarkMode)
		{
			darkMode = true;
		}
		else
		{
			darkMode = false;
		}
	}

	IEnumerator switchToDarkMode(float waitTime) {
		while (true) 
		{
			if(darkMode)
			{
				if(!firstPersonCamera.gameObject.activeInHierarchy)
				{
					Camera.main.gameObject.GetComponent<CameraFollow>().horizontalShakeIntensity = 1.0f;
					Camera.main.gameObject.GetComponent<CameraFollow>().verticalShakeIntensity = 1.0f;
			        yield return new WaitForSeconds(waitTime);

			        darkRoom.gameObject.SetActive(true);
					lightRoom.gameObject.SetActive(false);
					directionalLight.gameObject.SetActive(false);
					//thirdPersonCharacterModel.gameObject.SetActive(false);
					transform.gameObject.GetComponent<CharacterMotor>().movement.maxForwardSpeed = 1.5f;
					transform.gameObject.GetComponent<CharacterMotor>().movement.maxSidewaysSpeed = 1.5f;
					transform.gameObject.GetComponent<MovementController>().enabled = false;
					//transform.gameObject.GetComponent<CharacterMotor>().enabled = true;
					transform.gameObject.GetComponent<FPSInputController>().enabled = true;
					firstPersonCamera.gameObject.GetComponent<MouseLook>().enabled = true;
					firstPersonCamera.gameObject.SetActive(true);
					//thirdPersonCamera.gameObject.SetActive(false);
					RenderSettings.ambientLight = shadowModeAmbientLight;
					foreach (GameObject expolight in exposedLights) {
						expolight.SetActive(true);
					}

					oneHandJoint.gameObject.SetActive(false);
					twoHandsJoint.gameObject.SetActive(true);

					siljasAnimator.SetBool("darkmode", true);
				}
			}
			else
			{
				if(firstPersonCamera.gameObject.activeInHierarchy && !transform.gameObject.GetComponent<interact>().isInteractMode)
				{
					Camera.main.gameObject.GetComponent<CameraFollow>().horizontalShakeIntensity = 0.0f;
					Camera.main.gameObject.GetComponent<CameraFollow>().verticalShakeIntensity = 0.0f;

					darkRoom.gameObject.SetActive(false);
					lightRoom.gameObject.SetActive(true);
					directionalLight.gameObject.SetActive(true);
					//thirdPersonCharacterModel.gameObject.SetActive(true);
					//transform.gameObject.GetComponent<ThirdPersonController>().enabled = true;
					transform.gameObject.GetComponent<CharacterMotor>().movement.maxForwardSpeed = 1;
					transform.gameObject.GetComponent<CharacterMotor>().movement.maxSidewaysSpeed = 1;
					transform.gameObject.GetComponent<MovementController>().enabled = true;
					//transform.gameObject.GetComponent<CharacterMotor>().enabled = false;
					transform.gameObject.GetComponent<FPSInputController>().enabled = false;
					firstPersonCamera.gameObject.GetComponent<MouseLook>().enabled = false;
					firstPersonCamera.gameObject.SetActive(false);
					//thirdPersonCamera.gameObject.SetActive(true);
					RenderSettings.ambientLight = adventureModeAmbientLight;
					foreach (GameObject expolight in exposedLights) {
						expolight.SetActive(false);
					}

					oneHandJoint.gameObject.SetActive(true);
					twoHandsJoint.gameObject.SetActive(false);

					siljasAnimator.SetBool("darkmode", false);
				}
			}

	        yield return null;
    	}
    }

}
