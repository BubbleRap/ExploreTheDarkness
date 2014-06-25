using UnityEngine;
using System.Collections.Generic;

public class Interactor : MonoBehaviour 
{	
	private GameObject currentInteractionObject = null;
	private List<GameObject> interactionObjects = new List<GameObject>();

	public void OnInteractionEnter( GameObject interactionObject )
	{
		if( !interactionObjects.Contains( interactionObject ) )
			interactionObjects.Add( interactionObject );
		
		currentInteractionObject = interactionObject;
	}

	public void OnInteractionExit( GameObject interactionObject)
	{
		if( interactionObject == currentInteractionObject )
			currentInteractionObject = null;

		if( interactionObjects.Contains( interactionObject ) )
			interactionObjects.Remove( interactionObject );

		if( currentInteractionObject == null && interactionObjects.Count > 0 )
			currentInteractionObject = interactionObjects[0];
		
	}

	void Update () 
	{
		// here should be descibed 3 cases:
		// 1. start interaction
		// 2. stop interaction
		// 3. interaction ends on itself

		// IF INTERACTION BUTTON PRESSED
		if( Input.GetKeyDown(KeyCode.E) && currentInteractionObject != null )
		{
			// I would not use SendMessage, as "Find References" won't work
			// ... And maybe some other reasons
			foreach( MonoBehaviour behaviour in currentInteractionObject.GetComponents<MonoBehaviour>() )
			{
				IInteractableObject interactableInterface = behaviour as IInteractableObject;

				// Toggling all the scripts on the object, that implement the "interface"
				if( interactableInterface != null )
					interactableInterface.Activate();
			}
		}
	}

}
