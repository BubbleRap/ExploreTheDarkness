﻿using UnityEngine;
using System.Collections;

public class EndScreenController : MonoBehaviour {

	public bool EndScreenActive = false;
	public Camera EndScreenCamera;

	public GameObject VictorScreen;
	public GameObject GameOverScreen;
	public GameObject LoadingScreen;
	private bool victor;

	public Color fadeColor = Color.black;
	private Texture2D fadeTexture = null;
	private float currentAlpha = 0f;
	public float fadeTime = 2f;

	void Awake()
	{
		fadeTexture = new Texture2D(1,1, TextureFormat.RGBA32, false, false);
		fadeTexture.SetPixel(0,0, fadeColor);
		fadeTexture.Apply();
	}

	// Update is called once per frame
	void Update () {
		
		if (EndScreenActive) {
			if (Input.GetKeyDown(KeyCode.E)){
				(GameObject.FindObjectOfType<MenuController> () as MenuController).Restart ();
			}
		}
	}
	
	public void ShowEndScreen(bool avictor){

		victor = avictor;
		Time.timeScale = 0.0001f;
		foreach (AudioSource audio in GameObject.FindObjectsOfType<AudioSource>()){
			if (audio.time != 0f){
				audio.Pause();
			}
		}

		StartCoroutine("FadeToEndScreen");
	}

	private AsyncOperation loadingProgress;
	public void ShowLoadingScreen(AsyncOperation loadingProgress){

		LoadingScreen.SetActive(true);
		EndScreenCamera.enabled = true;

		this.loadingProgress = loadingProgress;
		StartCoroutine("DisableLoadingScreen");
	}

	private IEnumerator DisableLoadingScreen(){

		/* Debug.Log("lol");

		float fadeStart = Time.time;
		
		while (Time.time < fadeStart + fadeTime){
			currentAlpha = (Time.time - fadeStart)/fadeTime;
			yield return null;
		}

		LoadingScreen.SetActive(true);
		EndScreenCamera.enabled = true;

		while (Time.time < fadeStart + fadeTime){
			currentAlpha = 1f - (Time.time - fadeStart)/fadeTime;
			yield return null;
		}
		
		currentAlpha = 0f;*/

		while (!loadingProgress.isDone){
			yield return null;
		}

		LoadingScreen.SetActive(false);
		EndScreenCamera.enabled = false;
	}

	private IEnumerator FadeToEndScreen(){

		fadeTime *= 0.0001f;
		float fadeStart = Time.time;

		while (Time.time < fadeStart + fadeTime){
			currentAlpha = (Time.time - fadeStart)/fadeTime;
			yield return null;
		}

		if (victor)
			VictorScreen.SetActive(true);
		else
			GameOverScreen.SetActive(true);

		EndScreenActive = true;
		EndScreenCamera.enabled = true;

		fadeStart = Time.time;
		
		while (Time.time < fadeStart + fadeTime){
			currentAlpha = 1f - (Time.time - fadeStart)/fadeTime;
			yield return null;
		}

		currentAlpha = 0f;
	}

	private void OnGUI() 
	{ 
		if(currentAlpha > 0f)
		{
			Color guiColor = fadeColor;
			guiColor.a = currentAlpha;
			GUI.color = guiColor;
		
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),fadeTexture);
		}
	}
}
