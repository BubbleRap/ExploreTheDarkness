using UnityEngine;
using System.Collections;

public class moveObject : MonoBehaviour {

	public Camera camera;
	private GameObject previousObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButton (0)){ 
			RaycastHit hit; 
			Ray ray = camera.ScreenPointToRay(Input.mousePosition); 
			if ( Physics.Raycast (ray,out hit,100.0f)){ 
				if(hit.transform.tag == "Object")
				{
					if(previousObject != null)
					{ 
						if(previousObject != hit.transform.gameObject)
						{
							if(previousObject.GetComponent<Rigidbody>().isKinematic)
							{
								previousObject.GetComponent<Rigidbody>().isKinematic = false;
								previousObject = null;
							}
						}
					}

					Vector3 mousePos = Input.mousePosition;
					mousePos.z = 1.5f;

					mousePos = camera.ScreenToWorldPoint(mousePos);

					previousObject = hit.transform.gameObject;

					hit.transform.GetComponent<Rigidbody>().isKinematic = true;

					hit.transform.position = new Vector3(mousePos.x,mousePos.y,mousePos.z);
				}
			}
		}
		else if(previousObject != null)
		{
			if(previousObject.GetComponent<Rigidbody>().isKinematic)
			{
				previousObject.GetComponent<Rigidbody>().isKinematic = false;
				previousObject = null;
			}
		}
	}
}
