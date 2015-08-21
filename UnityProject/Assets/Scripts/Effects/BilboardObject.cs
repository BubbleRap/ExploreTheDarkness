using UnityEngine;
using System.Collections;

public class BilboardObject : MonoBehaviour 
{
	public enum DimensionalSpace
	{
		XZPlane,
		FullSpace
	}

	public DimensionalSpace space;
	public Transform lookAtTarget;
	public bool m_initializeTargetAsPlayer = true;

	void Awake()
	{
		if( m_initializeTargetAsPlayer )
		{
			GameObject silja = GameObject.FindGameObjectWithTag ("Player");
			lookAtTarget = silja.transform;
		}
	}

	void Update () 
	{
		if( lookAtTarget != null )
		{
			if( space == DimensionalSpace.FullSpace )
				transform.LookAt(lookAtTarget);

			if( space == DimensionalSpace.XZPlane )
			{
				Vector3 direction = (lookAtTarget.position - transform.position).normalized;
				direction.y = 0f;
				transform.rotation = Quaternion.LookRotation(direction);
			}
		}
	}
	
}
