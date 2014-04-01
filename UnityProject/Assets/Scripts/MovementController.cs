using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour 
{
	private CharacterController charController = null;
	private Animator animationController = null;
	public Transform cameraTransform = null;
	private Vector3 moveDirection = Vector3.zero;

	public float rotateSpeed = 500.0f;
	private float moveSpeed = 2f;
	public float movingSpeed = 10f;

	void Awake()
	{
		charController = GetComponent<CharacterController> ();
		animationController = GetComponentInChildren<Animator> ();
		animationController.SetBool ("scared", true);
		cameraTransform = Camera.main.transform;

		moveDirection = transform.TransformDirection(Vector3.forward);
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
		Vector3 targetDirection = h * right + v * forward;
	


		moveDirection = forward;

		if (targetDirection != Vector3.zero)
		{
			// If we are really slow, just snap to the target direction
			//if (moveSpeed < walkSpeed * 0.9 && grounded)
			//{
			//	moveDirection = targetDirection.normalized;
			//}
			//// Otherwise smoothly turn towards it
			//else
			//{
				transform.rotation = Quaternion.LookRotation(moveDirection);
				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000f);
				moveDirection = moveDirection.normalized;
			//}
		}






			
		// Smooth the speed based on the current target direction
		var curSmooth = 10f * Time.deltaTime;
		
		// Choose target speed
		//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
		float targetSpeed = Mathf.Min(targetDirection.magnitude, movingSpeed);
		
		moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

		animationController.SetFloat ("speed", moveSpeed);

		moveDirection = transform.forward;
		Vector3 movement = moveDirection * moveSpeed /*+ Vector3 (0, verticalSpeed, 0) + inAirVelocity*/;
		movement *= Time.deltaTime;

		charController.Move(movement);
	}
}
