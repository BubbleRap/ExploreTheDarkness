using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	private Transitioner transitionContoller = null;
	public bool DarkMode;

	public DoorOpen doorController;
	public bool OpenDoor;

	public Respawn spawnController;

	// Use this for initialization
	void Awake () {
		transitionContoller = Component.FindObjectOfType(typeof(Transitioner)) as Transitioner;
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			transitionContoller.doTransition(DarkMode);
			doorController.openDoor(OpenDoor);
			spawnController.SetRespawnPosition(transform.position);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
