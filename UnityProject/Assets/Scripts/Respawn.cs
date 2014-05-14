using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {

	private Vector3 spawnPosition;

	// Use this for initialization
	void Start () {
		spawnPosition = new Vector3(14.41889f,0.8870191f,-27.85333f);
	}
	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetKeyDown(KeyCode.R))
		{
			RespawnToLastPosition();
		}*/
	}

	public void RespawnToLastPosition()
	{
		transform.position = spawnPosition;
	}

	public void SetRespawnPosition(Vector3 newPosition)
	{
		spawnPosition = newPosition;
	}

}
