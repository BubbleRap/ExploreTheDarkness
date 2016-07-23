using UnityEngine;
using System.Collections;

public class CameraWhisker
{
    public bool hasHit; 
    public Vector3 direction;
    public float distance;
    public Vector3 hitPoint;
    public Vector3 hitNormal;
}

public class CameraFollow : MonoBehaviour 
{
	public Transform cameraFocusTarget = null; 
	
	[HideInInspector]
	public float cameraDistance = 2;
	public float minDistance = 0.01f;
	public float maxDistance = 1.15f;
	
	[Range(0f, 1f)]
	public float yaw = 0f;
	[Range(0f, 1f)]
	public float pitch = 0f;
	
	[Range(0f, 1f)]
	public float horizontalShakeIntensity = 0.0f;
	[Range(0f, 1f)]
	public float verticalShakeIntensity = 0.0f;
	[Range(0f, 10f)]
	public float shakeFrequency = 0f;
	
	public float collisionFixMinHeight = 0.45f;
	public float collisionFixMaxHeight = 0.7f;
	private Vector3 shakeOffset;
	
	[Range(0f, 2f)]
	public float focusDistance = 0.5f;
	[Range(0f, 90f)]
	public float focusAngleOffset = 30f;
	
	
	// FPS settings
	public float minimumY = -60F;
	public float maximumY = 60F;
	private float rotationY = 0F;

    // collisions avoidance
    private const int WHISKERS_COUNT = 16;
    private float m_swingSensitivity = 0.1f;

    private CameraWhisker lineOfSight = new CameraWhisker();
    private CameraWhisker backWhisker = new CameraWhisker();
    private CameraWhisker[] whiskers = new CameraWhisker[WHISKERS_COUNT];

    void Awake()
    {
        for(int i = 0; i < WHISKERS_COUNT; i++)
            whiskers[i] = new CameraWhisker();
    }

	public enum CameraControlType
	{
		CCT_Default,
		CCT_FPSLook,
		CCT_LookingAtObject,
		CCT_Overwritten
	}
	private CameraControlType m_camControlType = CameraControlType.CCT_Default;
	public CameraControlType CamControlType
	{
		set{ m_camControlType = value; }
		get{ return m_camControlType; }
	}
	
	[HideInInspector]
	public Vector3 focusPoint;

