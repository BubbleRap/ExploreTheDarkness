using UnityEngine;
using System.Collections;

public class Objectives : MonoBehaviour {

	private GUIStyle ObjectiveUIFont;
	private string theObjective = "";
	private bool haveObjective = false;
	private int margin = 10;

	// Use this for initialization
	void Start () {
		ObjectiveUIFont = new GUIStyle();
        ObjectiveUIFont.fontSize = Mathf.Min(Screen.width, Screen.height) / 25;
        ObjectiveUIFont.normal.textColor = Color.white;
        ObjectiveUIFont.wordWrap = false;
        ObjectiveUIFont.fontStyle = FontStyle.Bold;
	}


	void OnGUI() {
		if(theObjective != "")
		{
			Vector2 size2 = ObjectiveUIFont.CalcSize(new GUIContent(theObjective));
			GUI.Label (new Rect (Screen.width - size2.x - margin,20,100,50), theObjective, ObjectiveUIFont);
		}
	}

	public void setTheObjective(string objective)
	{
		theObjective = objective;
	}
}
