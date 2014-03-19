using UnityEngine;
using System.Collections;

public class Transitioner : MonoBehaviour {

	public Color adventureModeAmbientLight;
	public Color shadowModeAmbientLight;
	bool darkMode = false;
	public Transform directionalLight;
	private GameObject[] exposedLights;

	// Use this for initialization
	void Start () {
        exposedLights = GameObject.FindGameObjectsWithTag("ExposedLight");
        foreach (GameObject expolight in exposedLights) {
			expolight.SetActive(false);
		}
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
			directionalLight.gameObject.SetActive(false);
			RenderSettings.ambientLight = shadowModeAmbientLight;
			foreach (GameObject expolight in exposedLights) {
				expolight.SetActive(true);
			}
			darkMode = true;
		}
		else
		{
			directionalLight.gameObject.SetActive(true);
			RenderSettings.ambientLight = adventureModeAmbientLight;
			foreach (GameObject expolight in exposedLights) {
				expolight.SetActive(false);
			}
			darkMode = false;
		}
	}
}
