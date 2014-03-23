using UnityEngine;
using System.Collections;

public enum CameraMode
{
	Mouse_Free,
	WASD_Ctrl
}

public class CameraFollow : MonoBehaviour 
{
	public CameraMode cameraMode = CameraMode.WASD_Ctrl;

	public Transform cameraFocusTarget = null; 
	public Vector3 cameraFocusOffset;
	
	public float cameraDistance = 2;

	[Range(0f, 360f)]
	public float yaw = 180f;
	[Range(0f, 360f)]
	public float pitch = 45f;
	[Range(0f, 360f)]
	public float roll = 0f;

	public bool isFollowingPosition = true;
	public bool isFollowingRotation = true;

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


	// private sector, dude.
	// Authorized personel only!
	private Vector3 shakeOffset;

	private void Start()
	{
		if (cameraFocusTarget == null) 
		{
			Debug.LogError ("You forgot to assign the object to follow!");
			return;
		}

		StartCoroutine (FollowPosition ());
		StartCoroutine (FollowRotation ()); 
		StartCoroutine (ShakeCamera	()); 

	}

	IEnumerator FollowPosition()
	{
		while (true) 
		{
			if(isFollowingPosition) 
			{
				Vector3 relativePosition;
				if( cameraMode == CameraMode.WASD_Ctrl )
				{
					relativePosition = GetVectorFromAngle(pitch, yaw, roll, cameraDistance);
					transform.position = Vector3.Lerp(transform.position, cameraFocusTarget.TransformPoint(/*shakeOffset +*/ relativePosition), followingSpeed);
				}
				else
				{
					relativePosition = GetVectorFromAngle(pitch, yaw, roll, cameraDistance, cameraFocusTarget.position);
					transform.position = Vector3.Lerp(transform.position, /*cameraFocusTarget.TransformPoint(shakeOffset) + */relativePosition, followingSpeed);
				}
			}
			yield return null;
		}
	}

	IEnumerator FollowRotation()
	{
		while (true) 
		{
			if( isFollowingRotation )
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(((cameraFocusTarget.position + cameraFocusOffset ) - transform.position).normalized), 0.5f);
			}
			yield return null;
		}
	}

	IEnumerator ShakeCamera()
	{
		while (true) 
		{
			shakeOffset = new Vector3( Random.Range(-horizontalShakeIntensity, horizontalShakeIntensity), 
					                          Random.Range(-verticalShakeIntensity, verticalShakeIntensity), 0f);
			yield return new WaitForSeconds(shakeFrequency);
		}
	}

	Vector3 GetVectorFromAngle (float x, float y, float z, float distFromObj, Vector3 relativePosition = default(Vector3)) 
	{
		Vector3 forward = Vector3.zero; //Quaternion.Euler(x, y, z) * Vector3.forward;
		Vector3 up = Quaternion.Euler(x, y, z) * Vector3.up;
		Vector3 right = Vector3.zero; //Quaternion.Euler(x, y, z) * Vector3.right;

		return (forward + up + right).normalized * distFromObj + relativePosition;
	}
}
