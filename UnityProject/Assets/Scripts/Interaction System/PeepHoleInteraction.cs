using UnityEngine;
using System.Collections;

using UnityStandardAssets.ImageEffects;

public class PeepHoleInteraction : IInteractableObject 
{
	private SiljaBehaviour _siljaBeh;

	private Vector3 _originalPos;
	private Quaternion _originalRot;


	public override bool Activate()
	{
		interactionIsActive = !interactionIsActive;

		if( _siljaBeh == null )
		{
			GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
			_siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();
		}

		CameraFollow camControl = _siljaBeh.thisCamera.GetComponent<CameraFollow>();

		if( interactionIsActive )
		{
			_originalPos = _siljaBeh.camTransitioner.FPPCameraTransform.position;
			_originalRot = _siljaBeh.camTransitioner.FPPCameraTransform.rotation;

			_siljaBeh.camTransitioner.FPPCameraTransform.position = transform.position;
			_siljaBeh.camTransitioner.FPPCameraTransform.rotation = transform.rotation;

			_siljaBeh.IsMoveEnabled = false;

			_siljaBeh.camTransitioner.AddFPPCompleteAction( () =>
			{
				// setting up the character motion state

				camControl.CamControlType = CameraFollow.CameraControlType.CCT_Overwritten;
				_siljaBeh.thisCamera.GetComponent<ScreenOverlay>().enabled = true;
			});
			

			_siljaBeh.ShiftToFirstPerson();
		}
		else
		{
			_siljaBeh.thisCamera.GetComponent<ScreenOverlay>().enabled = false;

			_siljaBeh.camTransitioner.AddTPPCompleteAction( () =>
			{	
				// setting up the character motion state

				camControl.CamControlType = CameraFollow.CameraControlType.CCT_Default;
				_siljaBeh.IsMoveEnabled = true;

				_siljaBeh.camTransitioner.FPPCameraTransform.position = _originalPos;
				_siljaBeh.camTransitioner.FPPCameraTransform.rotation = _originalRot;
			});

			_siljaBeh.ShiftToThirdPerson();
		}


		IsCompleted = interactionIsActive;
		return interactionIsActive;
	}
}
