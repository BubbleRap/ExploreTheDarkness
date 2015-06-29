using UnityEngine;
using System.Collections;

public class CollectLillebrorUI : MonoBehaviour {

	public string lillebrorMessage;
	public bool multipleTask = false;
	private int lilleBrorPiecesTotal = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		ObjectivesManager siljaObjective = siljaGO.GetComponent<ObjectivesManager>();
		Inventory siljaInventory = siljaGO.GetComponent<Inventory>();
		
		if(multipleTask)
		{
			siljaObjective.setTheObjective(lillebrorMessage + " " + siljaInventory.getLilleBrorPieces() + "/" + lilleBrorPiecesTotal);
		}
		else
		{
			siljaObjective.setTheObjective(lillebrorMessage);
		}
	}
}
