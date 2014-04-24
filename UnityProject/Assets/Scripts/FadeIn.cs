using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {

	public Texture black;
	public bool stillBlack = true;
	public bool stillFading = true;
	public float fadeTime = 3f;
	public float fadeStart = 0f; 

	private static string intro = 
		"- No, I don't have time! You can do it yourself once!\n"+
			"- Don't be stupid, Silja, you know I can't!\n"+
			"- No! I'm APPOINTED!\n"+
			"- APPOINTED? You're NOT!\n\n"+
			"[Door slams]\n"+
			"- Silja!\n\n\n\n\n\n"+
			"[Press E]";

	public GUIStyle style;

	private void OnGUI() { 
		if (stillBlack){

			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),black);
			GUI.Label (new Rect (Screen.width / 2, Screen.height/2, 0, Screen.height * (8 / 10)), intro, style);

			if (Input.GetKeyDown(KeyCode.E)){
				stillBlack = false;
				fadeStart = Time.time;
			}

		} else if (stillFading){

			if (Time.time - fadeStart > fadeTime){
				stillFading = false;
			} else {

				Color guiColor = Color.black;
				guiColor.a = 1f - (Time.time - fadeStart)/(fadeTime);

				GUI.color = guiColor;

				GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),black);

			}
		}
	}
}