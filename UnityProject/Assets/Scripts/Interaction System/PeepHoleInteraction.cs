using UnityEngine;
using System.Collections;

using UnityStandardAssets.ImageEffects;

public class PeepHoleInteraction : IInteractableObject 
{
	private SiljaBehaviour _siljaBeh;

	private Vector3 _originalPos;
	private Quaternion _originalRot;


	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;

		if( _siljaBeh == null )
		{
			GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
			_siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();
		}

		CameraTransitioner transitioner = _siljaBeh.thisCamera.GetComponent<CameraTransitioner>();
		CameraFollow camControl = _siljaBeh.thisCamera.GetComponent<CameraFollow>();

		if( interactionIsActive )
		{
			_originalPos = transitioner.FPPCameraTransform.position;
			_originalRot = transitioner.FPPCameraTransform.rotation;

			transitioner.FPPCameraTransform.position = transform.position;
			transitioner.FPPCameraTransform.rotation = transform.rotation;

			_siljaBeh.IsMoveEnabled = false;

			transitioner.AddFPPCompleteAction( () =>
			{
				// setting up the character motion state

				camControl.CamControlType = CameraFollow.CameraControlType.CCT_Overwritten;


				_siljaBeh.thisCamera.GetComponent<ScreenOverlay>().enabled = true;
			});
			
			
			_siljaBeh.ShiftToDarkMode();
		}
		else
		{
			_siljaBeh.thisCamera.GetComponent<ScreenOverlay>().enabled = false;

			transitioner.AddTPPCompleteAction( () =>
			{	
				// setting up the character motion state

//				_siljaBeh.thisCamera.GetComponent<CameraInput>().enabled = true;
				camControl.CamControlType = CameraFollow.CameraControlType.CCT_Default;
				_siljaBeh.IsMoveEnabled = true;

				transitioner.FPPCameraTransform.position = _originalPos;
				transitioner.FPPCameraTransform.rotation = _originalRot;
			});

			_siljaBeh.ShiftToStoryMode();
		}

		IsCompleted = interactionIsActive;
	}
}
