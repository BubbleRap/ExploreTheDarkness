using UnityEngine;
using System.Collections;

public class PickUpLillebrorPiece : IInteractableObject 
{

	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		Inventory siljaInventory = siljaGO.GetComponent<Inventory>();

		if(siljaGO.GetComponent<CollectLillebrorUI>() == null)
		{
			CollectLillebrorUI objectiveController = siljaGO.AddComponent<CollectLillebrorUI>();
		}
		siljaGO.GetComponent<CollectLillebrorUI>().lillebrorMessage = "Collect the pieces";
		siljaGO.GetComponent<CollectLillebrorUI>().multipleTask = true;

		if(siljaGO.GetComponent<Inventory>().getLilleBrorPieces() == 4)
		{
			siljaGO.GetComponent<CollectLillebrorUI>().lillebrorMessage = "Sew Lillebror together";
			siljaGO.GetComponent<CollectLillebrorUI>().multipleTask = false;
		}

		siljaInventory.setLilleBrorPieces();
		Destroy(transform.gameObject);
    }
}
