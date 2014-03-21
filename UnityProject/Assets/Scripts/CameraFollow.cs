using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public Transform cameraFocusTarget = null; 
	public Vector3 cameraFocusOffset;

	//public float cameraHeight = 1;
	public float cameraDistance = 2;

	[Range(0f, 360f)]
	public float yaw = 0f;
	[Range(0f, 360f)]
	public float pitch = 0f;
	[Range(0f, 360f)]
	public float roll = 0f;

	public bool isFollowingPosition = true;
	public bool isFollowingRotation = true;

	[Range(0f,0.1f)]
	public float followingSpeed = 0.01f;
	[Range(0f,0.1f)]
	public float orientationSpeed = 0.01f;

	[Range(0f, 1f)]
	public float horizontalShakeIntensity = 1.0f;
	[Range(0f, 1f)]
	public float verticalShakeIntensity = 1.0f;
	[Range(0f, 10f)]
	public float shakeFrequency = 1f;


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
				Vector3 relativePosition = GetVectorFromAngle(pitch, yaw, roll, cameraDistance);
				transform.position = Vector3.Slerp(transform.position, cameraFocusTarget.TransformPoint(relativePosition + shakeOffset), followingSpeed);
			}
			yield return null;
		}
	}

	IEnumerator FollowRotation()
	{
		while (true) 
		{
			if( isFollowingRotation )
				transform.LookAt(cameraFocusTarget.position + cameraFocusOffset);
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

	Vector3 GetVectorFromAngle (float x, float y, float z, float distFromObj) 
	{
		//Quaternion rotation = Quaternion.Euler(x, y, z);

		Vector3 forward = Vector3.zero; //Quaternion.Euler(x, y, z) * Vector3.forward;
		Vector3 up = Quaternion.Euler(x, y, z) * Vector3.up;
		Vector3 right = Vector3.zero; //Quaternion.Euler(x, y, z) * Vector3.right;

		return (forward + up + right).normalized * distFromObj;
	}
}
