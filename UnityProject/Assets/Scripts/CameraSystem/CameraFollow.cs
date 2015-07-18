using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public Transform cameraFocusTarget = null; 
	
	[HideInInspector]
	public float cameraDistance = 2;
	public float minDistance = 0.01f;
	public float maxDistance = 1.15f;
	
	[Range(0f, 360f)]
	public float yaw = 180f;
	[Range(0f, 360f)]
	public float pitch = 45f;
	
	[Range(0f,1.0f)]
	public float followingSpeed = 0.05f;
	[Range(0f,1.0f)]
	public float orientationSpeed = 0.5f;
	
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
	[Range(0f, 45f)]
	public float focusAngleOffset = 30f;
	
	
	// FPS settings
	public float minimumY = -60F;
	public float maximumY = 60F;
	private float rotationY = 0F;
	
	// General settings
	[Range(0f, 10f)]
	public float verticalSensetivity = 1f;
	[Range(0f, 10f)]
	public float horizontalSensetivity = 1f;
	
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

	void LateUpdate()
	{
		if( GetComponent<CameraTransitioner>().Mode == CameraTransitioner.CameraMode.Transitioning )
			return;
		
		switch( m_camControlType )
		{
		case CameraControlType.CCT_Default:

			UpdateTPPAngles ();
			TPCameraBehaviour();
			
			break;
			
		case CameraControlType.CCT_FPSLook:

			UpdateTPPAngles ();
			FPCameraBehaviour();
			
			break;
			
		case CameraControlType.CCT_LookingAtObject:
			
			transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( focusPoint - transform.position ), Time.deltaTime );
			break;
			
		case CameraControlType.CCT_Overwritten:
			break;
		}
	}
	
	IEnumerator ShakeCamera()
	{
		while (true) 
		{
			shakeOffset = new Vector3( Random.Range(-horizontalShakeIntensity * 10, horizontalShakeIntensity * 10), 
			                          Random.Range(-verticalShakeIntensity * 10, verticalShakeIntensity * 10), 0f);
			yield return new WaitForSeconds(shakeFrequency);
		}
	}
	
	Vector3 GetVectorFromAngle (float x, float y, float distFromObj, Vector3 relativePosition = default(Vector3)) 
	{
		// Calculating the camera's position on the orbit
		Vector3 up = (Quaternion.Euler(x, y, 0f) * Vector3.up).normalized;
		return up * distFromObj;
	}
	
	private void TPCameraBehaviour()
	{
		float distanceFactor = Mathf.Clamp01((cameraDistance - minDistance) / (maxDistance - minDistance));
		float collisionFixHeight = Mathf.Lerp(collisionFixMinHeight, collisionFixMaxHeight, 1f - distanceFactor );
		
		// offset the camera corresponding to the angles and the shaking offsets
		Vector3 relativePosition = GetVectorFromAngle(pitch - shakeOffset.y, yaw - shakeOffset.x, cameraDistance) + cameraFocusTarget.position + Vector3.up * collisionFixHeight;
		
		// Set up the camera on the orbit around the camera, using PITCH and YAW angles taken from CameraInput
		transform.position = Vector3.Slerp(transform.position, relativePosition, followingSpeed);
		
		
		Vector3 camLocalDirection = new Vector3(-transform.localPosition.x, 0f, -transform.localPosition.z).normalized;
		camLocalDirection = (Quaternion.Euler(0f, focusAngleOffset, 0f) * camLocalDirection) * focusDistance;
		
		transform.localRotation = Quaternion.LookRotation( ((-transform.localPosition + Vector3.up * collisionFixHeight) + camLocalDirection).normalized );
		
	}
	
	private void FPCameraBehaviour()
	{
		Vector3 deltaMousePosition = new Vector3 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"), 0f);
		
		float rotationX = transform.localEulerAngles.y + deltaMousePosition.x * horizontalSensetivity;
		
		rotationY += deltaMousePosition.y * verticalSensetivity;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		
		transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
		transform.parent.Rotate(0,rotationX, 0);
	}

	void UpdateTPPAngles ()
	{
		Vector3 deltaMousePosition = new Vector3 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"), 0f);
		float pitchAngle = pitch + deltaMousePosition.y * verticalSensetivity ;
		pitchAngle = Mathf.Clamp (pitchAngle, 30f, 110f);	
		yaw = Mathf.Repeat(yaw + deltaMousePosition.x * horizontalSensetivity, 359.999f);
		pitch = pitchAngle;
	}
}