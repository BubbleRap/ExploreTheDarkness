using UnityEngine;
using System.Collections;

public class PickUpLillebrorPiece : IInteractableObject 
{

	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		Inventory siljaInventory = siljaGO.GetComponent<Inventory>();

		siljaInventory.setLilleBrorPieces();
		Destroy(transform.gameObject);
    }
}
