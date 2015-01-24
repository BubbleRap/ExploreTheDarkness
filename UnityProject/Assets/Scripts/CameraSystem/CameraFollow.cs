using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public Transform cameraFocusTarget = null; 
	public Vector3 cameraFocusOffset;
	
	public float cameraDistance = 2;

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

	
	private Vector3 shakeOffset;

	private void Start()
	{
		if (cameraFocusTarget == null) 
		{
			Debug.LogError ("You forgot to assign the object to follow!");
			return;
		}

		StartCoroutine (FollowPosition ());
		StartCoroutine (ShakeCamera	()); 

	}

	IEnumerator FollowPosition()
	{
		while (true) 
		{
			// offset the camera corresponding to the angles and the shaking offsets
			Vector3 relativePosition = GetVectorFromAngle(pitch - shakeOffset.y, yaw - shakeOffset.x, cameraDistance) + cameraFocusTarget.position;

			// Set up the camera on the orbit around the camera, using PITCH and YAW angles taken from CameraInput
			transform.position = Vector3.Slerp(transform.position, relativePosition, followingSpeed);
			// Look at the character with a bot of vertical offset, so it looks at the head
			transform.localRotation = Quaternion.LookRotation((cameraFocusOffset -transform.localPosition).normalized);

			// Offset the rotation a bit, so the character is positioned a bit on the left
			transform.Rotate( Vector3.up, 15f, Space.World );

			yield return null;
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
}
