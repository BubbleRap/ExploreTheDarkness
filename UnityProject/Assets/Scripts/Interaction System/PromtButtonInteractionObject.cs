﻿using UnityEngine;
using System.Collections;

public class PromtButtonInteractionObject : IInteractableObject
{
	public bool disableOnActive = true;
	public bool interactionWorksInFP = false;

	private GameObject buttonPrompt;
	private Interactor interactor;

	private bool objectIsClose = false;
	
	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;
	}
	
	void Start()
	{
		buttonPrompt = Instantiate(Resources.Load<GameObject>("buttonPrompt")) as GameObject;
		buttonPrompt.SetActive(false);

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		interactor = siljaGO.GetComponent<Interactor>();
	}

//	private void Update()
//	{
//		ActivatePromtButton((transform.position - interactor.transform.position).magnitude < 3f);
//		
//		Vector3 direction = ((transform.position - Vector3.up * 1.5f) - Camera.main.transform.position).normalized;
//		buttonPrompt.transform.position = transform.position - direction * 0.25f;
//		buttonPrompt.transform.LookAt(Camera.main.gameObject.transform);	
//	}

	private void Update()
	{
		bool isClose = (transform.position - interactor.transform.position).magnitude < 3f;
		bool isEligable = (interactionWorksInFP && SiljaBehaviour.darkMode) || !SiljaBehaviour.darkMode || interactionIsActive;
		OnInteractionClose(isClose && isEligable);

		Vector3 direction = ((transform.position - Vector3.up * 1.5f) - Camera.main.transform.position).normalized;
		buttonPrompt.transform.position = transform.position - direction * 0.25f;
		buttonPrompt.transform.LookAt(Camera.main.gameObject.transform);
	}

	public void ActivatePromtButton( bool state )
	{
		buttonPrompt.SetActive(state && (!interactionIsActive || !disableOnActive));

//		if( objectIsClose == state )
//			return;
//
//		if( state )
//			interactor.OnInteractionEnter( gameObject );
//		else
//			interactor.OnInteractionExit( gameObject );
//	
//		objectIsClose = state;
	}

	public void OnInteractionClose( bool state )
	{
		if( objectIsClose == state )
			return;
		
		if( state )
		{
			interactor.OnInteractionEnter( gameObject );
		}
		else
		{
			interactor.OnInteractionExit( gameObject );
		}
		
		objectIsClose = state;
	}

	void OnDestroy()
	{
		Destroy(buttonPrompt);
		buttonPrompt = null;
		Resources.UnloadUnusedAssets();
	}
}
