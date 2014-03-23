using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))] 
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CameraFollow))]
public class CameraPhysics : MonoBehaviour 
{
	// Percents
	[Range(0f, 1f)]
	public float minimumDistance = 0.5f;
	[Range(0f, 1f)]
	public float approachingSpeed = 0.1f;

	private CameraFollow follower = null;
	private float originalDistance = 0f;
	private List<Collider> colliders = new List<Collider>();

	void Awake()
	{
		follower = GetComponent<CameraFollow> ();
		originalDistance = follower.cameraDistance;
	}

	void Update()
	{
		if( colliders.Count > 0 )
			follower.cameraDistance = Mathf.Lerp (follower.cameraDistance, originalDistance * minimumDistance, approachingSpeed);
		else
			follower.cameraDistance = Mathf.Lerp (follower.cameraDistance, originalDistance, approachingSpeed);
	}

	void OnTriggerEnter(Collider other)
	{
		colliders.Add (other);
	}
	
	void OnTriggerExit(Collider other)
	{
		colliders.Remove (other);
	}
}
