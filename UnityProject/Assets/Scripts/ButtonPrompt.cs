using UnityEngine;
using System.Collections;

public class ButtonPrompt : MonoBehaviour {

	public HighlightedObject highlightedObject;
	public float distance = 0.5f;

	public Camera hudCamera;

	void Start()
	{
		hudCamera = Camera.main;
	}

	void Update () {

		float size = (hudCamera.gameObject.transform.position - transform.position).magnitude ;
		transform.localScale = new Vector3(size,size,size);
		transform.LookAt(hudCamera.gameObject.transform);
	}
}
