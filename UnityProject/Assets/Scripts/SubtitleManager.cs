using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubtitleManager : MonoBehaviour {

	public GUIStyle style;
	public Material OutlineMaterial;
	private string currentText = "";

	public bool isPlaying = false;

	Dictionary<float, string> diarySubtitles = new Dictionary<float, string>()
	{
		{ 3f, "Dear Diary, something crazy happened today at school. "},
		{ 8f, "William sat next to me in class\nI was so happy!"},
		{ 15f, ""},
		{ 16f, "He asked me to go home with him,\nbut I couldn’t because of Dad."},
		{ 23f, ""},
		{ 24f, "In gym class, Laura kicked the ball so hard,\nthat it hit me in the face. "},
		{ 29f, ""},
		{ 30f, "And Dad is asleep. "},
		{ 32f, ""}
	};

	Dictionary<float, string> phoneSubtitles = new Dictionary<float, string>()
	{
		{ 1f, "Uh, hi this is Erik.\nI just called to check on you guys?"},
		{ 8f, "I think I hear some strange sounds, \ncoming from your floor."},
		{ 14f, "But you can call me,\nYou know where I live, so …"},
		{ 20f, ""},
		{ 21f, "And say to hello from Silja from me!\nBye."},
		{ 24f, ""},
		{ 28f, "Hi again, Erik at 237,\nIt’s because of the sounds."},
		{ 36f, "Do you want me to come down? \nOr? …"},
		{ 42f, "And say hello to Silja from me, \nby the way …"},
		{ 45f, "… Or call me, OK? \nGood. Bye."},
		{ 49f, "Hi, hi, hi … Erik, again. "},
		{ 54f, "I’m convinced the sounds\nare coming from your apartment."},
		{ 60f, "I’m starting to worry … \nPlease call me, OK. Bye."},
		{ 67f, ""}
	};

	Dictionary<float, string> picSubtitles = new Dictionary<float, string>()
	{
		{ 1f, "Do you remember all the fuzz Dad made,\nso you wouldn’t see his legs?"},
		{ 4f, "It was so silly!"},
		{ 7f, ""},
	};

	private static SubtitleManager _Instance;
	public static SubtitleManager Instance {
		get {return _Instance;}
	}

	void Awake(){
		_Instance = this;
	}

	public void Diary(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", diarySubtitles);
	}

	public void Phone(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", phoneSubtitles);
	}

	public void Portrait(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", picSubtitles);
	}

	private IEnumerator PlayDictionary(Dictionary<float,string> dict){

		isPlaying = true;

		float secondsCounter = 0f;
		foreach (float key in dict.Keys) {
			yield return new WaitForSeconds(key - secondsCounter);
			secondsCounter = key;

			string value = "";
			dict.TryGetValue(key,out value);
			//this.guiText.text = value;
			currentText = value;
		}
		//this.guiText.text = "";
		currentText = "";

		isPlaying = false;
	}

	public void Stop(){
		StopAllCoroutines ();
		currentText = "";

		isPlaying = false;
	}

	void OnGUI(){

		style.font.material = OutlineMaterial;

		GUI.Label (new Rect (Screen.width / 2, Screen.height * (3f/4f), -Screen.height * (8/10), 100), currentText, style);

	}
}
