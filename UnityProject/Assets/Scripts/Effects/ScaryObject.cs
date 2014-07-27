using UnityEngine;
using System.Collections;

public class ScaryObject : MonoBehaviour
{
	private SiljaShakeOnScary siljaScaryComp = null;

	void Start()
	{
		GameObject silja = GameObject.FindGameObjectWithTag ("Player");
		siljaScaryComp = silja.GetComponent<SiljaShakeOnScary> ();
	}

	void Update()
	{
		Vector3 cameraRelativePosition = siljaScaryComp.transform.InverseTransformPoint(transform.position);
		
		// if the object withing screen space
		bool isInView = cameraRelativePosition.x < 1.0f * cameraRelativePosition.z && 
						cameraRelativePosition.x > -1.0f * cameraRelativePosition.z;


		// check if any of the objects are in the direct sight
		bool inDirectSight = false;

		RaycastHit hitOut;
		
		// cast Default layer only
		LayerMask mask = 1 << 0;
		
		if( Physics.Linecast(siljaScaryComp.firstPersonCameraShaker.transform.position, transform.position, out hitOut, mask ) )
		{
			if( hitOut.collider == collider )
			{
				inDirectSight = true;
			}
		}

		siljaScaryComp.IsScared = inDirectSight && isInView;
	}
}
