using UnityEngine;
using System.Collections;

public class IInteractableObject : MonoBehaviour 
{
	protected bool interactionIsActive = false;

	public string TextToDisplay;

	public virtual void Activate()
	{
		Debug.Log("Virtual call");
	}
	
}
