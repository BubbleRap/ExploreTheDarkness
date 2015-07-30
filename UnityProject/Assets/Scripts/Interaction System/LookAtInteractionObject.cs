using UnityEngine;
using System.Collections;

public class LookAtInteractionObject : IInteractableObject 
{
	private SiljaBehaviour _siljaBeh = null;

	// called by Interactor.cs
	public override bool Activate()
	{
		if( _siljaBeh == null )
		{
			GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
			_siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();
		}

		if( !ObjectivesManager.Instance.IsInteractionEligable( this ) )
			return false;

		interactionIsActive = !interactionIsActive;


		_siljaBeh.cameraFollow.CamControlType = interactionIsActive ? CameraFollow.CameraControlType.CCT_LookingAtObject : 
			CameraFollow.CameraControlType.CCT_Default;
		
		_siljaBeh.cameraFollow.focusPoint = transform.position;
		
		_siljaBeh.IsMoveEnabled = !interactionIsActive;


		ObjectivesManager.Instance.OnInteractionComplete( this, true );
		return false;
    }
}
