using UnityEngine;
using System.Collections;

public class LookAtInteractionObject : IInteractableObject 
{
	// The point to look at object from
	public Transform lookFromPoint = null;

	// called by Interactor.cs
	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		SiljaBehaviour siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();

		siljaBeh.FreezeSilja( interactionIsActive, transform, lookFromPoint );
    }
}
