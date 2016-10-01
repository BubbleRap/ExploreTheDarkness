using UnityEngine;
using System.Collections.Generic;

public class Interactor : MonoBehaviour 
{	
    public OnMouseOver m_backgroundOver;
    public OnMouseClick m_backgroundClick;
    public OnMouseDrag m_backgroundDrag;

    public IInteractableObject CurrentObject { get; private set; }

    private Dictionary<IInteractableObject, ButtonPrompt> interactionObjects = 
        new Dictionary<IInteractableObject, ButtonPrompt>();
    
    private List<IInteractableObject> m_sceneInteractions;

    public bool IsInteracting { get; set; }
    public int CloseInteractionsCount { get {return interactionObjects.Count;} }

    public void OnInteractionEnter( IInteractableObject interactionObject )
	{
		if( interactionObjects.ContainsKey( interactionObject ) )
            return;
        
        GameObject promtGO = Instantiate(Resources.Load<GameObject>("buttonPrompt")) as GameObject;
        ButtonPrompt buttonPrompt = promtGO.GetComponent<ButtonPrompt>(); 
        buttonPrompt.SetText ("");
        buttonPrompt.SetConnectedTransform (interactionObject.transform);


        interactionObjects.Add(interactionObject, buttonPrompt);
        interactionObject.onInteractionDestroyed += OnInteractionDestroyed;
        interactionObject.onInteractionDisabled += OnInteractionDisabled;
	}

    public void OnInteractionExit( IInteractableObject interactionObject)
	{
        if( CurrentObject == interactionObject )
            DeselectCurrentObject(CurrentObject);
        
		if( !interactionObjects.ContainsKey( interactionObject ) )
            return;
        
        Destroy(interactionObjects[interactionObject].gameObject);
        interactionObject.onInteractionDestroyed -= OnInteractionDestroyed;
        interactionObject.onInteractionDisabled -= OnInteractionDisabled;
		interactionObjects.Remove( interactionObject );	
    }

    void Start()
    {
        // search for all the interactions in the scene
        m_sceneInteractions = new List<IInteractableObject>(FindObjectsOfType<IInteractableObject>());
    }
       
	void Update () 
	{
        // iterate over all the interactions,
        // checking on distance
        // adding to the active list if close

        for(int i = 0; i < m_sceneInteractions.Count; i++)
        {
            IInteractableObject interaction = m_sceneInteractions[i];
                
            bool isEligable = 
                interaction.IsInteracting || interaction.ActiveWhen == IInteractableObject.WorkState.WorksAlways;

            if( interaction.ActiveWhen != IInteractableObject.WorkState.WorksAlways )
                isEligable = true && !LightStatesMachine.Instance.IsLightOn();

            bool closeInteraction = isEligable && IsCharCloserThan(interaction, 1.5f);


            if(closeInteraction)
            {
                OnInteractionEnter(interaction);
            }

            if(!closeInteraction)
            {
                OnInteractionExit(interaction);
            }
        }


		// here should be descibed 3 cases:
		// 1. start interaction
		// 2. stop interaction
		// 3. interaction ends on itself


		// select current interaction object going through the list
		float closestDistToCenter = Mathf.Infinity;
        IInteractableObject closestInteraction = null;

		//for( int i = 0; i < interactionObjects.Count; i++ )
        foreach(var interactionPair in interactionObjects)
		{
            IInteractableObject iObject = interactionPair.Key;
            ButtonPrompt promtObject = interactionPair.Value;

            //if (!iObject.isVisible())
            //    continue;

            bool showText = LightStatesMachine.Instance.IsLightOn() && !iObject.IsInteracting;
            if(iObject is DoorInteraction)
                showText = true;

            UpdatePromtButton(showText, iObject, promtObject);

            Vector3 viewPos = Camera.main.WorldToViewportPoint( iObject.transform.position );
			float distance = Vector2.Distance( viewPos, Vector2.one * 0.5f );
            if( distance < closestDistToCenter )
			{
				closestDistToCenter = distance;
                closestInteraction = iObject;
			}
		}

        if( closestInteraction == null )
			return;

		// switching to the new interactable object
        if(CurrentObject != closestInteraction)
		{
			if(CurrentObject != null)
			{
				DeselectCurrentObject(CurrentObject);
			}

            SelectCurrentObject(closestInteraction);
		}

		if(!IsInteracting && Input.GetMouseButtonDown(0))
		{
			IsInteracting = false;

			IInteractableObject[] interactableInterfaces = CurrentObject.GetComponents<IInteractableObject>();
            foreach (IInteractableObject ie in interactableInterfaces)
            {
				IsInteracting = ie.Activate() || IsInteracting;
            }
			
		}

		if(IsInteracting && Input.GetMouseButtonDown(1))
		{
			IInteractableObject[] interactableInterfaces = CurrentObject.GetComponents<IInteractableObject>();
			foreach (IInteractableObject ie in interactableInterfaces)
			{
				ie.Activate();
				IsInteracting = false;
			}
		}
	}

    public bool IsCharCloserThan(IInteractableObject obj, float dist)
    {
        return (transform.position - obj.transform.position).magnitude < dist;
    }

    private void UpdatePromtButton(bool showText, IInteractableObject iObject, ButtonPrompt promtObject)
    {
        if(IsInteracting)
            showText = false;

        string textToOutput = iObject.IsSelected ? iObject.ActionsToDisplay : iObject.TextToDisplay;       
        promtObject.SetText (showText ? textToOutput : "");
        promtObject.setInteractableUI(iObject.IsSelected && !IsInteracting);

        Vector3 direction = ((iObject.transform.position - Vector3.up * 1.5f) - Camera.main.transform.position).normalized;
        promtObject.transform.position = iObject.transform.position - direction * 0.25f;
        promtObject.transform.LookAt(Camera.main.gameObject.transform);
    }

    private void SelectCurrentObject(IInteractableObject obj)
    {
        CurrentObject = obj;
        CurrentObject.IsSelected = true;
    }

    private void DeselectCurrentObject(IInteractableObject obj)
    {
        CurrentObject.IsSelected = false;
        CurrentObject = null;
    }

    private void OnInteractionDestroyed(IInteractableObject obj)
    {
        m_sceneInteractions.Remove(obj);
        OnInteractionExit(obj);
    }

    private void OnInteractionDisabled(IInteractableObject obj)
    {
        m_sceneInteractions.Remove(obj);
        OnInteractionExit(obj);
    }
}
