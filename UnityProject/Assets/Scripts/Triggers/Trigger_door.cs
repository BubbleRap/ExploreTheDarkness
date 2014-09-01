using UnityEngine;
using System.Collections;

public class Trigger_door : MonoBehaviour {

	public GameObject door;
	private DoorOpenController doorController;
	public float rotateTo;

	private bool startTimer = false;
	private float TriggerTimer = 0.0f;

	[Range(0f, 5.00f)]
	public float duration = 0.0f;

	public bool lightAffected = false;
	public SiljaBehaviour siljaBeh;

	// Use this for initialization
	void Start () {
		doorController = door.GetComponent<DoorOpenController>();
	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Player")
		{
			if(!lightAffected || siljaBeh.getTeddyLight() <= siljaBeh.getMimimumIntensity())
			{
				if(!startTimer)
				{
					startTimer = true;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(startTimer)
		{
			TriggerTimer += Time.deltaTime;
		}

		if(TriggerTimer >= duration)
		{
			doorController.swingDoorTo(rotateTo);
			Destroy(this.gameObject);
		}
	}
}
