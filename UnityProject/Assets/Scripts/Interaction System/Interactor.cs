using UnityEngine;
using System.Collections.Generic;

public class Interactor : MonoBehaviour 
{	
	private GameObject currentInteractionObject = null;
	private List<GameObject> interactionObjects = new List<GameObject>();

	[HideInInspector]
	public bool isInteracting = false;

	public void OnInteractionEnter( GameObject interactionObject )
	{
		if( !interactionObjects.Contains( interactionObject ) )
			interactionObjects.Add( interactionObject );

		IInteractableObject promt = interactionObject.GetComponent<IInteractableObject>();
		promt.ActivatePromtButton(true);

	}

	public void OnInteractionExit( GameObject interactionObject)
	{
		if( interactionObjects.Contains( interactionObject ) )
			interactionObjects.Remove( interactionObject );	

		IInteractableObject promt = interactionObject.GetComponent<IInteractableObject>();
		promt.ActivatePromtButton(false);

		if( currentInteractionObject == interactionObject )
		{
			currentInteractionObject = null;
		}
	}

	void Update () 
	{
		// here should be descibed 3 cases:
		// 1. start interaction
		// 2. stop interaction
		// 3. interaction ends on itself


		// select current interaction object going through the list
		float closestDistToCenter = Mathf.Infinity;
		int closestIdx = -1;

		for( int i = 0; i < interactionObjects.Count; i++ )
		{
			// check if object was destroyed
			if( interactionObjects[i] == null)
			{
				interactionObjects.RemoveAt( i );	
				i--;
				continue;
			}

			Vector3 viewPos = Camera.main.WorldToViewportPoint( interactionObjects[i].transform.position );
			float distance = Vector2.Distance( viewPos, Vector2.one * 0.5f );
			if( distance < closestDistToCenter )
			{
				closestDistToCenter = distance;
				closestIdx = i;
			}
		}

		if( interactionObjects.Count == 0 )
			return;

		// switching to the new interactable object
		if( currentInteractionObject != interactionObjects[closestIdx] )
		{
			currentInteractionObject = interactionObjects[closestIdx];
		}


		if( Input.GetKeyDown(KeyCode.E) )
		{
			IInteractableObject interactableInterface = currentInteractionObject.GetComponent<IInteractableObject>();
			isInteracting = interactableInterface.Activate();
		}
	}

}
