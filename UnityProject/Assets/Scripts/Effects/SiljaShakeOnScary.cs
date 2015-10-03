using UnityEngine;
using System.Collections.Generic;

using UnityStandardAssets.ImageEffects;

public class SiljaShakeOnScary : MonoBehaviour 
{
	[HideInInspector]
	public CameraShaker firstPersonCameraShaker;
	private List<GameObject> scaryObjectsInSight = new List<GameObject> ();
	
	private MotionBlur motionBlurComponent;
	
	private float scaryAmount = 0f;

	private SiljaBehaviour siljaBeh;

	void Awake()
	{
		firstPersonCameraShaker = GetComponentInChildren<CameraShaker>();
		siljaBeh = GetComponent<SiljaBehaviour>();

		motionBlurComponent = firstPersonCameraShaker.GetComponent<MotionBlur> ();
	}

	void Update()
	{
		// there are objects in sight
		if (siljaBeh.IsScared) 
		{
			// §if it is enabled, increase shaking
			if( firstPersonCameraShaker.shaking )
			{
				scaryAmount = Mathf.Clamp( scaryAmount + 0.02f, 0f, 1f );

				firstPersonCameraShaker.horizontalShakeIntensity = scaryAmount;
				firstPersonCameraShaker.verticalShakeIntensity = scaryAmount;

				motionBlurComponent.blurAmount = scaryAmount;
			}
			else
			{
				scaryAmount = 0f;
				
				firstPersonCameraShaker.shakeFrequency = 0.05f;	
				firstPersonCameraShaker.EnableShake ();
							
				motionBlurComponent.enabled = true;
			
			}
		} 
		else 
		{
			// if there are no objecs, but there is still shaking 
			// -> dump it down
			if( firstPersonCameraShaker.shaking )
			{
				scaryAmount = Mathf.Clamp( scaryAmount - 0.04f, 0f, 1f );
				
				firstPersonCameraShaker.horizontalShakeIntensity = scaryAmount;
				firstPersonCameraShaker.verticalShakeIntensity = scaryAmount;

				motionBlurComponent.blurAmount = scaryAmount;

				// disable, where shake amount is 0
				if( scaryAmount <= 0 )
				{
					firstPersonCameraShaker.DisableShake ();

					motionBlurComponent.enabled = false;
				}
			}
		}
	}
}
