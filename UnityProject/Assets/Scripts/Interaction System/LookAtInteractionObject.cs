using UnityEngine;
using System.Collections;

public class LookAtInteractionObject : IInteractableObject 
{
	// The point to look at object from
	public Transform lookFromPoint = null;

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
		_siljaBeh.LookAtPointFP( interactionIsActive, transform, lookFromPoint );

		return false;
    }
}
