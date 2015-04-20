using UnityEngine;
using System.Collections;

public class Trigger_darknessTimer : MonoBehaviour {
	public SiljaBehaviour SijlaBeh;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			SijlaBeh.isDarknessApproaching(false);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player")
		{
			SijlaBeh.isDarknessApproaching(true);
			Destroy(this);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
