using UnityEngine;
using System.Collections.Generic;

public class SiljaShakeOnScary : MonoBehaviour 
{
	[HideInInspector]
	public CameraShaker firstPersonCameraShaker;
	private List<GameObject> scaryObjectsInSight = new List<GameObject> ();

//	private CameraMotionBlur blurComponent;
//	private MotionBlur motionBlurComponent;

	private bool isScared = false;
	private float scaryAmount = 0f;

	public bool IsScared
	{
		get{ return isScared; }
		set{ isScared = value; }
	}

	void Awake()
	{
		firstPersonCameraShaker = transform.FindChild("1st Person Camera").GetComponent<CameraShaker>();
//		blurComponent = firstPersonCameraShaker.GetComponent<CameraMotionBlur> ();
//		motionBlurComponent = firstPersonCameraShaker.GetComponent<MotionBlur> ();
	}

	void Update()
	{
		// there are objects in sight
		if (isScared) 
		{
			// §if it is enabled, increase shaking
			if( firstPersonCameraShaker.shaking )
			{
				scaryAmount = Mathf.Clamp( scaryAmount + 0.02f, 0f, 1f );

				firstPersonCameraShaker.horizontalShakeIntensity = scaryAmount;
				firstPersonCameraShaker.verticalShakeIntensity = scaryAmount;
//				motionBlurComponent.blurAmount = scaryAmount;
			}
			else
			{
				scaryAmount = 0f;
				
				firstPersonCameraShaker.shakeFrequency = 0.05f;
				
				firstPersonCameraShaker.EnableShake ();
//				blurComponent.enabled = true;
				
//				motionBlurComponent.enabled = true;
			
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
//				motionBlurComponent.blurAmount = scaryAmount;

				// disable, where shake amount is 0
				if( scaryAmount <= 0 )
				{
					firstPersonCameraShaker.DisableShake ();
//					blurComponent.enabled = false;
//					motionBlurComponent.enabled = false;
				}
			}
		}
	}
}
