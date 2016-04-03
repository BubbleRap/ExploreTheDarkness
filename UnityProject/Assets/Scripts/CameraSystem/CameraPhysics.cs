using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CameraFollow))]
public class CameraPhysics : MonoBehaviour 
{
	// Percents
	[Range(0f, 10f)]
    [Tooltip("In units per second")]
	public float approachingSpeed = 0.1f;

	public AnimationCurve fallbackCurve;
	[Range(0f, 5f)]
	public float fallbackTime = 5f;

    public Transform HeadBone;

    [Header("Debug")]
    public bool hasHit;
    public float currentHitDistance;

    private CameraFollow follower = null;

    private RaycastHit[] raycasts = new RaycastHit[1];

	private float _timer = 5f;
	private float fallbackFrom = 0f;
    private List<Collider> collidingColliders = new List<Collider>();

    void Awake()
	{
		follower = GetComponent<CameraFollow> ();
    }

    void Update()
	{

		_timer += Time.deltaTime;
		_timer = Mathf.Clamp(_timer, 0f, fallbackTime);

        hasHit = CheckForHits();

		if(hasHit)
		{
            currentHitDistance = GetMaxDistance();

            if (currentHitDistance < follower.cameraDistance)
            {
                // if there is a hit, then get a close up
                follower.cameraDistance =
                    Mathf.Lerp(
                        follower.cameraDistance,
                        currentHitDistance,
                        approachingSpeed * Time.deltaTime //* ((collidingColliders.Count > 0) ? 2 : 1)
                );

                _timer = 0f;
                fallbackFrom = follower.cameraDistance;
            }
            else
            {
                if (_timer < fallbackTime)
                {
                    // fallback the camera if there is no collision happening
                    follower.cameraDistance = Mathf.Lerp(fallbackFrom, currentHitDistance, fallbackCurve.Evaluate(_timer / fallbackTime));
                }
            }
        }
        else
        {
            currentHitDistance = follower.maxDistance;

            if (_timer < fallbackTime)
            {
                // fallback the camera if there is no collision happening
                follower.cameraDistance = Mathf.Lerp(fallbackFrom, follower.maxDistance, fallbackCurve.Evaluate(_timer / fallbackTime));
            }
        }
    }

    private float GetMaxDistance()
    {
        return Mathf.Max(
            follower.minDistance,
                Mathf.Min(
                follower.maxDistance,
                Vector3.Distance(
                    HeadBone.position,
                    raycasts[0].point
                ) - 0.2f
            )
        );
    }

    private bool CheckForHits()
    {
        raycasts[0] = default(RaycastHit);

        bool hasHit = 
            Physics.Raycast(
               HeadBone.position,
                -transform.forward,
                out raycasts[0],
                follower.maxDistance,
                ((1 << LayerMask.NameToLayer("Default")))
        );

        return hasHit;
    }

    void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(
                HeadBone.position,
                ((raycasts[0].transform != null) ? raycasts[0].point :
                HeadBone.position + transform.forward * -follower.maxDistance));
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Default"))
            collidingColliders.Add(c);
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Default"))
            collidingColliders.Remove(c);
    }
}
