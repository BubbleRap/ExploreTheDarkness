using UnityEngine;
using System.Collections;

public class SewLillebror :IInteractableObject 
{

	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		Inventory siljaInventory = siljaGO.GetComponent<Inventory>();
		if(siljaGO.GetComponent<Inventory>().getLilleBrorPieces() > 4)
		{
			if(siljaGO.GetComponent<CollectLillebrorUI>() == null)
			{
				CollectLillebrorUI objectiveController = siljaGO.AddComponent<CollectLillebrorUI>();
			}
			siljaGO.GetComponent<CollectLillebrorUI>().lillebrorMessage = "";
			siljaGO.GetComponent<CollectLillebrorUI>().multipleTask = false;

			siljaInventory.lillebrorComplete = true;
		}
    }
}
