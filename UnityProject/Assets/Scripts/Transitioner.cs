using UnityEngine;
using System.Collections;

public class Transitioner : MonoBehaviour {

	public Color adventureModeAmbientLight;
	public Color shadowModeAmbientLight;
	[HideInInspector]
	public bool darkMode = false;
	public Transform directionalLight;
	private GameObject[] exposedLights;

	public Transform lightRoom;
	public Transform darkRoom;

	public float transitionTime = 17f;
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

		StartCoroutine(switchToDarkMode(transitionTime));

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
				if(!darkRoom.gameObject.activeInHierarchy)
				{
					siljaCharacter = GameObject.FindGameObjectWithTag("Player");

					MovementController moveCtrl = siljaCharacter.GetComponent<MovementController>();
					moveCtrl.canMove = false;

					Camera.main.gameObject.GetComponent<CameraFollow>().horizontalShakeIntensity = 0.1f;
					Camera.main.gameObject.GetComponent<CameraFollow>().verticalShakeIntensity = 0.1f;
			        yield return new WaitForSeconds(waitTime);

					moveCtrl.canMove = true;

			        darkRoom.gameObject.SetActive(true);
					lightRoom.gameObject.SetActive(false);
					directionalLight.gameObject.SetActive(false);

					RenderSettings.ambientLight = shadowModeAmbientLight;
					RenderSettings.fogColor = shadowModeAmbientLight;
					foreach (GameObject expolight in exposedLights) {
						expolight.SetActive(true);
					}

					siljaCharacter.SendMessage("EnableDarkMode", SendMessageOptions.RequireReceiver);
				}
			}
			else
			{
				if(darkRoom.gameObject.activeInHierarchy)
				{
					siljaCharacter = GameObject.FindGameObjectWithTag("Player");

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
				}
			}

	        yield return null;
    	}
    }

}
