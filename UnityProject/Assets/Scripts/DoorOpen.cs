using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour {

	private bool doorIsOpen = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void openDoor (bool open) {
		if(!doorIsOpen && open)
		{
			transform.Rotate(0, 0, 80);
			doorIsOpen = true;
		}
		else if(doorIsOpen && !open)
		{
			transform.Rotate(0, 0, -80);
			doorIsOpen = false;
		}
	}
}
