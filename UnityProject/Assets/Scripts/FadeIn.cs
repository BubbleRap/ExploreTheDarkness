using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {

	public Texture background;
	public bool stillBlack = true;
	public bool stillFading = true;
	public float fadeTime = 3f;
	public float fadeStart = 0f;

	private void OnGUI() { 
		if (stillBlack){

			GUI.Box(new Rect(0,0,Screen.width,Screen.height),background);

			if (Input.GetKeyDown(KeyCode.E)){
				stillBlack = false;
				fadeStart = Time.time;
			}

		} else if (stillFading){

			if (Time.time - fadeStart > fadeTime){
				stillFading = false;
			} else {

				Color guiColor = Color.black;
				guiColor.a = (Time.time - fadeStart)/(fadeTime);

				GUI.color = guiColor;

				GUI.Box(new Rect(0,0,Screen.width,Screen.height),background);

			}
		}
	}
