using UnityEngine;
using System.Collections;

public class Trigger_francis : MonoBehaviour {

	public SiljaBehaviour siljaBeh;
	public Health healthController;
	private AIBehaviour[] aiEntities = null;
	private int health;

	// Use this for initialization
	void Start () {
	}

	void Awake() {
		aiEntities = FindObjectsOfType<AIBehaviour>();
	}

	void OnTriggerEnter(Collider other) {
		health = healthController.health;
	}

	void OnTriggerStay(Collider other) {
		if(other.tag == "Player")
		{
			if(siljaBeh.getTeddyLight() <= siljaBeh.getMimimumIntensity())
			{
				foreach( AIBehaviour aiEntity in aiEntities )
					aiEntity.SpawnAI();

			}

			if(health > healthController.health)
			{
				Destroy(this);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player")
		{
			foreach( AIBehaviour aiEntity in aiEntities )
				aiEntity.DespawnAI();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
