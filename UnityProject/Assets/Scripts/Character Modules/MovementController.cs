using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour 
{
    public Vector2 MoveSpeed { get; private set; }
    public bool IsMoving { get; private set; }
    public bool CanMove { get; private set; }


    private CharacterController m_charController;
    private CharacterMotor m_charMotor;

    private float m_movingSpeed = 1f;
    private Vector3 m_motorMovement = Vector3.zero;
	
    private float m_rotationSpeed = 5f;
    private float m_moveAccel = 5f;


	void Awake()
	{
        m_charController = gameObject.AddComponent<CharacterController>();
        m_charMotor = gameObject.AddComponent<CharacterMotor>();

        IsMoving = false;
        CanMove = true;
	} 

    public void InitializeCharacterController(float slopeLimit,
                                              float stepOffset,
                                              float skinWidth,
                                              float radius,
                                              float height)
    {
        m_charController.slopeLimit = slopeLimit;
        m_charController.stepOffset = stepOffset;
        m_charController.skinWidth = skinWidth;
        m_charController.radius = radius;
        m_charController.height = height;
    }

    public void InitializeCharacterMotor(bool useFixedUpdate,
                                         float maxBackwardsSpeed,
                                         float maxForwardSpeed,
                                         float maxSidewaysSpeed,
                                         bool jumpingEnabled,
                                         bool movingPlatformEnabled,
                                         bool slidingEnabled)
    {
        m_charMotor.useFixedUpdate = useFixedUpdate;
        m_charMotor.movement.maxBackwardsSpeed = maxBackwardsSpeed;
        m_charMotor.movement.maxForwardSpeed = maxForwardSpeed;
        m_charMotor.movement.maxSidewaysSpeed = maxSidewaysSpeed;
        m_charMotor.jumping.enabled = jumpingEnabled;
        m_charMotor.movingPlatform.enabled = movingPlatformEnabled;
        m_charMotor.sliding.enabled = slidingEnabled;
    }

    public void EnableMoving( bool state )
    {
        CanMove = state;

        MoveSpeed = Vector2.zero;
        m_charMotor.inputMoveDirection = Vector3.zero;
    }

    public void MoveTowardsDirection(Vector3 targetDirection, Vector2 forwardSideDirection)
    {
        if( !CanMove )
            return;
        
        IsMoving = (targetDirection != Vector3.zero);
        MoveSpeed = Vector2.Lerp(
            MoveSpeed, 
            forwardSideDirection * m_movingSpeed, 
            m_moveAccel * Time.deltaTime
        );
            
        m_motorMovement = targetDirection.magnitude > 0f ? targetDirection.normalized : transform.forward;       
        m_charMotor.inputMoveDirection = m_motorMovement * MoveSpeed.magnitude;
    }

    public void RotateTowardsDirection(Vector3 desiredDirection)
    {
        if( !CanMove )
            return;

        Vector3 moveDirection = Vector3.RotateTowards( 
            transform.forward, 
            desiredDirection, 
            m_rotationSpeed * Time.deltaTime,
            0f);

        moveDirection.y = 0f;
        moveDirection.Normalize();

        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    public void SetMaxWalkingSpeed(float forwardSpeed, float sidewaysSpeed)
    {
        m_charMotor.movement.maxForwardSpeed = forwardSpeed;
        m_charMotor.movement.maxSidewaysSpeed = sidewaysSpeed;
    }
}
