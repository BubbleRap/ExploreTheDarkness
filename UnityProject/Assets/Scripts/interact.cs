using UnityEngine;
using System.Collections;

public class interact : MonoBehaviour {

	public Transform firstPersonCamera;
	public Color adventureModeAmbientLight;
	public Color shadowModeAmbientLight;
	bool isInteractMode = false;
	bool darkMode = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isInteractMode && Input.GetKeyDown(KeyCode.E))
		{
			firstPersonCamera.gameObject.SetActive(false);
			transform.gameObject.GetComponent<ThirdPersonController>().canMove = true;
			isInteractMode = false;
			/*
			foreach (Transform child in transform){
				if(child.tag == "Player")
				{
					child.gameObject.SetActive(true);
				}
			}
			*/
		}
		else
		{
			Vector3 fwd = transform.TransformDirection(Vector3.forward);
			RaycastHit[] hits;
			hits = Physics.RaycastAll(transform.position, fwd, 10);
			int i = 0;
			while (i < hits.Length) {

				RaycastHit hit = hits[i];
				if(hit.transform.tag == "Object")
				{
					if(Input.GetKeyDown(KeyCode.E))
					{
						firstPersonCamera.gameObject.SetActive(true);
						firstPersonCamera.LookAt(hit.transform);
						transform.gameObject.GetComponent<ThirdPersonController>().canMove = false;
						//Destroy(hit.transform.gameObject);
						isInteractMode = true;
						/*
						foreach (Transform child in transform){
							if(child.tag == "Player")
							{
								child.gameObject.SetActive(false);
							}
						}
						*/
					}
				}
				i++;
			}
		}

		if(Input.GetKeyDown(KeyCode.R))
		{
			if(darkMode)
			{
				RenderSettings.ambientLight = adventureModeAmbientLight;
				darkMode = false;
			}
			else
			{
				RenderSettings.ambientLight = shadowModeAmbientLight;
				darkMode = true;
			}
		}
	}
}
