﻿using UnityEngine;
using System.Collections;

public class Transitioner : MonoBehaviour {

	public Color adventureModeAmbientLight;
	public Color shadowModeAmbientLight;
	[HideInInspector]
	public bool darkMode = false;
	public Transform directionalLight;
	private GameObject[] exposedLights;
//	public Transform firstPersonCamera;
	
	public Transform lightRoom;
	public Transform darkRoom;


	public Transform elevator;
	private GameObject siljaCharacter = null;

	void Awake()
	{
		siljaCharacter = GameObject.FindGameObjectWithTag("Player");
	}

	void Start () {
        exposedLights = GameObject.FindGameObjectsWithTag("ExposedLight");
        foreach (GameObject expolight in exposedLights) {
			expolight.SetActive(false);
		}

		StartCoroutine(switchToDarkMode(2.0F));

		doTransition(darkMode);
	}

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
				//if(!firstPersonCamera.gameObject.activeInHierarchy)
				//{
					Camera.main.gameObject.GetComponent<CameraFollow>().horizontalShakeIntensity = 1.0f;
					Camera.main.gameObject.GetComponent<CameraFollow>().verticalShakeIntensity = 1.0f;
			        yield return new WaitForSeconds(waitTime);

			        darkRoom.gameObject.SetActive(true);
					lightRoom.gameObject.SetActive(false);
					directionalLight.gameObject.SetActive(false);

					RenderSettings.ambientLight = shadowModeAmbientLight;
					foreach (GameObject expolight in exposedLights) {
						expolight.SetActive(true);
					}

					siljaCharacter.SendMessage("EnableDarkMode", SendMessageOptions.RequireReceiver);

					elevator.gameObject.SetActive(false);
				//}
			}
			else
			{
				//if(firstPersonCamera.gameObject.activeInHierarchy && !transform.gameObject.GetComponent<interact>().isInteractMode)
				//{
					Camera.main.gameObject.GetComponent<CameraFollow>().horizontalShakeIntensity = 0.0f;
					Camera.main.gameObject.GetComponent<CameraFollow>().verticalShakeIntensity = 0.0f;

					darkRoom.gameObject.SetActive(false);
					lightRoom.gameObject.SetActive(true);
					directionalLight.gameObject.SetActive(true);

					RenderSettings.ambientLight = adventureModeAmbientLight;
					foreach (GameObject expolight in exposedLights) {
						expolight.SetActive(false);
					}

					siljaCharacter.SendMessage("EnableStoryMode", SendMessageOptions.RequireReceiver);

					elevator.gameObject.SetActive(true);
				//}
			}

	        yield return null;
    	}
    }

}
