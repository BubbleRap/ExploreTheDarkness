using UnityEngine;
using System.Collections;

public class ButtonPrompt : MonoBehaviour {

	public float distance = 0.5f;

	public Camera hudCamera;

	void Start()
	{
		hudCamera = Camera.main;
	}

	void OnEnable(){
		foreach (TextMesh m in this.GetComponentsInChildren<TextMesh> ())
			m.color = new Color (
				1f, 1f, 1f, 0f);
	}

	public void SetText(string text){
		foreach (TextMesh m in this.GetComponentsInChildren<TextMesh> ())
			m.text = text;
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

		Vector2 screenpos = Camera.main.WorldToScreenPoint (transform.position);
		screenpos.x /= Screen.width;
		screenpos.y /= Screen.height;

		screenpos.x = Mathf.Abs(0.5f - screenpos.x);
		screenpos.y = Mathf.Abs(0.5f - screenpos.y);
		float distFromCenter = Mathf.Max (screenpos.x, screenpos.y);

		return 1f - 2*distFromCenter;

	}
}
