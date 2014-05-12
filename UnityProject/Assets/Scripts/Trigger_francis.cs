using UnityEngine;
using System.Collections;

public class Trigger_francis : MonoBehaviour {

	public SiljaBehaviour siljaBeh;

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			siljaBeh.SetLightIntensity(0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
