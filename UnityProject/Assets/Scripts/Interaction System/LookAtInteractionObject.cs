using UnityEngine;
using System.Collections;
using System;

public class LookAtInteractionObject : IInteractableObject 
{
	private SiljaBehaviour _siljaBeh = null;

    public Transform overrideTransform;
    private Vector3 overrideTransformPos;
    private Quaternion overrideTransformRotation;

    protected override void Awake()
    {
        base.Awake();
        overrideTransformPos = overrideTransform.position;
        overrideTransformRotation = overrideTransform.rotation;
    }

	// called by Interactor.cs
	public override bool Activate()
	{
        StopAllCoroutines();
        overrideTransform.position = overrideTransformPos;
        overrideTransform.rotation = overrideTransformRotation;

		if( _siljaBeh == null )
		{
			GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
			_siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();
        }

        CameraTransitioner transitioner = _siljaBeh.thisCamera.GetComponent<CameraTransitioner>();

        if ( !ObjectivesManager.Instance.IsInteractionEligable( this ) )
			return false;

		interactionIsActive = !interactionIsActive;
       

        if(interactionIsActive)
        {
            _siljaBeh.cameraFollow.CamControlType = CameraFollow.CameraControlType.CCT_LookingAtObject;
        }
        else
        {
            if(LightStatesMachine.Instance.IsLightOn())
                _siljaBeh.cameraFollow.CamControlType = CameraFollow.CameraControlType.CCT_Default;
            else
                _siljaBeh.cameraFollow.CamControlType = CameraFollow.CameraControlType.CCT_FPSLook;
        }
		
		_siljaBeh.cameraFollow.focusPoint = transform.position;
		
        if (interactionIsActive)
        {
            _siljaBeh.IsMoveEnabled = false;
        }
        else
        {
            StartCoroutine(UnfreezeSiljaAfterTransition(transitioner));
        }

		if(interactionIsActive)
		{
			UIManager.Instance.lookAtUI(true);	
		}
		else
		{
			UIManager.Instance.lookAtUI(false);
		}

        if (interactionIsActive)
        {
            if (overrideTransform != null)
            {
                overrideTransform.LookAt(transform);

                if(LightStatesMachine.Instance.IsLightOn())
                    transitioner.TransitionTPPtoOther(overrideTransform);
                else
                    transitioner.TransitionFPPtoOther(overrideTransform);
            }
            else {
                if(LightStatesMachine.Instance.IsLightOn())
                    transitioner.TransitionTPPtoFPP(transform);
                else
                    transitioner.TransitionFPPtoFPP();
            }
        }
        else {
            if (overrideTransform != null)
            {
                overrideTransform.LookAt(transform);

                if(LightStatesMachine.Instance.IsLightOn())
                    transitioner.TransitionOtherToTPP(overrideTransform);
                else
                    transitioner.TransitionOtherToFPP(overrideTransform);
            }
            else {
                if(LightStatesMachine.Instance.IsLightOn())
                    transitioner.TransitionFPPtoTPP();
                else
                    transitioner.TransitionFPPtoFPP();
            }
        }
        
        ObjectivesManager.Instance.OnInteractionComplete( this, true );
		return interactionIsActive;
    }

    private IEnumerator UnfreezeSiljaAfterTransition(CameraTransitioner t)
    {
        while (t.Mode == CameraTransitioner.CameraMode.Transitioning)
            yield return 0;

        _siljaBeh.IsMoveEnabled = true;
    }
}
