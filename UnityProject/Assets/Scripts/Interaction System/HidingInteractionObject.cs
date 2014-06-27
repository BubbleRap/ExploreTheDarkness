using UnityEngine;
using System.Collections;
using RAIN.Entities;

public class HidingInteractionObject : IInteractableObject 
{
	public Transform hidePositionPoint;
	public Transform unhidePositionPoint;

	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		SiljaBehaviour siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();
		EntityRig aiSensor = siljaGO.GetComponent<EntityRig>();

		// gettin in the object
		if( interactionIsActive )
		{			
			siljaBeh.LookAtPoint(true, unhidePositionPoint, hidePositionPoint);
		}
		// getting out of the object
		else
		{
			siljaBeh.LookAtPoint(false, hidePositionPoint, unhidePositionPoint);
		}

		aiSensor.Entity.IsActive = interactionIsActive;
	}
}
