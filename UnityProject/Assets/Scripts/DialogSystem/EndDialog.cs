using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class EndDialog : MonoBehaviour {

	public DialogChoices dialogManager;
	public Image endImage;
	public float fadeInImageBegin = 4.0f;
	public float fadeInTime = 1.0f;
	public float imageStayTime = 10.0f;
	public float fadeOutTime = 1.0f;
	private bool isActive = false;
	private bool doneLoading = false;
	private bool fadeBlack = false;

	public bool fadeInOnTrail = false;
	public AudioSource EndSong;
	public AudioSource EndTail;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(dialogManager.getIsEnd() && !isActive)
		{
			if(fadeInOnTrail)
			{
				if(!EndSong.isPlaying && !EndTail.isPlaying)
				{
					dialogManager.setNextID();
					endImage.enabled = true;
					StartCoroutine(FadeInBg(endImage,fadeInImageBegin,fadeInTime));
					isActive = true;
				}
			}
			else if(!fadeInOnTrail)
			{
				dialogManager.setNextID();
				endImage.enabled = true;
				StartCoroutine(FadeInBg(endImage,fadeInImageBegin,fadeInTime));
				isActive = true;
			}
		}
	}

	IEnumerator FadeInBg (Image fade, float delay, float time)
	{
		float colorAlpha = fade.color.a;
		float elapsedTime = 0;

		yield return new WaitForSeconds(delay);

		while(elapsedTime < time)
		{
			fade.color = new Color(fade.color.r,fade.color.g,fade.color.b,Mathf.Lerp(0,1,(elapsedTime / time)));

			elapsedTime += Time.deltaTime;

			yield return null;
		}

		StartCoroutine(FadeOutBg(endImage,imageStayTime,fadeOutTime));
	}

	IEnumerator FadeOutBg (Image fade, float delay, float time)
	{
		float colorAlpha = fade.color.a;
		float elapsedTime = 0;

		StartCoroutine(LoadLevel(Application.loadedLevel + 1));
		//yield return new WaitForSeconds(delay);

		while(!doneLoading)
		{
			yield return new WaitForSeconds(1f);
		}

		while(elapsedTime < time)
		{
			fade.color = new Color(fade.color.r,fade.color.g,fade.color.b,Mathf.Lerp(1,0,(elapsedTime / time)));

			elapsedTime += Time.deltaTime;

			yield return null;
		}

		fadeBlack = true;

		yield return null;
	}

	public IEnumerator LoadLevel(int levelNumber) {
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		AsyncOperation async = Application.LoadLevelAsync(levelNumber);
		async.allowSceneActivation = false;

		while(!async.isDone)
		{
			if(async.progress == 0.9f)
			{
				doneLoading = true;
			}
			if(fadeBlack)
			{
				async.allowSceneActivation = true;
			}
			yield return null;
		}

		yield return null;
	}
}
