using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour 
{
	private CharacterController charController = null;
	private Animator animationController = null;
	public Transform cameraTransform = null;
	private Vector3 moveDirection = Vector3.zero;
	
	private float moveSpeed = 2f;
	public float movingSpeed = 10f;

	private Vector3 targetDirection = Vector3.zero;

	public bool alwaysFollows = false;
	[Range(0f, 1f)]
	public float rotationSpeed = 0.1f;

	void Awake()
	{
		charController = GetComponent<CharacterController> ();
		animationController = GetComponentInChildren<Animator> ();
		animationController.SetBool ("scared", true);
		cameraTransform = Camera.main.transform;


	}

	void Start()
	{
		//moveDirection = transform.TransformDirection(Vector3.forward);
		//StartCoroutine(FollowRotation());
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
	
		moveDirection = forward;

		if( alwaysFollows )
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed);
		else
			if( targetDirection.magnitude > 0f )
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed);

		float curSmooth = 10f * Time.deltaTime;

		float targetSpeed = Mathf.Min(targetDirection.magnitude, movingSpeed);
		
		moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		animationController.SetFloat ("speed", moveSpeed);

		Vector3 movement = moveDirection * moveSpeed;
		movement *= Time.deltaTime;
		charController.Move(movement);
	}

	//IEnumerator FollowRotation()
	//{
	//	while (true) 
	//	{
	//		if( alwaysFollows )
	//			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed);
	//		else
	//			if( targetDirection.magnitude > 0f )
	//				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed);
	//
	//		yield return null;
	//	}
	//}
}
