using UnityEngine;
using System.Collections.Generic;
using System;

public class RaycastResult
{
    public bool hasHit; 
    public float distance;
    public Vector3 hitPoint;
}

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

    private const int WHISKERS_COUNT = 16;

    private RaycastResult lineOfSight = new RaycastResult();
    private RaycastResult backWhisker = new RaycastResult();
    private RaycastResult[] whiskers = new RaycastResult[WHISKERS_COUNT];


	private float _timer = 5f;
	private float fallbackFrom = 0f;

    void Awake()
	{
		follower = GetComponent<CameraFollow> ();

        for(int i = 0; i < WHISKERS_COUNT; i++)
            whiskers[i] = new RaycastResult();
    }

    void Update()
	{

		_timer += Time.deltaTime;
		_timer = Mathf.Clamp(_timer, 0f, fallbackTime);

        CheckLeftWhiskers();
        CheckRightWhiskers();

        if(CheckBackWhisker())
        {
            currentHitDistance = GetMaxDistance(ref backWhisker);

            if (currentHitDistance < follower.cameraDistance)
            {
                //OPTION 1: Lerping the camera to quickly solve the tunneling. Elastic.
                follower.cameraDistance =
                    Mathf.Lerp(
                        follower.cameraDistance,
                        currentHitDistance,
                        approachingSpeed * Time.deltaTime
                );
            
                // OPTION 2: very strct camera distance, never tunneling through walls
                //follower.cameraDistance = currentHitDistance;
            
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

    private float GetMaxDistance(ref RaycastResult whisker)
    {
        return Mathf.Max(
            follower.minDistance,
                Mathf.Min(
                follower.maxDistance,
                whisker.distance - 0.2f
            )
        );
    }

    private bool CheckLineOfSight()
    {
        CheckCollisionsFor(transform.position, HeadBone.position, ref lineOfSight);

        return lineOfSight.hasHit;
    }

    private bool CheckBackWhisker()
    {
        CheckCollisionsFor(transform.position, transform.position - transform.forward, ref backWhisker);

        return backWhisker.hasHit;
    }

    private float CheckLeftWhiskers()
    {
        bool isColliding = false;
        float avoidanceMagnitude = 0f;
        int halfWhiskersCount = WHISKERS_COUNT / 2;
    
        for( int i = 0; i < halfWhiskersCount; i++ )
        {
            Vector3 direction = Vector3.Lerp(-transform.right, transform.forward, i / (float)halfWhiskersCount);
            if( CheckCollisionsFor(transform.position, transform.position + direction, ref whiskers[i]) )
                isColliding = true;
        }
    
        avoidanceMagnitude = isColliding ? -1f : 0f;
    
        // return a magnitude of obstacle avoidance to the right
        return avoidanceMagnitude;
    }
    
    private float CheckRightWhiskers()
    {
        bool isColliding = false;
        float avoidanceMagnitude = 0f;
        int halfWhiskersCount = WHISKERS_COUNT / 2;
    
        for( int i = halfWhiskersCount; i < WHISKERS_COUNT; i++ )
        {
            Vector3 direction = Vector3.Lerp(transform.right, transform.forward, (WHISKERS_COUNT - i) / (float) (WHISKERS_COUNT - halfWhiskersCount));
            if(CheckCollisionsFor(transform.position, transform.position + direction, ref whiskers[i]))
                isColliding = true;
        }
    
        avoidanceMagnitude = isColliding ? 1f : 0f;
    
        // return a magnitude of obstacle avoidance to the left
    
        return avoidanceMagnitude;
    }



    private bool CheckCollisionsFor(Vector3 fromPoint, Vector3 toPoint, ref RaycastResult result)
    {
        RaycastHit outHit;

        Vector3 direction = (toPoint - fromPoint).normalized;
        bool hasHit = 
            Physics.Raycast(
                fromPoint,
                direction,
                out outHit,
                follower.maxDistance,
                ((1 << LayerMask.NameToLayer("Default")))
            );

        if(hasHit)
            Debug.DrawLine(fromPoint, outHit.point, Color.red);
        else
            Debug.DrawLine(fromPoint, fromPoint + direction * follower.maxDistance, Color.green);

        result.hasHit = hasHit;
        result.hitPoint = outHit.point;
        result.distance = outHit.distance;

        return hasHit;
    }
}
