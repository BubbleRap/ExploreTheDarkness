using UnityEngine;
using System.Collections;

public class DarknessLevel1 : MonoBehaviour {

	public Transform normalLights;
	public Transform horrorLights;
	public Camera firstPersonCam;
	public Camera thirdPersonCam;
	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Player")
		{
			Destroy(this);
			normalLights.gameObject.SetActive(false);
			horrorLights.gameObject.SetActive(true);
			firstPersonCam.clearFlags = CameraClearFlags.SolidColor;
			firstPersonCam.backgroundColor = Color.black;
			thirdPersonCam.clearFlags = CameraClearFlags.SolidColor;
			thirdPersonCam.backgroundColor = Color.black;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
