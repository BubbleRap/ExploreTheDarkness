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
		{ 1f, "Hi this is Erik from upstairs, i’ve been trying to call you..."},
		{ 6f, ""},
		{ 7f, "I know that my party dosent start until an hour, \nbut I can hear that you are home."},
		{ 14f, "Do you need help getting up here?"},
		{ 17f, ""},
		{ 18f, "I’m hearing wired noises coming from your apartment..."},
		{ 22f, ""},
		{ 23f, "I’m worried..."},
		{ 25f, "" },
		{ 27f, "I’m coming down now to check on you, \ni'll bring the Die Heart VHS that I borrowed."},
		{ 34f, "" },
	};

	public void Phone(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", phoneSubtitles);
	}

	Dictionary<float, string> electricitySubtitles = new Dictionary<float, string>()
	{
		{ 1f, "I can't remember how many times I had to replace the fuse."},
		{ 4f, "By now I must be and expert at it."},
		{ 8f, ""},
	};

	public void Electricity(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", electricitySubtitles);
	}

	Dictionary<float, string> picSubtitles = new Dictionary<float, string>()
	{
		{ 1f, "Dad always took the time, to hide the fact that he was in a wheelchair.\n Especially on photos."},
		{ 8f, ""},
	};

	public void Portrait(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", picSubtitles);
	}

	Dictionary<float, string> dateSubtitles = new Dictionary<float, string>()
	{
		{ 1f, "Tuesday, the 21th of September."},
		{ 4f, ""},
	};

	public void Calendar(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", dateSubtitles);
	}

	Dictionary<float, string> postcardsSubtitles = new Dictionary<float, string>()
	{
		{ 1f, "Of all the places, Paris would properly be the place to go."},
		{ 5f, ""},
	};

	public void Postcards(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", postcardsSubtitles);
	}

	Dictionary<float, string> darknessSubtitles = new Dictionary<float, string>()
	{
		{ 1f, "It's too dark."},
		{ 3f, ""},
	};

	public void Darkness(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", darknessSubtitles);
	}

	Dictionary<float, string> darkness2Subtitles = new Dictionary<float, string>()
	{
		{ 1f, "Maybe I should use my old flashlight?"},
		{ 5f, ""},
	};

	public void Darkness2(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", darkness2Subtitles);
	}

	Dictionary<float, string> darkness3Subtitles = new Dictionary<float, string>()
	{
		{ 1f, "Hmmm... No batteries"},
		{ 5f, ""},
	};

	public void Darkness3(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", darkness3Subtitles);
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

	public void FarHvorErDu2(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", farHvorErDu2Subs);
	}

	Dictionary<float, string> farHvorErDuSubs = new Dictionary<float, string>()
	{
		{ 0.1f, "Daddy, where are you?"},
		{ 3f, ""},
	};

	public void FarHvorErDu(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", farHvorErDuSubs);
	}

	Dictionary<float, string> SorryDadSubs = new Dictionary<float, string>()
	{
		{ 4f, "Daddy, I'm sorry..."},
		{ 7f, "Daddy? Hello?"},
		{ 10f, ""},
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
		{ 0.01f, "Daddy’s pills."},
		{ 3f, ""},
	};

	public void DaddysPills(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", DaddysPillsSubs);
	}

	Dictionary<float, string>	MelodiSubs = new Dictionary<float, string>()
	{
		{ 12f, "What do you think, Lillebror? I made it on my own."},
		{ 16f, "Whaat, I think I know that better than you!"},
		{ 21f, "Really? Of course I do!"},
		{ 24f, ""},
	};
	
	public void Melodi(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", MelodiSubs);
	}

	Dictionary<float, string> TrorDuViKanSubs = new Dictionary<float, string>()
	{
		{ 2f, " Lillebror do you.. Do you think that we may enter?"},
		{ 5f, ""},
	};
	
	public void TrorDuViKan(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", TrorDuViKanSubs);
	}

	Dictionary<float, string> ViMaFindeSubs = new Dictionary<float, string>()
	{
		{ 2f, "Lillebror, I have to find someone who can help me find dad!"},
		{ 5f, ""},
	};
	
	public void ViMaFinde(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", ViMaFindeSubs);
	}

	Dictionary<float, string> FarErDuHinSubs = new Dictionary<float, string>()
	{
		{ 3f, "Daddy? Are you here?"},
		{ 5f, ""},
	};
	
	public void FarErDuHin(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", FarErDuHinSubs);
	}

	Dictionary<float, string> ManyPicturesSubs = new Dictionary<float, string>()
	{
		{ 2f, "Wow... Look at that!"},
		{ 5f, "He has so many photos..."},
		{ 7f, "He surely has a lot of children!"},
		{ 9f, ""},
	};
	
	public void ManyPictures(){
		StopAllCoroutines ();
		StartCoroutine ("PlayDictionary", ManyPicturesSubs);
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
