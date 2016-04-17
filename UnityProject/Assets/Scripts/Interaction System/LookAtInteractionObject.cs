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

		_siljaBeh.cameraFollow.CamControlType = interactionIsActive ? CameraFollow.CameraControlType.CCT_LookingAtObject : 
			CameraFollow.CameraControlType.CCT_Default;
		
		_siljaBeh.cameraFollow.focusPoint = transform.position;
		
        if (interactionIsActive)
        {
            _siljaBeh.IsMoveEnabled = false;
        }
        else
        {
            StartCoroutine(UnfreezeSiljaAfterTransition(transitioner));
        }

        if (interactionIsActive)
        {
            if (overrideTransform != null)
            {
                overrideTransform.LookAt(transform);
                transitioner.TransitionTPPtoOther(overrideTransform);
            }
            else {
                transitioner.TransitionTPPtoFPP(transform);
            }
        }
        else {
            if (overrideTransform != null)
            {
                overrideTransform.LookAt(transform);
                transitioner.TransitionOtherToTPP(overrideTransform);
            }
            else {
                transitioner.TransitionFPPtoTPP();
            }
        }
        
        ObjectivesManager.Instance.OnInteractionComplete( this, true );
		return false;
    }

    private IEnumerator UnfreezeSiljaAfterTransition(CameraTransitioner t)
    {
        while (t.Mode == CameraTransitioner.CameraMode.Transitioning)
            yield return 0;

        _siljaBeh.IsMoveEnabled = true;
    }
}
