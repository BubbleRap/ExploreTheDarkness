﻿using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour 
{
	private CharacterController charController = null;
	private Animator animationController = null;
	public Transform cameraTransform = null;
	private Vector3 moveDirection = Vector3.zero;
	
	private float moveSpeed = 0f;
	public float movingSpeed = 10f;

	private Vector3 targetDirection = Vector3.zero;

	public bool alwaysFollows = false;
	public float rotationSpeed = 150f;
	public float moveAccel = 1f;

	void Awake()
	{
		charController = GetComponent<CharacterController> ();
		animationController = GetComponentInChildren<Animator> ();
		animationController.SetBool ("scared", true);
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

		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");

		bool isMoving = Mathf.Abs (h) > 0.1 || Mathf.Abs (v) > 0.1;

		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		targetDirection = h * right + v * forward;
	

		if( alwaysFollows )
		{
			moveDirection = forward;
			transform.rotation = Quaternion.LookRotation(moveDirection);
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

		animationController.SetFloat ("speed", moveSpeed);

		Vector3 movement = moveDirection * moveSpeed;
		movement *= Time.deltaTime;

		charController.Move(movement);
	}

	IEnumerator LookAround()
	{
		while (true) 
		{
			// gasp sound to add here

			yield return new WaitForSeconds( Random.Range(5f, 15f));
			bool rightOrLeft = (Random.Range(0, 10) % 2) == 0 ;
			string lookEvent = rightOrLeft ? "lookLeftEvent" : "lookRightEvent";
			animationController.SetTrigger(lookEvent);
		}
	}
}
