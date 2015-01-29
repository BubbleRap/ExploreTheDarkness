using UnityEngine;
using System.Collections;

public class PuzzleLillebror : MonoBehaviour {

	public DoorOpenController Door;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Player")
		{
			if(other.transform.GetComponent<Inventory>().lillebrorComplete)
			{
				Door.isLocked = false;
			}
			else
			{
				if(other.transform.GetComponent<CollectLillebrorUI>() == null)
				{
					if(other.transform.gameObject.GetComponent<CollectLillebrorUI>() == null)
					{
						CollectLillebrorUI objectiveController = other.transform.gameObject.AddComponent<CollectLillebrorUI>();
						objectiveController.lillebrorMessage = "Find Lillebror";
						objectiveController.multipleTask = false;
					}
				}
				Debug.Log("You do not have lillebror collected yet");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
