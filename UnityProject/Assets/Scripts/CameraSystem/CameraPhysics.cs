using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))] 
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CameraFollow))]
public class CameraPhysics : MonoBehaviour 
{
	// Percents
	[Range(0f, 1f)]
	public float approachingSpeed = 0.1f;

	public AnimationCurve fallbackCurve;
	[Range(0f, 5f)]
	public float fallbackTime = 5f;

	private CameraFollow follower = null;

	private List<Collider> colliders = new List<Collider>();

	private float _timer = 5f;
	private float fallbackFrom = 0f;

	void Awake()
	{
		follower = GetComponent<CameraFollow> ();
	}

	void Update()
	{
		_timer += Time.deltaTime;
		_timer = Mathf.Clamp(_timer, 0f, fallbackTime);

		RaycastHit hit = default(RaycastHit);

		// if camera collides itself
		if( colliders.Count > 0 
		   // if camera center point raycast hits a wall
	//	   ||  	Physics.Raycast(transform.position, transform.forward, out hit, transform.localPosition.magnitude, 1 << 0 ) 
		   // if raycast to character hits a wall
		   ||	Physics.Raycast(transform.position, (follower.cameraFocusTarget.position - transform.position).normalized, out hit, follower.cameraDistance, 1 << 0) 
		   )
		{
			// if there is a hit, then get a close up
			follower.cameraDistance = Mathf.Lerp (follower.cameraDistance, follower.maxDistance * follower.minDistance, approachingSpeed);


			_timer = 0f;
			fallbackFrom = follower.cameraDistance;
		}
		else
		{
			if( _timer < fallbackTime )
			{
				// fallback the camera if there is no collision happening
				follower.cameraDistance = Mathf.Lerp (fallbackFrom, follower.maxDistance, fallbackCurve.Evaluate(_timer / fallbackTime ) );
			}
		}

	}

	void OnTriggerEnter(Collider other)
	{
	//	Debug.Log(other.name);

		colliders.Add (other);
	}
	
	void OnTriggerExit(Collider other)
	{
		colliders.Remove (other);
	}
}
