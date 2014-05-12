﻿using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour 
{
	private CharacterController charController = null;
	private CharacterMotor charMotor = null;

	public Animator characterAnimator;
	public Animator firstPersonAnimator;

	private Transform cameraTransform = null;
	private Vector3 moveDirection = Vector3.zero;
	
	private float moveSpeed = 0f;
	public float movingSpeed = 10f;

	private Vector3 targetDirection = Vector3.zero;

	public bool alwaysFollows = false;
	public float rotationSpeed = 150f;
	public float moveAccel = 1f;

	public bool canMove = true;

	[HideInInspector]
	public float v = 0;
	[HideInInspector]
	public float h = 0;

	void Awake()
	{
		charController = GetComponent<CharacterController> ();
		charMotor = GetComponent<CharacterMotor>();

		characterAnimator.SetBool ("scared", true);
		cameraTransform = Camera.main.transform;
	}

	void Start()
	{
		StartCoroutine(LookAround());
	}

	void Update()
	{
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);

		forward.y = 0;
		forward = forward.normalized;

		if(canMove)
		{
			v = Input.GetAxisRaw("Vertical");
			h = Input.GetAxisRaw("Horizontal");
		}

		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		targetDirection = h * right + v * forward;

		if( alwaysFollows )
		{
			moveDirection = targetDirection;
			transform.rotation = Quaternion.LookRotation(forward);
		}
		else
			if( targetDirection.magnitude > 0f )
			{
				moveDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 300f);
				moveDirection = moveDirection.normalized;

				transform.rotation = Quaternion.LookRotation(moveDirection);
			}

		float curSmooth = moveAccel * Time.deltaTime;

		float targetSpeed = targetDirection.magnitude * movingSpeed;
		
		moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

		characterAnimator.SetFloat ("speed", moveSpeed);

		if( firstPersonAnimator.gameObject.activeInHierarchy )
			firstPersonAnimator.SetFloat ("speed", moveSpeed);

		Vector3 movement = moveDirection * moveSpeed;
		if( moveSpeed < 0.1f )
			movement = Vector3.zero;

		charMotor.inputMoveDirection = movement;

		// reseting values used by animator
		v = 0f; h = 0f;
	}

	IEnumerator LookAround()
	{
		while (true) 
		{
			// gasp sound to add here

			yield return new WaitForSeconds( Random.Range(5f, 15f));
			bool rightOrLeft = (Random.Range(0, 10) % 2) == 0 ;
			string lookEvent = rightOrLeft ? "lookLeftEvent" : "lookRightEvent";
			characterAnimator.SetTrigger(lookEvent);
		}
	}

	// called by animation event
	public void EnableMoving()
	{
		canMove = true;
	}

	// called by Trigger.cs by sending message
	public void StartMonsterHandCutScene()
	{
		characterAnimator.SetTrigger("handCutSceneStart");
	}

	// called from Monster Hand class by sending message
	public void MonsterHandHitEvent()
	{
		characterAnimator.SetTrigger("handCutSceneHit");
	}
}
