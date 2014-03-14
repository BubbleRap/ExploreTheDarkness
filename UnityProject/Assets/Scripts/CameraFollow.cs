using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public Transform cameraFocusTarget = null; 
	public Vector3 cameraFocusOffset;

	public float cameraHeight;
	public float cameraDistance;

	public bool isFollowingPosition = true;
	public bool isFollowingRotation = true;

	[Range(0f,0.1f)]
	public float followingSpeed = 0.1f;
	[Range(0f,0.1f)]
	public float orientationSpeed = 0.1f;

	void Start()
	{
		StartCoroutine (FollowPosition ());
		StartCoroutine (FollowRotation ()); 
	}

	IEnumerator FollowPosition()
	{
		while (true) 
		{
			if(isFollowingPosition) 
				transform.position = Vector3.Slerp(transform.position, cameraFocusTarget.TransformPoint(new Vector3(0f, cameraHeight, -cameraDistance) + cameraFocusOffset), followingSpeed);

			yield return null;
		}
	}

	IEnumerator FollowRotation()
	{
		while (true) 
		{
			if( isFollowingRotation )
				transform.rotation = Quaternion.Slerp( transform.rotation, cameraFocusTarget.rotation, orientationSpeed);

			yield return null;
		}
	}
}
