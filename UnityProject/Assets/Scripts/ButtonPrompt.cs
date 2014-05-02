using UnityEngine;
using System.Collections;

public class ButtonPrompt : MonoBehaviour {

	public HighlightedObject highlightedObject;
	public float distance = 0.5f;

	public Camera hudCamera;

	void Start()
	{
		//hudCamera = GameObject.FindGameObjectWithTag("HUDCamera").camera;
		hudCamera = Camera.main;
	}

	// Update is called once per frame
	void Update () {

		//Vector3 desiredPosition = new Vector3(highlightedObject.transform.position.x,
		//                                      highlightedObject.transform.position.y,
		//                                      highlightedObject.transform.position.z);
		//
		//Vector3 modifier = hudCamera.gameObject.transform.position - desiredPosition;
		//modifier.Normalize ();
		//modifier *= distance;
		//
		//transform.position = desiredPosition + modifier;

		float size = (hudCamera.gameObject.transform.position - transform.position).magnitude ;
		transform.localScale = new Vector3(size,size,size);
		transform.LookAt(hudCamera.gameObject.transform);
	}
}
