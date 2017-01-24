﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class DialogChoices : MonoBehaviour {

	private int ID = -1;
	private bool isEnd = false;

	public float fadeTimeBigText = 0.8f;
	public float fadeTimeOption = 0.8f;
	public float fadeInBlack = 1.0f;
	public float fadeOutBlack = 1.0f;

	public float beginingDelay = 8.0f;

	public float delayBetweenDialogs = 0.4f;

	public Setting[] settings;
	private int settingNumber = -1;

	public List<Dialog> Dialog;

	public Text Description;
	public GameObject Choice;
	public GameObject MultipleChoice;
	public Image backgroundImage;
	public Image fadeImage;

	private GameObject optionText1_single = null;
	private GameObject optionText1_multiple = null;
	private GameObject optionText2_multiple = null;

	private Coroutine choice1;
	private Coroutine choice2;
	private Coroutine choice3;

	private Coroutine choiceBlink1;

	private Color ChoiceColor;

	private bool isFaded = true;
	private bool isFadedBG = true;

	private int marginHeight = 20;

	// Use this for initialization
	void Start () {
		isFaded = true;
		isFadedBG = true;
		fadeImage.enabled = true;
		fadeImage.color = new Color(fadeImage.color.r,fadeImage.color.g,fadeImage.color.b,1.0f);
		StartCoroutine(FadeOutBgBegin(fadeImage,beginingDelay,fadeOutBlack));

		if(settings[0].changeAtID == 0)
		changeBackground(settings[0].background);

		Cursor.visible = true;
		Screen.lockCursor = false;

		clear();
	}
	
	// Update is called once per frame
	void Update () {
		if(ID < Dialog.Count && isFaded && isFadedBG && ID > -1)
		{
			if(Description.text != Dialog[ID].Text)
			{
				StartCoroutine(FadeIn(Description,fadeTimeBigText));
				Description.text = Dialog[ID].Text;
			}

			optionText1_single = Choice.transform.GetChild(0).gameObject;

			optionText1_multiple = MultipleChoice.transform.GetChild(0).gameObject;
			optionText2_multiple = MultipleChoice.transform.GetChild(1).gameObject;

			if(Description.color.a > 0.8f)
			{
				if(Dialog[ID].options.Length == 1)
				{
					Choice.SetActive(true);
					MultipleChoice.SetActive(false);

					Choice.transform.localPosition = new Vector2(0,-((Description.preferredHeight/2)+(marginHeight/2)));

					if(optionText1_single.GetComponent<Text>().text != Dialog[ID].options[0].Text)
					{
						choice1 = StartCoroutine(FadeIn(optionText1_single.GetComponent<Text>(),fadeTimeOption));
						optionText1_single.GetComponent<Text>().text = Dialog[ID].options[0].Text;

						if(ID == 0)
						{
							ChoiceColor = optionText1_single.GetComponent<Text>().color;
							choiceBlink1 = StartCoroutine(ChoiceBlink(optionText1_single.GetComponent<Text>(),10f,0.6f));
						}
					}
				}
				else
				{
					Choice.SetActive(false);
					MultipleChoice.SetActive(true);

					MultipleChoice.transform.localPosition = new Vector2(0,-((Description.preferredHeight/2)+marginHeight));

					if(optionText1_multiple.GetComponent<Text>().text != Dialog[ID].options[0].Text)
					{
						choice2 = StartCoroutine(FadeIn(optionText1_multiple.GetComponent<Text>(),fadeTimeOption));
						optionText1_multiple.GetComponent<Text>().text = Dialog[ID].options[0].Text;
					}

					if(optionText2_multiple.GetComponent<Text>().text != Dialog[ID].options[1].Text)
					{
						choice3 = StartCoroutine(FadeIn(optionText2_multiple.GetComponent<Text>(),fadeTimeOption));
						optionText2_multiple.GetComponent<Text>().text = Dialog[ID].options[1].Text;
					}
				}
			}
			else
			{
				optionText1_single.GetComponent<Text>().text = "";

				optionText1_multiple.GetComponent<Text>().text = "";
				optionText2_multiple.GetComponent<Text>().text = "";

				Choice.SetActive(false);
				MultipleChoice.SetActive(false);
			}
		}
	}

	public void dialogNext(int OptionNumber)
	{
		if(choice1 != null)
		{
			StopCoroutine(choice1);
		}

		if(choiceBlink1 != null)
		{
			StopCoroutine(choiceBlink1);
			optionText1_single.GetComponent<Text>().color = ChoiceColor;
		}

		if(choice2 != null)
		{
			StopCoroutine(choice2);
		}

		if(choice3 != null)
		{
			StopCoroutine(choice3);
		}

		if(Dialog[ID].options[OptionNumber].gotoID < Dialog.Count)
		{
			isFaded = false;

			StartCoroutine(FadeOut(Description,1));
			StartCoroutine(FadeOut(optionText1_single.GetComponent<Text>(),delayBetweenDialogs));
			StartCoroutine(FadeOut(optionText1_multiple.GetComponent<Text>(),delayBetweenDialogs));
			StartCoroutine(FadeOut(optionText2_multiple.GetComponent<Text>(),delayBetweenDialogs));

			ID = Dialog[ID].options[OptionNumber].gotoID;
			Dialog [ID].onDialogActivated.Invoke ();

			int prevSettingNumber = settingNumber;

			for(int i = 0; i < settings.Length; i++)
			{
				if(settings[i].changeAtID == ID)
				{
					settingNumber = i;
				}
			}

			if(settingNumber != prevSettingNumber)
			{
				isFadedBG = false;
				fadeImage.enabled = true;
				StartCoroutine(FadeInBg(fadeImage,fadeInBlack));
			}

			/*
			if(settingNumber < settings.Length)
			{
				if(settings[settingNumber].changeAtID == ID)
				{
					isFadedBG = false;
					fadeImage.enabled = true;
					StartCoroutine(FadeInBg(fadeImage,fadeInBlack));
					settingNumber ++;
				}
			}
			*/
		}
		else if(Dialog[ID].options[OptionNumber].gotoID >= Dialog.Count)
		{
			isEnd = true;
			fadeImage.enabled = true;
			ID = Dialog[ID].options[OptionNumber].gotoID;
			StartCoroutine(FadeInBg(fadeImage,fadeInBlack));
		}
	}

    public void MoveFromTo(int src, int dst)
    {
        Dialog item = Dialog[src];
        Dialog.RemoveAt(src);
        Dialog.Insert(dst, item);
    }

    public void SyncronizeIndexes()
    {
        for(int i = 0; i < Dialog.Count; i++ )
            Dialog[i].index.index = i;
    }

    public void SortDialogsByIndex()
    {
        Dialog.Sort( (Dialog left, Dialog right) => { return left.index.index.CompareTo(right.index.index);} );
    }

    public void IncrementIndeciesFrom(int fromIdx)
    {
        for( int i = fromIdx; i < Dialog.Count; i++ )
            Dialog[i].index.index++;
    }

	IEnumerator FadeIn (Text text, float time)
	{
		float colorAlpha = text.color.a;
		float elapsedTime = 0;

		while(elapsedTime < time)
		{
			text.color = new Color(text.color.r,text.color.g,text.color.b,Mathf.Lerp(0,1,(elapsedTime / time)));

			elapsedTime += Time.deltaTime;

			yield return null;
		}
	}

	IEnumerator FadeOut (Text text, float delay)
	{
		float colorAlpha = text.color.a;

		text.color = new Color(text.color.r,text.color.g,text.color.b,0);

		yield return new WaitForSeconds(delay);

		isFaded = true;
	}

	IEnumerator FadeInBg (Image fade, float time)
	{
		float colorAlpha = fade.color.a;
		float elapsedTime = 0;

		while(elapsedTime < time)
		{
			fade.color = new Color(fade.color.r,fade.color.g,fade.color.b,Mathf.Lerp(0,1,(elapsedTime / time)));

			elapsedTime += Time.deltaTime;

			yield return null;
		}

		if(!isEnd)
		{
			changeBackground(settings[settingNumber].background);
			StartCoroutine(FadeOutBg(fadeImage,fadeOutBlack));
		}
		else if(isEnd)
		{
			fade.color = new Color(fade.color.r,fade.color.g,fade.color.b,1);
		}
	}

	IEnumerator FadeOutBg (Image fade, float time)
	{
		float colorAlpha = fade.color.a;
		float elapsedTime = 0;

		while(elapsedTime < time)
		{
			fade.color = new Color(fade.color.r,fade.color.g,fade.color.b,Mathf.Lerp(1,0,(elapsedTime / time)));

			elapsedTime += Time.deltaTime;

			yield return null;
		}

		isFadedBG = true;
		fadeImage.enabled = false;
	}

	IEnumerator FadeOutBgBegin (Image fade, float delay, float time)
	{
		float colorAlpha = fade.color.a;
		float elapsedTime = 0;

		yield return new WaitForSeconds(delay);

		while(elapsedTime < time)
		{
			fade.color = new Color(fade.color.r,fade.color.g,fade.color.b,Mathf.Lerp(1,0,(elapsedTime / time)));

			elapsedTime += Time.deltaTime;

			yield return null;
		}

		isFadedBG = true;
		fadeImage.enabled = false;

		ID = 0;
	}

	IEnumerator FadeToNewScene (Image fade, float time)
	{
		float colorAlpha = fade.color.a;
		float elapsedTime = 0;

		if(Application.loadedLevel < Application.levelCount)
		{
			Application.LoadLevelAsync(Application.loadedLevel + 1);
		}

		while(elapsedTime < time)
		{
			fade.color = new Color(fade.color.r,fade.color.g,fade.color.b,Mathf.Lerp(0,1,(elapsedTime / time)));

			elapsedTime += Time.deltaTime;

			yield return null;
		}
	}

	IEnumerator ChoiceBlink (Text text, float delay, float time)
	{
		float elapsedTime = 0;
		float elapsedTime2 = 0;

		Color startColor = new Color(text.color.r,text.color.g,text.color.b);
		Color endColor = new Color(text.color.r+0.4f,text.color.g+0.3f,text.color.b+0.1f);

		yield return new WaitForSeconds(delay);

		while(elapsedTime < time)
		{
			text.color = Color.Lerp(startColor, endColor, (elapsedTime / time));

			elapsedTime += Time.deltaTime;

			yield return null;
		}

		yield return new WaitForSeconds(0.2f);

		while(elapsedTime2 < time)
		{
			text.color = Color.Lerp(endColor, startColor, (elapsedTime2 / time));

			elapsedTime2 += Time.deltaTime;

			yield return null;
		}

		yield return new WaitForSeconds(0.2f);

		choiceBlink1 = StartCoroutine(ChoiceBlink(text,0f,time));
	}

	public void changeBackground(Sprite background)
	{
		backgroundImage.sprite = background;
	}

	public int getID()
	{
		return ID;
	}

	public void clear()
	{
		Description.text = "";

		optionText1_single = Choice.transform.GetChild(0).gameObject;

		optionText1_multiple = MultipleChoice.transform.GetChild(0).gameObject;
		optionText2_multiple = MultipleChoice.transform.GetChild(1).gameObject;
		optionText1_single.GetComponent<Text>().text = "";

		optionText1_multiple.GetComponent<Text>().text = "";
		optionText2_multiple.GetComponent<Text>().text = "";
	}

	public bool getIsEnd()
	{
		return isEnd; 
	}

	public void setNextID()
	{
		ID++;
	}
}
