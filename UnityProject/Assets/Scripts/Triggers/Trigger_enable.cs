﻿using UnityEngine;
using System.Collections;

public class Trigger_enable : MonoBehaviour {

	public GameObject theGameObject;
	public bool setEnable = false;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Player")
		{
			theGameObject.SetActive(setEnable);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void enableThisObject()
	{
		transform.gameObject.SetActive(false);
	}
}
