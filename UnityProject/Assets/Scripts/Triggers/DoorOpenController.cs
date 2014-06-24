﻿using UnityEngine;
using System.Collections;

public class DoorOpenController : MonoBehaviour {

	bool doorIsOpened = false;
	Transform myTransform;
	public float yawRot;
	float openSpeed = 2.5f;
	// Use this for initialization
	void Start () 
	{
		myTransform = this.gameObject.transform;
		yawRot = myTransform.transform.localRotation.eulerAngles.z;
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Player" && !doorIsOpened)
		{
			StartCoroutine("OpenDatDoor");
		}

	}
	IEnumerator OpenDatDoor() 
	{
		doorIsOpened = true;

		float t = 0.0f;
		float openingSpeed = 0.25f;
		while(t < 1)
		{
			t += (openingSpeed*Time.deltaTime);
			yawRot = Mathf.LerpAngle(yawRot, 245f , t);
			myTransform.localEulerAngles = new Vector3(0,0,yawRot);
			yield return null;
		}
	}
}