    public void UpdateCameraControls(float horizDelta, float vertDelta)
    {
        pitch = Mathf.Clamp (pitch - vertDelta, 0.5f, 0.7f);    
        yaw = Mathf.Repeat(yaw + horizDelta, 1f);

		switch( m_camControlType )
		{
		case CameraControlType.CCT_Default:

			UpdateTP();
            break;
			
		case CameraControlType.CCT_FPSLook:
            
            UpdateFP(horizDelta, vertDelta);
			break;
			
		case CameraControlType.CCT_LookingAtObject:
			
			transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( focusPoint - transform.position ), Time.deltaTime );           
			break;
			
		case CameraControlType.CCT_Overwritten:
			break;
		}
    }

	private void UpdateTP()
	{
		float distanceFactor = Mathf.Clamp01((cameraDistance - minDistance) / (maxDistance - minDistance));
		float collisionFixHeight = Mathf.Lerp(collisionFixMinHeight, collisionFixMaxHeight, 1f - distanceFactor );
		
		// offset the camera corresponding to the angles and the shaking offsets
        Quaternion lookTransform = Quaternion.Euler(new Vector2(pitch * 360f + shakeOffset.x, yaw * 360f + shakeOffset.y));
        Vector3 lookDirection = lookTransform * Vector3.forward;

        transform.position = cameraFocusTarget.position + lookDirection * cameraDistance + Vector3.up * collisionFixHeight;
       			
		Vector3 camLocalDirection = new Vector3(-transform.localPosition.x, 0f, -transform.localPosition.z).normalized;
        camLocalDirection = (Quaternion.Euler(0f, focusAngleOffset, 0f) * camLocalDirection) * focusDistance * 0f;
		
        transform.rotation = Quaternion.LookRotation(-lookDirection + Vector3.up * collisionFixHeight + camLocalDirection);		
	}
	
    private void UpdateFP(float mouseX, float mouseY)
	{
        float rotationX = transform.localEulerAngles.y + mouseX;
		
        rotationY += mouseY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		
		transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
		transform.parent.Rotate(0,rotationX, 0);
	}

    private IEnumerator ShakeCamera()
    {
        while (true) 
        {
            shakeOffset = new Vector3( Random.Range(-horizontalShakeIntensity * 10, horizontalShakeIntensity * 10), 
                Random.Range(-verticalShakeIntensity * 10, verticalShakeIntensity * 10), 0f);
            yield return new WaitForSeconds(shakeFrequency);
        }
    }

    // control the distance to the character
    public void CameraDistanceControl()
    {
        CheckCollisionsFor(cameraFocusTarget.position, transform.position, ref lineOfSight);
        CheckCollisionsFor(transform.position, transform.position - transform.forward, ref backWhisker);

        // default case: no collisions, lerp to the maximum distance
        float distanceTo = maxDistance;

        if(lineOfSight != null && lineOfSight.hasHit)
        {
            distanceTo = Mathf.Clamp(lineOfSight.distance, minDistance, maxDistance);
        
            // sight line hit: jump straight to the hit point
            cameraDistance = distanceTo;
        }
        else 
        if(backWhisker != null && backWhisker.hasHit)
        {
            // back line hit: lerp to the hit point
            distanceTo = Mathf.Clamp(backWhisker.distance, minDistance, maxDistance);
        }

        cameraDistance = Mathf.MoveTowards(cameraDistance, distanceTo, Time.deltaTime);
    }

    // control the obstacles avoidance by directing the camera between 
    // left and right whiskers
    public void CameraSwingControl()
    {
        CameraWhisker left = FindClosestLeftObstacle();
        CameraWhisker right = FindClosestRightObstacle();

        // swinging to the right when obstacle from the left
        if(left != null && left.hasHit)
        {
            Vector3 localDirection = transform.InverseTransformDirection(left.direction);
            // -90 degrees = -1f, 90 degrees = 1f
            float angle = Vector3.Cross(Vector3.forward, localDirection).y;
            // (distance == max distance) = 0f, (distance == 0f) = 1f
            float distance = 1f - left.distance / maxDistance;
        
            yaw += angle * distance * m_swingSensitivity * Time.deltaTime;;
        }
        
        // swinging to the left when obstacle from the right
        if(right != null && right.hasHit)
        {
            Vector3 localDirection = transform.InverseTransformDirection(right.direction);
            // -90 degrees = -1f, 90 degrees = 1f
            float angle = Vector3.Cross(Vector3.forward, localDirection).y;
            // (distance == max distance) = 0f, (distance == 0f) = 1f
            float distance = 1f - right.distance / maxDistance;
        
            yaw += angle * distance * m_swingSensitivity * Time.deltaTime;
        }

        // swing the camera along the wall's normal to avoid walls hack
        if(lineOfSight.hasHit)
        {
            Vector3 localDirection = transform.InverseTransformDirection(lineOfSight.hitNormal);
            float angle = Vector3.Cross(Vector3.forward, localDirection).y;
        
            // ranges [0.75f..1.0f] and [-1.0..-0.75] are critical:
            // when the camera is colliding with the wall by 45 to 90 degrees
            yaw -= angle * 0.0025f;
        }
    }

    private CameraWhisker FindClosestLeftObstacle()
    {
        float minDistance = float.MaxValue;

        // default 45 deg whisker
        CameraWhisker closestObstacleWhisker = null;

        for( int i = 0; i < (int) (WHISKERS_COUNT * 0.5f); i++ )
        {
            Vector3 direction = Vector3.Lerp(-transform.right, transform.forward, i / (WHISKERS_COUNT * 0.5f));
            CheckCollisionsFor(transform.position, transform.position + direction, ref whiskers[i]);
            if(whiskers[i].hasHit)
            {
                if(whiskers[i].distance < minDistance)
                {
                    closestObstacleWhisker = whiskers[i];
                    minDistance = whiskers[i].distance;
                }
            }
        }

        return closestObstacleWhisker;
    }

    private CameraWhisker FindClosestRightObstacle()
    {
        float minDistance = float.MaxValue;

        // default 45 deg whisker
        CameraWhisker closestObstacleWhisker = null;

        for( int i = (int) (WHISKERS_COUNT * 0.5f); i < WHISKERS_COUNT; i++ )
        {
            Vector3 direction = Vector3.Lerp(transform.right, transform.forward, (WHISKERS_COUNT - i) /  (WHISKERS_COUNT - WHISKERS_COUNT * 0.5f));
            CheckCollisionsFor(transform.position, transform.position + direction, ref whiskers[i]);
            if(whiskers[i].hasHit)
            {
                if(whiskers[i].distance < minDistance)
                {
                    closestObstacleWhisker = whiskers[i];
                    minDistance = whiskers[i].distance;
                }
            }
        }

        return closestObstacleWhisker;
    }

    private void CheckCollisionsFor(Vector3 fromPoint, Vector3 toPoint, ref CameraWhisker result)
    {
        RaycastHit outHit;

        result.distance = (toPoint - fromPoint).magnitude;
        result.hitPoint = toPoint;
        result.hitNormal = Vector3.zero;

        Vector3 direction = (toPoint - fromPoint).normalized;
        bool hasHit = 
            Physics.Raycast(
                fromPoint,
                direction,
                out outHit,
                maxDistance,
                ((1 << LayerMask.NameToLayer("Default")))
            );

        Debug.DrawLine(fromPoint, fromPoint + direction * result.distance, hasHit ? Color.red : Color.green);

        result.hasHit = hasHit;
        result.direction = direction;

        if(hasHit)
        {
            result.distance = outHit.distance;
            result.hitPoint = outHit.point;
            result.hitNormal = outHit.normal;
        }
    }
}