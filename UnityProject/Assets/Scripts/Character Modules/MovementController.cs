using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour 
{
	private CharacterController charController;
	private CharacterMotor charMotor;

	private CharacterAnimation characterAnimator;

	private Transform cameraTransform;
	private Vector3 moveDirection = Vector3.zero;

	[HideInInspector]
    public Vector2 moveSpeed;

	public float movingSpeed = 1f;

	private Vector3 targetDirection = Vector3.zero;
	private Vector3 motorMovement = Vector3.zero;
	
	public float rotationSpeed = 150f;
	public float moveAccel = 2.5f;

	public bool canMove = true;

	[HideInInspector]
	public float v = 0;
	[HideInInspector]
	public float h = 0;

	private bool lookingAround = false;
	private CameraFollow camFollowComp = null;
	private CameraInput camInputComp = null;

	void Awake()
	{
        charController = gameObject.AddComponent<CharacterController>();
        charController.slopeLimit = 45f;
        charController.stepOffset = 0.3f;
        charController.skinWidth = 0.01f;
        charController.radius = 0.25f;
        charController.height = 1.76f;


        charMotor = gameObject.AddComponent<CharacterMotor>();
        charMotor.useFixedUpdate = false;
        charMotor.movement.maxBackwardsSpeed = 1f;
        charMotor.movement.maxForwardSpeed = 1f;
        charMotor.movement.maxSidewaysSpeed = 1f;
        charMotor.jumping.enabled = false;
        charMotor.movingPlatform.enabled = false;
        charMotor.sliding.enabled = false;

        camFollowComp = GetComponentInChildren<CameraFollow>();
		camInputComp = GetComponentInChildren<CameraInput>();
	}

    public void Initialize( Transform rigCamera, CharacterAnimation rigAnimator)
    {
        cameraTransform = rigCamera;
        characterAnimator = rigAnimator;
    }

	void Update()
	{
		Vector3 forward = cameraTransform.forward;

		forward.y = 0;
		forward = forward.normalized;

		if(canMove)
		{
			v = Input.GetAxisRaw("Vertical");
			h = Input.GetAxisRaw("Horizontal");
		}

		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		targetDirection = (h * right + v * forward).normalized;


		if( targetDirection.magnitude > 0f )
		{
			moveDirection = Vector3.RotateTowards(transform.forward, forward, rotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 300f);
            moveDirection.Normalize();

            float turning = Vector3.Dot(forward, transform.forward);

            characterAnimator.SetTurningSpeed(1f - turning);

			transform.rotation = Quaternion.LookRotation(moveDirection);
		}


		float curSmooth = moveAccel * Time.deltaTime;
		
        moveSpeed = Vector2.Lerp(moveSpeed, new Vector2(h,v) * movingSpeed, curSmooth);

        characterAnimator.SetForwardSpeed (moveSpeed.y);
        characterAnimator.SetSidewaysSpeed (moveSpeed.x);


		if( targetDirection.magnitude > 0f )
			motorMovement = targetDirection.normalized;


        charMotor.inputMoveDirection = motorMovement * moveSpeed.magnitude;


        // Stop movement if speed is low forward and sideways
        //if( moveSpeed.y < 0.175f ||  moveSpeed.x < 0.175f )
         //   charMotor.inputMoveDirection = Vector3.zero;

		// reseting values used by animator
		v = 0f; h = 0f;
	}

	public void EnableMoving( bool state )
	{
		canMove = state;

        moveSpeed = Vector2.zero;
        charMotor.inputMoveDirection = Vector3.zero;
	}

    public void SetWalkingSpeed(float forwardSpeed, float sidewaysSpeed)
    {
        charMotor.movement.maxForwardSpeed = forwardSpeed;
        charMotor.movement.maxSidewaysSpeed = sidewaysSpeed;
    }

    public bool isMoving()
	{
        return targetDirection != Vector3.zero;
    }
}
