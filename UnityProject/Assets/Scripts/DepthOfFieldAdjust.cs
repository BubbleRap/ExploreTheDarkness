using UnityEngine;
using System.Collections;

public class DepthOfFieldAdjust : MonoBehaviour {

//	private DepthOfField34 Dof34 = null;
//	public Transform target;
//	public Transform Origin;
//
//	private Vector3 newTargetPos;
//	private float Distance = 0.0f;
//
//	private bool Diffuse = false;
//
//	// Use this for initialization
//	void Start () {
//		Dof34 = GetComponent<DepthOfField34>();
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		Ray ray = new Ray(Origin.position, Origin.forward);
//        RaycastHit hit = new RaycastHit ();
//
//        if (Physics.Raycast (ray, out hit, Mathf.Infinity))
//        {
//        	if(hit.transform.gameObject.layer != 13)
//        	{
//	        	Debug.Log(hit.transform.name);
//	            Dof34.objectFocus = target;
//	            newTargetPos = hit.point;
//	            Distance = hit.distance;
//        	}
//        }
//        else
//        {
//        	Dof34.objectFocus = null;
//        }
//
//        if(target.transform.position != newTargetPos && !Diffuse)
//	    {
//	    	target.transform.position = Vector3.Lerp(target.transform.position, newTargetPos, (Time.deltaTime * 3)/(Distance * 1.2f));
//	    	//Debug.Log(target.transform.position);
//	    }
//	    else if(Diffuse)
//	    {
//	    	target.transform.position = newTargetPos;
//	    	Diffuse = false;
//	    }
//
//        if(Input.GetAxis("Mouse X") > 1.5f || Input.GetAxis("Mouse X") < -1.5f || Input.GetAxis("Mouse Y") > 1.5f || Input.GetAxis("Mouse Y") < -1.5f)
//        {
//        	target.transform.localPosition = new Vector3(0,0,0.8f);
//        	Diffuse = true;
//        }
//	}
}
