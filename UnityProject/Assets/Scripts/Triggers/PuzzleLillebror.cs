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
			if(other.transform.GetComponent<Inventory>().getLilleBrorPieces() > 4)
			{
				Door.isLocked = false;
			}
			else
			{
				Debug.Log("You do not have lillebror collected yet");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
