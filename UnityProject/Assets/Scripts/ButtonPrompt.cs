using UnityEngine;
using System.Collections;

public class ButtonPrompt : MonoBehaviour {

	public float distance = 0.5f;

	public Camera hudCamera;

	private Interactor interactor;
	private Transform connectedObject;

	void Start()
	{
		hudCamera = Camera.main;
	}

	void OnEnable(){
		foreach (TextMesh m in this.GetComponentsInChildren<TextMesh> ())
			m.color = new Color (
				1f, 1f, 1f, 0f);

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		interactor = siljaGO.GetComponent<Interactor>();
	}

	public void SetText(string text){
		foreach (TextMesh m in this.GetComponentsInChildren<TextMesh> ())
			m.text = text;
	}

	public void SetConnectedTransform(Transform t){
		connectedObject = t;
	}

	void Update () {

		float size = (hudCamera.gameObject.transform.position - transform.position).magnitude ;
		transform.localScale = new Vector3(size,size,size);

		foreach (TextMesh m in this.GetComponentsInChildren<TextMesh> ())
			m.color = new Color (
				1f, 1f, 1f, Mathf.Lerp(m.color.a, GetAlpha (),0.1f));

		transform.LookAt(hudCamera.gameObject.transform);
	}

	private float GetAlpha(){

		if (connectedObject != null &&
			connectedObject.GetComponent<Renderer> () != null &&
			!connectedObject.GetComponent<Renderer> ().isVisible)
			return 0f;
		else {
			Vector3 objToCamera = Camera.main.transform.position - connectedObject.position;
			if (Physics.Raycast(
				connectedObject.transform.position, 
				objToCamera, 
				objToCamera.magnitude,
				LayerMask.NameToLayer("Default")))
				return 0f;

			RaycastHit hit;

			if(Physics.Raycast(Camera.main.transform.position,-objToCamera, out hit, 10F,LayerMask.NameToLayer("Trigger"))){
				if(hit.transform.gameObject != connectedObject.gameObject)
				{
					return 0f;
				}
			}
		}

		/*
		Vector2 screenpos = Camera.main.WorldToScreenPoint (transform.position);
		screenpos.x /= Screen.width;
		screenpos.y /= Screen.height;

		screenpos.x = Mathf.Abs(0.5f - screenpos.x);
		screenpos.y = Mathf.Abs(0.5f - screenpos.y);
		float distFromCenter = Mathf.Max (screenpos.x, screenpos.y);
		distFromCenter *= 4;
		*/

		float distFromSilja = Vector3.Distance (connectedObject.position, interactor.transform.position);

		distFromSilja /= 4f;

		//float dist = Mathf.Max (distFromCenter, distFromSilja);
		float dist = distFromSilja;

		return 1f - (dist*dist);

	}
}
