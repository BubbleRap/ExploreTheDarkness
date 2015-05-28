using UnityEngine;
using System.Collections;

public class LookThroughHoleInteraction : IInteractableObject 
{
//	public Transform lookFromPoint = null;
	
	private SiljaBehaviour _siljaBeh = null;
	
	// called by Interactor.cs
	public override void Activate()
	{
		if( _siljaBeh == null )
		{
			GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
			_siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();
		}
		
		interactionIsActive = !interactionIsActive;
		_siljaBeh.LookAtPointFP( interactionIsActive, transform.position + transform.forward, transform.position );
	}
}
