using UnityEngine;
using System.Collections;

public class ButtonPrompt : MonoBehaviour {

	public HighlightedObject highlightedObject;
	public float distance = 0.5f;

	// Update is called once per frame
	void Update () {

		Vector3 desiredPosition = new Vector3(highlightedObject.transform.position.x,
		                                      highlightedObject.transform.position.y,
		                                      highlightedObject.transform.position.z);

		Vector3 modifier = Camera.main.gameObject.transform.position - desiredPosition;
		modifier.Normalize ();
		modifier *= distance;

		transform.position = desiredPosition + modifier;

		transform.LookAt(Camera.main.gameObject.transform);
	}
}
