using UnityEngine;
using System.Collections;

public class Trigger_francis : MonoBehaviour {

	public SiljaBehaviour siljaBeh;
	private AIBehaviour[] aiEntities = null;

	// Use this for initialization
	void Start () {
	}

	void Awake() {
		aiEntities = FindObjectsOfType<AIBehaviour>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			foreach( AIBehaviour aiEntity in aiEntities )
				aiEntity.SpawnAI();

			siljaBeh.SetLightIntensity(0f);
			Destroy(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
