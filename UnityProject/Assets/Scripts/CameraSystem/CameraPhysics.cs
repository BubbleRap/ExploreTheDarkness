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

	public AnimationCurve fallbackCurve;
	[Range(0f, 5f)]
	public float fallbackTime = 5f;

	private CameraFollow follower = null;
	private float originalDistance = 0f;
	private List<Collider> colliders = new List<Collider>();

	private float _timer = 0f;
	private float fallbackFrom = 0f;

	void Awake()
	{
		follower = GetComponent<CameraFollow> ();
		originalDistance = follower.cameraDistance;
	}

	void Update()
	{
		_timer += Time.deltaTime;
		_timer = Mathf.Clamp(_timer, 0f, fallbackTime);

//		RaycastHit hit = default(RaycastHit);

		// if camera collides itself
		if( colliders.Count > 0 
		   // if camera center point raycast hits a wall
		   ||  	Physics.Raycast(transform.position, transform.forward, transform.localPosition.magnitude, 1 << 0 ) 
		   // if raycast to character hits a wall
		   ||	Physics.Raycast(transform.position, (follower.cameraFocusTarget.position - transform.position).normalized/*, out hit*/, follower.cameraDistance, 1 << 0) 
		   )
		{
			follower.cameraDistance = Mathf.Lerp (follower.cameraDistance, originalDistance * minimumDistance, approachingSpeed);

//			Debug.Log(hit.collider.gameObject.name);

			_timer = 0f;
			fallbackFrom = follower.cameraDistance;
		}
		else
		{
			if( _timer < fallbackTime )
			{
	//			Debug.Log(_timer);
				follower.cameraDistance = Mathf.Lerp (fallbackFrom, originalDistance, fallbackCurve.Evaluate(_timer / fallbackTime ) );
			}
		}

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
