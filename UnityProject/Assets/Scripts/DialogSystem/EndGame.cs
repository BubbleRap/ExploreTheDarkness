using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class EndGame : MonoBehaviour {

	public DialogChoices dialogManager;
	public Image fadeImage;
	private bool isActive = false;
	public float fadeInEndScreen = 4.0f;
	public float fadeInTime = 1.0f;
	public GameObject dialog;

	public GameObject endScreen;
	private bool fadeBlack = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(dialogManager.getIsEnd() && !isActive)
		{
			isActive = true;

			dialog.SetActive(false);
			//endScreen.SetActive(true);
			StartCoroutine(FadeInEndScreen(fadeImage,fadeInEndScreen,fadeInTime));
		}
	}

	IEnumerator FadeInEndScreen (Image fade, float delay, float time)
	{
		yield return new WaitForSeconds(delay);

		float colorAlpha = fade.color.a;
		float elapsedTime = 0;

		while(elapsedTime < time)
		{
			fade.color = new Color(fade.color.r,fade.color.g,fade.color.b,Mathf.Lerp(1,0,(elapsedTime / time)));

			elapsedTime += Time.deltaTime;

			yield return null;
		}

		endScreen.SetActive(true);

		fade.enabled = false;

		yield return null;
	}


	public void exitGame()
	{
		Application.Quit();
	}
}
