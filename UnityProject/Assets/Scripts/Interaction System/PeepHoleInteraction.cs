using UnityEngine;
using System.Collections;

using UnityStandardAssets.ImageEffects;

public class PeepHoleInteraction : IInteractableObject 
{
//	[Range(0.001f, 0.1f)]
//	public float _rotationSensetivity = 0.1f;

	private SiljaBehaviour _siljaBeh;
//	private Vector3 _prevMousePos;

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

			transitioner.AddFPPCompleteAction( () =>
			{
				// setting up the character motion state

				camControl.CamControlType = CameraFollow.CameraControlType.CCT_Overwritten;
//				_siljaBeh.IsFPSLookEnabled = false;
				_siljaBeh.IsMoveEnabled = false;

//				_siljaBeh.thisCamera.GetComponent<CameraInput>().enabled = false;
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
	}

	private void Update()
	{
//		Vector3 mouseVelocity = Input.mousePosition - _prevMousePos;
//		_prevMousePos = Input.mousePosition;
//
//		if( interactionIsActive )
//		{
//			if( Input.GetMouseButton(0) )
//			{
//				transform.Rotate( mouseVelocity.y * _rotationSensetivity, mouseVelocity.x * _rotationSensetivity, 0f );
//			}
//		}
	}
}
