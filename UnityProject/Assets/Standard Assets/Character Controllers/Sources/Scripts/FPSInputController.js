private var motor : CharacterMotor;
private var animator : Animator;
public var firstpersonCam : Transform;
private var timer = 0.0;
private var bobbingSpeed  = 0.18f; 
private var shimmycam : float = 5.0f;

// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
	animator = GetComponentInChildren(Animator);
}

// Update is called once per frame
function Update () {
	// Get the input vector from keyboard or analog stick
	var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	
	animator.speed = 1;
	if (directionVector != Vector3.zero) {
		// Get the length of the directon vector and then normalize it
		// Dividing by the length is cheaper than normalizing when we already have the length anyway
		//Debug.Log(directionVector.magnitude);
		var directionLength = directionVector.magnitude;
		directionVector = directionVector / directionLength;
		
		// Make sure the length is no bigger than 1
		directionLength = Mathf.Min(1, directionLength);
		
		// Make the input vector more sensitive towards the extremes and less sensitive in the middle
		// This makes it easier to control slow speeds when using analog sticks
		directionLength = directionLength * directionLength;
		
		// Multiply the normalized direction vector by the modified length
		directionVector = directionVector * directionLength;

		if(directionVector.magnitude >= 1)
		{
			animator.speed = directionVector.magnitude * 1.7f;
		}

		waveslice = Mathf.Sin(Time.time * 9.7f * directionVector.magnitude)*0.004f;

	   	firstpersonCam.localPosition.y += waveslice;
	}
	else
	{
		if(firstpersonCam.localEulerAngles.z != 0)
		{
			firstpersonCam.localEulerAngles = Vector3(firstpersonCam.localEulerAngles.x, firstpersonCam.localEulerAngles.y, 0);
		}
	}
	
	// Apply the direction to the CharacterMotor
	motor.inputMoveDirection = transform.rotation * directionVector;
	animator.SetFloat ("speed", directionVector.magnitude);
	//motor.inputJump = Input.GetButton("Jump");
}

// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")
