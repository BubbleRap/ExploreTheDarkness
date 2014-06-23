using UnityEngine;
using System.Collections;

public class Interactor : MonoBehaviour 
{	
	private GameObject interactionObject = null;

	public GameObject ObjectToInteract
	{
		get { return interactionObject; }
		set { interactionObject = value; }
	}

	void Update () 
	{
		// here should be descibed 3 cases:
		// 1. start interaction
		// 2. stop interaction
		// 3. interaction ends on itself

		// IF INTERACTION BUTTON PRESSED
		if( Input.GetKeyDown(KeyCode.E) && interactionObject != null )
		{
			// I would not use SendMessage, as "Find References" won't work
			// ... And maybe some other reasons
			foreach( MonoBehaviour behaviour in interactionObject.GetComponents<MonoBehaviour>() )
			{
				IInteractableObject interactableInterface = behaviour as IInteractableObject;

				// Toggling all the scripts on the object, that implement the "interface"
				if( interactableInterface != null )
					interactableInterface.Activate();
			}
		}
	}

}
