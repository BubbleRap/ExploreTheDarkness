using UnityEngine;
using System.Collections;

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
        Vector3 lookDirection = lookTransform * Vector3.forward * cameraDistance;
        transform.position = cameraFocusTarget.position + lookDirection + Vector3.up * collisionFixHeight;
       			
		Vector3 camLocalDirection = new Vector3(-transform.localPosition.x, 0f, -transform.localPosition.z).normalized;
        camLocalDirection = (Quaternion.Euler(0f, focusAngleOffset, 0f) * camLocalDirection) * focusDistance * 0f;
		
		transform.localRotation = Quaternion.LookRotation( ((-transform.localPosition + Vector3.up * collisionFixHeight) + camLocalDirection).normalized );		
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
}