using UnityEngine;
using System.Collections;

public class PickUp :IInteractableObject 
{
	public override bool Activate()
	{
		interactionIsActive = !interactionIsActive;
		
		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		Inventory siljaInventory = siljaGO.GetComponent<Inventory>();

		siljaInventory.putInInventory(this.transform.name);

		Destroy(this.transform.gameObject);

		return false;
	}
}
