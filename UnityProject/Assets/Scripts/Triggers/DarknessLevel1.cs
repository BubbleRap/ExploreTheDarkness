using UnityEngine;
using System.Collections;

public class DarknessLevel1 : MonoBehaviour {

	public Transform normalLights;
	public Transform horrorLights;
	public Camera firstPersonCam;
	public Camera thirdPersonCam;
	public Transform lightProbe;
	public AudioSource roomAmbience;
	public AudioClip scarySounds;
	public AudioSource outsideAmbience;
	public AudioClip windSounds;
	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Player")
		{
			Destroy(this);
			roomAmbience.clip = scarySounds;
			roomAmbience.Play();
			roomAmbience.volume = 1.0f;
			outsideAmbience.clip = windSounds;
			outsideAmbience.Play();
			outsideAmbience.volume = 0.3f;
			normalLights.gameObject.SetActive(false);
			horrorLights.gameObject.SetActive(true);
			firstPersonCam.clearFlags = CameraClearFlags.SolidColor;
			firstPersonCam.backgroundColor = Color.black;
			thirdPersonCam.clearFlags = CameraClearFlags.SolidColor;
			thirdPersonCam.backgroundColor = Color.black;
			lightProbe.gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
