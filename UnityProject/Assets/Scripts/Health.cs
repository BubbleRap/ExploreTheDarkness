using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int health = 8;
	public Transitioner transitionController;

	// Use this for initialization
	void Start () {
	
	}

	void OnGUI () {
		// Make a background box
		if(transitionController.darkMode)
		{
			GUI.skin.label.fontSize = 100;
			GUI.Label(new Rect (40,20,100,300), health.ToString());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void looseLife() {
		health --;
	}
}
