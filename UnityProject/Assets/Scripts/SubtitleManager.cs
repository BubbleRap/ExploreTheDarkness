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

	public void Diary(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", diarySubtitles);
	}

	Dictionary<float, string> phoneSubtitles = new Dictionary<float, string>()
	{
		{ 1f, "Hi this is Erik from 237. Just calling to check on you guys? "},
		{ 6f, ""},
		{ 7f, "I hear some strange sounds that might be from your floor.\nIf there’s anything I can do, please call me."},
		{ 10f, "You know where I live, just above you."},
		{ 14f, ""},
		{ 15f, "And say hello to Silja from me. Bye."},
		{ 17f, ""},
		{ 20f, "Uh, hi again, this is Erik again, from 237."},
		{ 24f, "It’s just... those sounds coming from below…"},
		{ 28f, "… do you want me to pop by?"},
		{ 30f, "Or call me OK? Bye"},
		{ 32f, ""},
		{ 35f, "Hi, hi, hi... it’s Erik … again …"},
		{ 39f, ""},
		{ 40f, "I just wanted to say that the sounds must be coming from your floor."},
		{ 44f, ""},
		{ 45f, "It’s making me a bit anxious…"},
		{ 48f, "Please call me right away. Bye."},
		{ 51f, ""},
	};

	public void Phone(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", phoneSubtitles);
	}

	Dictionary<float, string> picSubtitles = new Dictionary<float, string>()
	{
		{ 1f, "Do you remember all the fuzz Dad made,\nso you wouldn’t see his legs?"},
		{ 4f, "It was so silly!"},
		{ 7f, ""},
	};

	public void Portrait(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", picSubtitles);
	}


	Dictionary<float, string> farHalloSubs = new Dictionary<float, string>()
	{
		{ 1f, "Daddy? Hello?"},
		{ 3f, ""},
	};

	public void FarHallo(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", farHalloSubs);
	}


	Dictionary<float, string> farHoldOpSubs = new Dictionary<float, string>()
	{
		{ 1f, "Please daddy."},
		{ 3f, ""},
	};

	public void FarHoldOp(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", farHoldOpSubs);
	}

	Dictionary<float, string> farHvorErDu2Subs = new Dictionary<float, string>()
	{
		{ 1f, "Daddy?"},
		{ 3f, ""},
	};

	public void farHvorErDu2(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", farHvorErDu2Subs);
	}

	Dictionary<float, string> farHvorErDuSubs = new Dictionary<float, string>()
	{
		{ 0.1f, "Daddy, where are you?"},
		{ 3f, ""},
	};

	public void farHvorErDu(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", farHvorErDuSubs);
	}

	Dictionary<float, string> SorryDadSubs = new Dictionary<float, string>()
	{
		{ 1f, "Daddy, I'm sorry..."},
		{ 2f, "Daddy? Hello?"},
		{ 4f, ""},
	};

	public void SorryDad(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", SorryDadSubs);
	}

	Dictionary<float, string> WhereDoesErikLiveSubs = new Dictionary<float, string>()
	{
		{ 1f, "Lillebror. Can you remember where Erik lives?"},
		{ 4f, ""},
	};

	public void WhereDoesErikLive(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", WhereDoesErikLiveSubs);
	}

	Dictionary<float, string> DearDiary4Subs = new Dictionary<float, string>()
	{
		{ 3f, "Dear diary, something wonderful happened today."},
		{ 7f, "Christian sat next to me and asked me to come to his birthday party tomorrow."},
		{ 12f, "Dad told me it was OK if I went."},
		{ 15f, ""},
	};

	public void DearDiary4(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", DearDiary4Subs);
	}

	Dictionary<float, string> DoorLockedSubs = new Dictionary<float, string>()
	{
		{ 1f, "It’s locked..."},
		{ 4f, ""},
	};

	public void DoorLocked(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", DoorLockedSubs);
	}

	Dictionary<float, string> FieFarDuMaUnskyldeMia = new Dictionary<float, string>()
	{
		{ 1f, "Dad, I'm sorry..."},
		{ 4f, ""},
		{ 5f, "... Dad... Hello?"},
		{ 10f, ""},
	};

	public void FieFarDuMaUnskylde(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", FieFarDuMaUnskyldeMia);
	}

	Dictionary<float, string> DictaphoneSubs = new Dictionary<float, string>()
	{
		{ 1f, "Dad: It’s not Silja’s fault. What has she done?"},
		{ 4f, "Shrink: Of course not, but I think your blaming yourself for things you can’t control."},
		{ 8f, ""},
		{ 10f, "Dad: I just want her to be happy. You know?"},
		{ 16f, ""},
		{ 17f, "-	I just wish … I can never pay her back for losing her mom."},
		{ 23f, ""},
		{ 24f, "-	Isn’t this the part where you interject with some sort of wisdom?"},
		{ 28f, "Shrink: Sure, do you think these are things you can change?"},
		{ 30f, "Dad: Okay, what am I paying you by the hour again?"},
		{ 34f, "Shrink: … What are your hopes for Silja?"},
		{ 39f, "None of this, that’s for sure."},
		{ 43f, "You can’t blame yours..."},
		{ 45f, "And you’re repeating what yourself! I think it’s time to leave."},
		{ 49f, "Shrink:OK. Next week?\nMay be. Maybe I’m walking by then …"},
		{ 52f, "That was a joke … "},
	};

	public void Dictaphone(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", DictaphoneSubs);
	}

	Dictionary<float, string>	DaddysPillsSubs = new Dictionary<float, string>()
	{
		{ 1f, "Daddy’s pills."},
		{ 3f, ""},
	};

	public void DaddysPills(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", DaddysPillsSubs);
	}

	private static SubtitleManager _Instance;
	public static SubtitleManager Instance {
		get {return _Instance;}
	}

	void Awake(){
		_Instance = this;
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
