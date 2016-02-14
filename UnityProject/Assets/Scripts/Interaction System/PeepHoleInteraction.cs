using UnityEngine;
using System.Collections;

using UnityStandardAssets.ImageEffects;

public class PeepHoleInteraction : IInteractableObject 
{
	private SiljaBehaviour _siljaBeh;

	private Vector3 _originalPos;
	private Quaternion _originalRot;

	private CameraFollow 		_camFollow;
	private CameraTransitioner 	_camTransitioner;
	private ScreenOverlay		_overlay;

    private void Awake()
    {
        _siljaBeh = DarknessManager.Instance.m_mainCharacter;

        _camFollow = _siljaBeh.cameraFollow;
        _camTransitioner = _siljaBeh.camTransitioner;
        _overlay = _siljaBeh.thisCamera.GetComponent<ScreenOverlay>();
    }

	public override bool Activate()
	{
		if( !ObjectivesManager.Instance.IsInteractionEligable( this ) )
			return false;


		interactionIsActive = !interactionIsActive;


		if( interactionIsActive )
		{
			if(_siljaBeh.IsFirstPerson)
				EnablePeepHoleFPP();
			else
				EnablePeepHoleTPP();

			m_onInteractionActivated.Invoke();
		}
		else
		{
			if(_siljaBeh.IsFirstPerson)
				DisablePeepHoleFPP();
			else
				DisablePeepHoleTPP();
		}


		ObjectivesManager.Instance.OnInteractionComplete( this, interactionIsActive );
		return interactionIsActive;
	}

	private void EnablePeepHoleFPP()
	{
		Debug.Log("EnablePeepHoleFPP");
		_siljaBeh.IsMoveEnabled = false;

		_originalPos = _camTransitioner.FPPCameraTransform.position;
		_originalRot = _camTransitioner.FPPCameraTransform.rotation;

		_camTransitioner.FPPCameraTransform.position = transform.position;
		_camTransitioner.FPPCameraTransform.rotation = transform.rotation;


		_camTransitioner.Transition( 
		                            _siljaBeh.thisCamera.transform, _camTransitioner.FPPCameraTransform,
		                            "OnFPPStart", "OnFPPToComplete", 
		          				 	gameObject, gameObject );
	}

	private void DisablePeepHoleFPP()
	{
		Debug.Log("DisablePeepHoleFPP");
		_overlay.enabled = false;

		_camTransitioner.FPPCameraTransform.position = _originalPos;
		_camTransitioner.FPPCameraTransform.rotation = _originalRot;

		_camTransitioner.Transition( 
		                            _siljaBeh.thisCamera.transform, _camTransitioner.FPPCameraTransform,
		                            "OnFPPStart", "OnFPPFromComplete", 
		           					gameObject, gameObject );
	}

	private void EnablePeepHoleTPP()
	{
		Debug.Log("EnablePeepHoleTPP");
		_originalPos = _camTransitioner.FPPCameraTransform.position;
		_originalRot = _camTransitioner.FPPCameraTransform.rotation;
		
		_camTransitioner.FPPCameraTransform.position = transform.position;
		_camTransitioner.FPPCameraTransform.rotation = transform.rotation;
		
		_siljaBeh.IsMoveEnabled = false;
		
		_camTransitioner.AddFPPCompleteAction( () =>
		                                               {
			// setting up the character motion state
			
			_camFollow.CamControlType = CameraFollow.CameraControlType.CCT_Overwritten;
			_overlay.enabled = true;
		});


        if (LightStatesMachine.Instance.IsLightOn())
            _siljaBeh.ShiftToThirdPerson();
        else
            _siljaBeh.ShiftToFirstPerson();
    }

	private void DisablePeepHoleTPP()
	{
		_overlay.enabled = false;
		
		_camTransitioner.AddTPPCompleteAction( () =>
		                                               {	
			// setting up the character motion state
			
			_camFollow.CamControlType = CameraFollow.CameraControlType.CCT_Default;
			_siljaBeh.IsMoveEnabled = true;
			
			_camTransitioner.FPPCameraTransform.position = _originalPos;
			_camTransitioner.FPPCameraTransform.rotation = _originalRot;
		});
		
		_siljaBeh.ShiftToThirdPerson();
	}

	private void OnFPPStart()
	{
		_camTransitioner.Mode = CameraTransitioner.CameraMode.Transitioning;
	}

	private void OnFPPToComplete()
	{
		_camTransitioner.Mode = CameraTransitioner.CameraMode.Fpp;
		_camFollow.CamControlType = CameraFollow.CameraControlType.CCT_Overwritten;
		_overlay.enabled = true;
	}

	private void OnFPPFromComplete()
	{
		_camTransitioner.Mode = CameraTransitioner.CameraMode.Fpp;
		_camFollow.CamControlType = CameraFollow.CameraControlType.CCT_FPSLook;
		_siljaBeh.IsMoveEnabled = true;

	}
}
