using UnityEngine;
using System.Collections;

public class BilboardObject : MonoBehaviour 
{
	public Transform lookAtTarget;

	void Update () 
	{
		transform.LookAt(lookAtTarget);
	}

	void OnBecameVisible()
	{
		Debug.Log("Became Visible");
	}

	void OnBecameInvisible()
	{
		Debug.Log("Became Invisible");
	}
}
