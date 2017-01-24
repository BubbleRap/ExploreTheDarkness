using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

	public GameObject interlude;
	public GameObject startButton;
	public GameObject Fade;

	// Use this for initialization
	void Start () {
		
	}

	public void beginInterlude()
	{
		interlude.SetActive(true);
		Fade.SetActive(true);
		startButton.SetActive(false);
	}
}
