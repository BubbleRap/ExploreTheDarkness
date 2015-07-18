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

		interactionIsActive = !interactionIsActive;
		IsCompleted = true;

		_siljaBeh.cameraFollow.CamControlType = interactionIsActive ? CameraFollow.CameraControlType.CCT_LookingAtObject : 
			CameraFollow.CameraControlType.CCT_Default;
		
		_siljaBeh.cameraFollow.focusPoint = transform.position;
		
		_siljaBeh.IsMoveEnabled = !interactionIsActive;

		return false;
    }
}
