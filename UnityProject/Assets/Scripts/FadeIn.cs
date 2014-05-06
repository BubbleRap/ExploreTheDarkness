using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeIn : MonoBehaviour {

	public Color fadeColor = Color.black;
	private Texture2D fadeTexture = null; 

	public bool stillBlack = true;
	public bool isTitleScreen = false;
	public bool stillFading = true;

	private float titleStart = float.MaxValue;
	public float titleDuration = 3f;

	private float fadeStart = float.MaxValue; 
	public float fadeDuration = 3f;

	public AudioSource mainTheme;
	private GameObject siljaCharacter = null;

	public Texture titleTexture;

	Dictionary<float, string> subtitles = new Dictionary<float, string>()
	{
		{ 1f, "Silja: Dad, it’s always like this \n Father: You know it’s not my fault"},
		{ 5f, "Silja: Still, you’re not without blame. \n Father: Okay. So it’s my fault he called in sick?."},
		{ 10f, "Silja: You should have droven more carefully, \n then this wouldn’t be a problem."},
		{ 15f, "Father: You better stay here!  \n That’s the least you can do, after all I’ve done for you …"},
		{ 21f, ""},
		{ 23f, "- … Silja, listen. It’s been a very long day. \n Silja: I don’t see how? You never do anything!"},
		{ 30f, "Father: Silja, just please start on dinner, will you? \n Silja: No."},
		{ 34f, "Silja, I need you to grow up. \n - I’ll need help going to to bathroom soon."},
		{ 41f, "- Silja? You can’t just leave. \n Silja: I can."},
		{ 45f, "Father: Silja, for crying out - please come back. Come back!"},
		{ 52f, ""},
	};

	public GUIStyle style;
	private string currentText = "";

	void Awake()
	{
		fadeTexture = new Texture2D(1,1, TextureFormat.RGBA32, false, false);
		fadeTexture.SetPixel(0,0, fadeColor);
		fadeTexture.Apply();
	}

	void Start(){
		StartCoroutine("PlaySubtitles");

		siljaCharacter = GameObject.FindGameObjectWithTag("Player");
		siljaCharacter.GetComponent<MovementController>().canMove = false;
	}

	private IEnumerator PlaySubtitles(){

		float secondsCounter = 0f;
		foreach (float key in subtitles.Keys) {
			yield return new WaitForSeconds(key - secondsCounter);
			secondsCounter = key;
			
			string value = "";
			subtitles.TryGetValue(key,out value);
			//this.guiText.text = value;
			currentText = value;
		}
		//this.guiText.text = "";
		currentText = "";		
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.E) ||
		    Input.GetKeyDown(KeyCode.Space) ||
		    Input.GetKeyDown(KeyCode.Return) ||
		    Input.GetMouseButtonDown(0)){
		    audio.Stop();
		}

		if (!audio.isPlaying && !isTitleScreen)
		{
			StopAllCoroutines ();
			currentText = "";
			
			isTitleScreen = true;

			titleStart = Time.time;
		}

		if (Time.time - titleStart > titleDuration && stillBlack)
		{
			stillBlack = false;
			fadeStart = Time.time;
		}
		
		if (Time.time - fadeStart > fadeDuration)
		{
			stillFading = false;
		}
	}

	private void OnGUI() 
	{ 
		if( !stillBlack && stillFading)
		{
			Color guiColor = fadeColor;
			guiColor.a = 1f - (Time.time - fadeStart)/fadeDuration;
			GUI.color = guiColor;
		}

		if (stillFading) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTexture);
			
			if(isTitleScreen)
			{
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), titleTexture, ScaleMode.ScaleToFit, true, 0.0F);
			}

			if (currentText.Length != 0){
				GUI.color = Color.white;
				GUI.Label (new Rect (Screen.width / 2, Screen.height * (3f/4f), -Screen.height * (8/10), 100), currentText, style);
			}
		} else {
			siljaCharacter.GetComponent<AudioSource>().Play();
			siljaCharacter.GetComponent<MovementController>().canMove = true;
			mainTheme.Play();
			MonoBehaviour.Destroy(this);
		}
	}
}