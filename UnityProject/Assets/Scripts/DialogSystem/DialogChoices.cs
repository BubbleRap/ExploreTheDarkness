using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class DialogChoices : MonoBehaviour {

	[TextArea(3,10)]
	public string[] dialog;
	private int ID = 0;

	[TextArea(3,10)]
	public string[] option1;
	[TextArea(3,10)]
	public string[] option2;

	public Dialog[] Dialog;

	public Text Description;
	public GameObject Choice;
	public GameObject MultipleChoice;

	private int marginHeight = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(ID <= Dialog.Length)
		{
			if(Description.text != Dialog[ID].Text)
			{
				Description.text = Dialog[ID].Text;
			}

			GameObject optionText1 = null;
			GameObject optionText2 = null;

			if(Dialog[ID].options.Length == 1)
			{
				Choice.SetActive(true);
				MultipleChoice.SetActive(false);

				Choice.transform.localPosition = new Vector2(0,-((Description.preferredHeight/2)+(marginHeight/2)));

				optionText1 = Choice.transform.GetChild(0).gameObject;
				optionText2 = MultipleChoice.transform.GetChild(0).gameObject;

				if(optionText1.GetComponent<Text>().text != Dialog[ID].options[0].Text)
				{
					optionText1.GetComponent<Text>().text = Dialog[ID].options[0].Text;
				}
			}
			else
			{
				Choice.SetActive(false);
				MultipleChoice.SetActive(true);

				MultipleChoice.transform.localPosition = new Vector2(0,-((Description.preferredHeight/2)+marginHeight));

				optionText1 = MultipleChoice.transform.GetChild(0).gameObject;
				optionText2 = MultipleChoice.transform.GetChild(1).gameObject;

				if(optionText1.GetComponent<Text>().text != Dialog[ID].options[0].Text)
				{
					optionText1.GetComponent<Text>().text = Dialog[ID].options[0].Text;
				}

				if(optionText2.GetComponent<Text>().text != Dialog[ID].options[1].Text)
				{
					optionText2.GetComponent<Text>().text = Dialog[ID].options[1].Text;
				}
			}
		}

		/*
		if(ID <= option1.Length)
		{
			GameObject optionText1 = null;
			GameObject optionText2 = null;

			if(option2[ID] == "")
			{
				Choice.SetActive(true);
				MultipleChoice.SetActive(false);

				optionText1 = Choice.transform.GetChild(0).gameObject;
				optionText2 = MultipleChoice.transform.GetChild(0).gameObject;
			}
			else
			{
				Choice.SetActive(false);
				MultipleChoice.SetActive(true);

				optionText1 = MultipleChoice.transform.GetChild(0).gameObject;
				optionText2 = MultipleChoice.transform.GetChild(1).gameObject;
			}

			if(optionText1.GetComponent<Text>().text != option1[ID])
			{
				optionText1.GetComponent<Text>().text = option1[ID];
			}

			if(optionText2.GetComponent<Text>().text != option2[ID])
			{
				optionText2.GetComponent<Text>().text = option2[ID];
			}
		}
		*/
	}

	public void dialogNext(int OptionNumber)
	{
		if(Dialog[ID].options[OptionNumber].gotoID <= Dialog.Length)
		{
			ID = Dialog[ID].options[OptionNumber].gotoID;
		}
	}
}
