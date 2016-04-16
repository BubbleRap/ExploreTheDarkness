using UnityEngine;
using System.Collections.Generic;

public class Interactor : MonoBehaviour 
{	
    private IInteractableObject currentInteractionObject = null;
    private List<IInteractableObject> interactionObjects = new List<IInteractableObject>();

    public bool IsInteracting { get; set; }
    public int CloseInteractionsCount { get {return interactionObjects.Count;} }

    public void OnInteractionEnter( IInteractableObject interactionObject )
	{
		if( !interactionObjects.Contains( interactionObject ) )
			interactionObjects.Add( interactionObject );
	}

    public void OnInteractionExit( IInteractableObject interactionObject)
	{
		if( interactionObjects.Contains( interactionObject ) )
			interactionObjects.Remove( interactionObject );	

		if( currentInteractionObject == interactionObject )
			currentInteractionObject = null;
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
            IsInteracting = false;

			IInteractableObject[] interactableInterfaces = currentInteractionObject.GetComponents<IInteractableObject>();
            foreach (IInteractableObject ie in interactableInterfaces)
            {
                IsInteracting = ie.Activate() || IsInteracting;
            }
			
		}
	}

}
