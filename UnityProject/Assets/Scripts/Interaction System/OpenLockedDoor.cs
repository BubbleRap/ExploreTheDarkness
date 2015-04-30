using UnityEngine;
using System.Collections;

public class OpenLockedDoor :IInteractableObject 
{
	public DoorOpen doorController;
	
	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		Inventory siljaInventory = siljaGO.GetComponent<Inventory>();

		Debug.Log(siljaInventory.lookUpInventory("Key"));
		if(siljaInventory.lookUpInventory("Key"))
		{
			doorController.openDoor(!doorController.doorIsOpen);
		}
	}
}
