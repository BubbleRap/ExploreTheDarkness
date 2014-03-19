using UnityEngine;
using System.Collections;

public class Triger : MonoBehaviour {

	public Transitioner transitionContoller;
	public bool DarkMode;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			transitionContoller.doTransition(DarkMode);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
