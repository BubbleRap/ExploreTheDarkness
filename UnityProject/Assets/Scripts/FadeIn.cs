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
		{ 1f, "Those subtitles will"},
		{ 4f, "Display during the intro."},
		{ 8f, ""},
		{ 10f, "They're cool."},
		{ 14f, ""},
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