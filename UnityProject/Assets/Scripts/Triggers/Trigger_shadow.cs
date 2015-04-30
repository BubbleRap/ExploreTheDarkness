using UnityEngine;
using System.Collections;

public class Trigger_shadow : MonoBehaviour {

	public DoorOpen doorController;
	public bool OpenDoor;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			doorController.openDoor(OpenDoor);
			Destroy(this.transform.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
