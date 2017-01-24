using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void openURL(string URL)
	{
		Application.OpenURL(URL);
	}
}
