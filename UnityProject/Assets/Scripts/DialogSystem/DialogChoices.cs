using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class DialogChoices : MonoBehaviour {

	private int ID = 0;

	public float fadeTimeBigText = 0.8f;
	public float fadeTimeOption = 0.8f;
	public float fadeInBlack = 1.0f;
	public float fadeOutBlack = 1.0f;

	public float delayBetweenDialogs = 0.4f;

	public Setting[] settings;
	private int settingNumber = 0;

	public List<Dialog> Dialog;

	public Text Description;
	public GameObject Choice;
	public GameObject MultipleChoice;
	public Image backgroundImage;
	public Image fadeImage;

	private GameObject optionText1_single = null;
	private GameObject optionText1_multiple = null;
	private GameObject optionText2_multiple = null;

	private bool isFaded = true;
	private bool isFadedBG = true;

	private int marginHeight = 20;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(ID <= Dialog.Count && isFaded && isFadedBG)
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
						StartCoroutine(FadeIn(optionText1_single.GetComponent<Text>(),fadeTimeOption));
						optionText1_single.GetComponent<Text>().text = Dialog[ID].options[0].Text;
					}
				}
				else
				{
					Choice.SetActive(false);
					MultipleChoice.SetActive(true);

					MultipleChoice.transform.localPosition = new Vector2(0,-((Description.preferredHeight/2)+marginHeight));

					if(optionText1_multiple.GetComponent<Text>().text != Dialog[ID].options[0].Text)
					{
						StartCoroutine(FadeIn(optionText1_multiple.GetComponent<Text>(),fadeTimeOption));
						optionText1_multiple.GetComponent<Text>().text = Dialog[ID].options[0].Text;
					}

					if(optionText2_multiple.GetComponent<Text>().text != Dialog[ID].options[1].Text)
					{
						StartCoroutine(FadeIn(optionText2_multiple.GetComponent<Text>(),fadeTimeOption));
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
		if(Dialog[ID].options[OptionNumber].gotoID <= Dialog.Count && Dialog[ID].options[OptionNumber].gotoID != 0)
		{
			isFaded = false;
			StartCoroutine(FadeOut(Description,1));
			StartCoroutine(FadeOut(optionText1_single.GetComponent<Text>(),delayBetweenDialogs));
			StartCoroutine(FadeOut(optionText1_multiple.GetComponent<Text>(),delayBetweenDialogs));
			StartCoroutine(FadeOut(optionText2_multiple.GetComponent<Text>(),delayBetweenDialogs));

			ID = Dialog[ID].options[OptionNumber].gotoID;

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
		}

		if(Dialog[ID].options[OptionNumber].gotoID == 0)
		{
			fadeImage.enabled = true;
			StartCoroutine(FadeToNewScene(fadeImage,fadeInBlack));
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

		changeBackground(settings[settingNumber - 1].background);
		StartCoroutine(FadeOutBg(fadeImage,fadeOutBlack));
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

	public void changeBackground(Sprite background)
	{
		backgroundImage.sprite = background;
	}

	public int getID()
	{
		return ID;
	}
}
