using UnityEngine;
using System.Collections;

public class interact : MonoBehaviour {

	public Transform firstPersonCamera;
	public Transform thirdPersonCamera;
	bool isFirstPerson = false;
	[HideInInspector]
	public bool isInteractMode = false;
	private Transform interactedObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(interactedObject != null)
		{
			if(interactedObject.GetComponent<HighlightedObject>().StoppedPlaying() && interactedObject.GetComponent<HighlightedObject>().soundClip != null)
			{
				if(isFirstPerson)
				{
					firstPersonCamera.gameObject.SetActive(false);
					transform.gameObject.GetComponent<MovementController>().canMove = true;
					isInteractMode = false;
				}
				interactedObject = null;
			}
		}
		if (isInteractMode && Input.GetKeyDown(KeyCode.E))
		{
			if(isFirstPerson)
			{
				firstPersonCamera.gameObject.SetActive(false);
			}
			transform.gameObject.GetComponent<MovementController>().canMove = true;
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
		else if(!isInteractMode)
		{
			Vector3 fwd = thirdPersonCamera.TransformDirection(Vector3.forward);
			RaycastHit[] hits;
			hits = Physics.RaycastAll(thirdPersonCamera.position, fwd, 4);
			int i = 0;
			while (i < hits.Length) {

				RaycastHit hit = hits[i];
				if(hit.transform.tag == "Object")
				{
					hit.transform.GetComponent<HighlightedObject>().hitObject = true;
					if(Input.GetKeyDown(KeyCode.E))
					{
						if(hit.transform.GetComponent<HighlightedObject>() != null)
						{
							interactedObject = hit.transform;
							if(hit.transform.GetComponent<HighlightedObject>().firstperson)
							{
								firstPersonCamera.gameObject.SetActive(true);
								isFirstPerson = true;
								firstPersonCamera.LookAt(hit.transform);
								transform.gameObject.GetComponent<MovementController>().canMove = false;
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
							if(hit.transform.GetComponent<HighlightedObject>().soundClip != null)
							{
								hit.transform.GetComponent<HighlightedObject>().PlayAudio();
							}
						}
					}
				}
				i++;
			}
		}
	}
}
