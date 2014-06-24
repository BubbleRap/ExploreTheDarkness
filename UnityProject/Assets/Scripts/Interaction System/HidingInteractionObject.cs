﻿using UnityEngine;
using System.Collections;

public class HidingInteractionObject : IInteractableObject 
{
	public Transform hidePositionPoint;
	public Transform unhidePositionPoint;

	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		SiljaBehaviour siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();

		// gettin in the object
		if( interactionIsActive )
		{			
			siljaBeh.FreezeSilja(true, unhidePositionPoint, hidePositionPoint);
		}
		// getting out of the object
		else
		{
			siljaBeh.FreezeSilja(false, hidePositionPoint, unhidePositionPoint);
		}
	}
}
