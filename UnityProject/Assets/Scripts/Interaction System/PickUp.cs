using UnityEngine;
using System.Collections;

public class PickUp :IInteractableObject 
{
	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;
		
		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		Inventory siljaInventory = siljaGO.GetComponent<Inventory>();

		siljaInventory.putInInventory(this.transform.name);

		Destroy(this.transform.gameObject);
	}
}
