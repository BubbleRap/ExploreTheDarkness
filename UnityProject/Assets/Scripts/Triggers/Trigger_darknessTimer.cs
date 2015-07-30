using UnityEngine;
using System.Collections;

public class Trigger_darknessTimer : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			other.GetComponent<SiljaBehaviour>().isDarknessApproaching(false);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player")
		{
			other.GetComponent<SiljaBehaviour>().isDarknessApproaching(true);
			Destroy(this);
		}
	}
}
