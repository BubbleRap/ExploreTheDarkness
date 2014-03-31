using UnityEngine;
using System.Collections;

public class Transitioner : MonoBehaviour {

	public Color adventureModeAmbientLight;
	public Color shadowModeAmbientLight;
	bool darkMode = false;
	public Transform directionalLight;
	private GameObject[] exposedLights;
	public Transform firstPersonCamera;
	public Transform thirdPersonCamera;
	public Transform thirdPersonCharacterModel;
	public Transform lightRoom;
	public Transform darkRoom;

	// Use this for initialization
	void Start () {
        exposedLights = GameObject.FindGameObjectsWithTag("ExposedLight");
        foreach (GameObject expolight in exposedLights) {
			expolight.SetActive(false);
		}
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
			darkRoom.gameObject.SetActive(true);
			lightRoom.gameObject.SetActive(false);
			directionalLight.gameObject.SetActive(false);
			//thirdPersonCharacterModel.gameObject.SetActive(false);
			transform.gameObject.GetComponent<ThirdPersonController>().enabled = false;
			transform.gameObject.GetComponent<CharacterMotor>().enabled = true;
			transform.gameObject.GetComponent<FPSInputController>().enabled = true;
			transform.gameObject.GetComponent<MouseLook>().enabled = true;
			firstPersonCamera.gameObject.SetActive(true);
			//thirdPersonCamera.gameObject.SetActive(false);
			RenderSettings.ambientLight = shadowModeAmbientLight;
			foreach (GameObject expolight in exposedLights) {
				expolight.SetActive(true);
			}

			darkMode = true;
		}
		else
		{
			darkRoom.gameObject.SetActive(false);
			lightRoom.gameObject.SetActive(true);
			directionalLight.gameObject.SetActive(true);
			//thirdPersonCharacterModel.gameObject.SetActive(true);
			transform.gameObject.GetComponent<ThirdPersonController>().enabled = true;
			transform.gameObject.GetComponent<CharacterMotor>().enabled = false;
			transform.gameObject.GetComponent<FPSInputController>().enabled = false;
			transform.gameObject.GetComponent<MouseLook>().enabled = false;
			firstPersonCamera.gameObject.SetActive(false);
			//thirdPersonCamera.gameObject.SetActive(true);
			RenderSettings.ambientLight = adventureModeAmbientLight;
			foreach (GameObject expolight in exposedLights) {
				expolight.SetActive(false);
			}
			darkMode = false;
		}
	}
}
