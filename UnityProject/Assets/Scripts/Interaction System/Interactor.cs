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
	}

	public void OnInteractionExit( GameObject interactionObject)
	{
		if( interactionObjects.Contains( interactionObject ) )
			interactionObjects.Remove( interactionObject );	
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
			if( interactionObjects[i] == null )
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
			PromtButtonInteractionObject currentPromt;


			// disabling promt for the old object
			if( currentInteractionObject != null )
			{
				currentPromt = currentInteractionObject.GetComponent<PromtButtonInteractionObject>();
				currentPromt.ActivatePromtButton(false);
			}

			currentInteractionObject = interactionObjects[closestIdx];

			// enabling promt for the new object
			currentPromt = currentInteractionObject.GetComponent<PromtButtonInteractionObject>();
			currentPromt.ActivatePromtButton(true);
		}


		if( Input.GetKeyDown(KeyCode.E) /* || Input.GetMouseButtonDown(0) */)
		{
			foreach( MonoBehaviour behaviour in currentInteractionObject.GetComponents<MonoBehaviour>() )
			{
				IInteractableObject interactableInterface = behaviour as IInteractableObject;

				// Toggling all the scripts on the object, that implement the "interface"
				if( interactableInterface != null )
				{

					// So here, I am thinkering having a check, if an Activate returns false, the whole sequence returns false
					// but now, it is time for hacks


					// >>>da hack
					// check, if there is a glow component, which will tell us
					// whether player is looking at the object

//					GlowInteractionObject glowCom = interactableInterface as GlowInteractionObject;
//					if( glowCom != null )
//						if( !glowCom.activated )
//							return;


					interactableInterface.Activate();
				}
			}
		}
	}

}
