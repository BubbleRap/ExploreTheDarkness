using UnityEngine;
using System.Collections.Generic;
using System;

public class CameraWhisker
{
    public bool hasHit; 
    public Vector3 direction;
    public float distance;
    public Vector3 hitPoint;
}

[RequireComponent(typeof(CameraFollow))]
public class CameraPhysics : MonoBehaviour 
{
    public Transform HeadBone;

    [Header("Debug")]
    public bool hasHit;
    public float currentHitDistance;

    private CameraFollow follower = null;

    private const int WHISKERS_COUNT = 16;

    private CameraWhisker lineOfSight = new CameraWhisker();
    private CameraWhisker backWhisker = new CameraWhisker();
    private CameraWhisker[] whiskers = new CameraWhisker[WHISKERS_COUNT];

    void Awake()
	{
		follower = GetComponent<CameraFollow> ();

        for(int i = 0; i < WHISKERS_COUNT; i++)
            whiskers[i] = new CameraWhisker();
    }

    public void UpdateCameraCollisions()
	{
        CameraSwingControl();
        CameraDistanceControl();
    }

    // control the distance to the character
    private void CameraDistanceControl()
    {
        CameraWhisker sightLine = CheckLineOfSight();
        CameraWhisker backLine = CheckBackWhisker();

        float distanceFrom, distanceTo, lerpSpeed;

        if(sightLine != null && sightLine.hasHit)
        {
            currentHitDistance = GetMaxDistance(ref sightLine);
            // jump to the hit point
            follower.cameraDistance = distanceTo = currentHitDistance;
        }
        else if(backLine != null && backLine.hasHit)
        {
            currentHitDistance = GetMaxDistance(ref backLine);
            distanceTo = currentHitDistance;
        }
        else
        {
            currentHitDistance = follower.maxDistance;
            distanceTo = follower.maxDistance;
        }

        follower.cameraDistance = Mathf.MoveTowards(follower.cameraDistance, distanceTo, Time.deltaTime);
    }

    // control the obstacles avoidance by directing the camera between 
    // left and right whiskers
    private void CameraSwingControl()
    {
        CameraWhisker left = CheckLeftWhiskers();
        CameraWhisker right = CheckRightWhiskers();

        Vector3 swingToLeft = Vector3.zero, swingToRight = Vector3.zero;

        if(left != null && left.hasHit)
        {
            Vector3 localDirection = transform.InverseTransformDirection(left.direction);
            swingToRight = Vector3.Cross(Vector3.forward, localDirection);
            //swingToRight *= (follower.maxDistance / left.distance);
        }

        if(right != null && right.hasHit)
        {
            Vector3 localDirection = transform.InverseTransformDirection(right.direction);
            swingToLeft = Vector3.Cross(Vector3.forward, localDirection);
            //swingToLeft *= (follower.maxDistance / right.distance);
        }
            
        follower.yaw += (swingToRight.y + swingToLeft.y) * Time.deltaTime * 0.1f;
    }

    private float GetMaxDistance(ref CameraWhisker whisker)
    {
        return Mathf.Max(
                follower.minDistance,
                Mathf.Min(
                follower.maxDistance,
                whisker.distance - 0.3f
            )
        );
    }

    private CameraWhisker CheckLineOfSight()
    {
        CheckCollisionsFor(transform.position, HeadBone.position, ref lineOfSight);

        return lineOfSight;
    }

    private CameraWhisker CheckBackWhisker()
    {
        CheckCollisionsFor(transform.position, transform.position - transform.forward, ref backWhisker);

        return backWhisker;
    }

    private CameraWhisker CheckLeftWhiskers()
    {
        float minDistance = float.MaxValue;

        // default 45 deg whisker
        CameraWhisker closestObstacleWhisker = null;
    
        for( int i = 0; i < (int) (WHISKERS_COUNT * 0.5f); i++ )
        {
            Vector3 direction = Vector3.Lerp(-transform.right, transform.forward, i / (WHISKERS_COUNT * 0.5f));
            CheckCollisionsFor(transform.position, transform.position + direction, ref whiskers[i]);
            if(whiskers[i].hasHit)
            {
                if(whiskers[i].distance < minDistance)
                {
                    closestObstacleWhisker = whiskers[i];
                    minDistance = whiskers[i].distance;
                }
            }
        }

        return closestObstacleWhisker;
    }
    
    private CameraWhisker CheckRightWhiskers()
    {
        float minDistance = float.MaxValue;

        // default 45 deg whisker
        CameraWhisker closestObstacleWhisker = null;
    
        for( int i = (int) (WHISKERS_COUNT * 0.5f); i < WHISKERS_COUNT; i++ )
        {
            Vector3 direction = Vector3.Lerp(transform.right, transform.forward, (WHISKERS_COUNT - i) /  (WHISKERS_COUNT - WHISKERS_COUNT * 0.5f));
            CheckCollisionsFor(transform.position, transform.position + direction, ref whiskers[i]);
            if(whiskers[i].hasHit)
            {
                if(whiskers[i].distance < minDistance)
                {
                    closestObstacleWhisker = whiskers[i];
                    minDistance = whiskers[i].distance;
                }
            }
        }

        return closestObstacleWhisker;
    }



    private void CheckCollisionsFor(Vector3 fromPoint, Vector3 toPoint, ref CameraWhisker result)
    {
        RaycastHit outHit;

        result.distance = (toPoint - fromPoint).magnitude;
        result.hitPoint = toPoint;

        Vector3 direction = (toPoint - fromPoint).normalized;
        bool hasHit = 
            Physics.Raycast(
                fromPoint,
                direction,
                out outHit,
                follower.maxDistance,
                ((1 << LayerMask.NameToLayer("Default")))
            );
                
        Debug.DrawLine(fromPoint, fromPoint + direction * result.distance, hasHit ? Color.red : Color.green);

        result.hasHit = hasHit;
        result.direction = direction;

        if(hasHit)
        {
            result.distance = outHit.distance;
            result.hitPoint = outHit.point;
        }
    }
}
