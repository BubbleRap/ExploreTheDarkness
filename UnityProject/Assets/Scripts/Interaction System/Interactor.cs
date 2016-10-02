using UnityEngine;
using System.Collections.Generic;

public class Interactor : MonoBehaviour 
{	
    public OnMouseOver m_backgroundOver;
    public OnMouseClick m_backgroundClick;
    public OnMouseDrag m_backgroundDrag;

    public bool IsInteracting { get; set; }
    public IInteractableObject CurrentObject { get; private set; }


    private Canvas m_canvas;
    private List<IInteractableObject> m_sceneInteractions;
    private Dictionary<IInteractableObject, ButtonPrompt> interactionObjects;


    void Start()
    {
        m_canvas = FindObjectOfType<Canvas>();
        interactionObjects = new Dictionary<IInteractableObject, ButtonPrompt>();

        // search for all the interactions in the scene
        m_sceneInteractions = new List<IInteractableObject>(FindObjectsOfType<IInteractableObject>());
    }

    void Update () 
    {
        CheckForCloseInteractions();
        CheckCurrentSelection();
        CheckInteractionsInput();
    }
       
    private void CheckForCloseInteractions()
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
    }

    private void CheckCurrentSelection()
    {
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

            //if (!promtObject.IsVisible)
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
    }

    private void CheckInteractionsInput()
    {
        // this should be checking all interactions attached to a single game object,
        // but lets imagine there is one one at a time

        if(!IsInteracting && Input.GetMouseButtonDown(0))
        {
            IsInteracting = false;
            IsInteracting = CurrentObject.Activate() || IsInteracting;
        }

        if(IsInteracting && Input.GetMouseButtonDown(1))
        {
            IsInteracting = false;
            CurrentObject.Activate();
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
    }

    private void OnInteractionEnter( IInteractableObject interactionObject )
    {
        if( interactionObjects.ContainsKey( interactionObject ) )
            return;

        GameObject promtPrefab = UIManager.Instance.m_buttonPromtPrefab;
        GameObject promtGO = Instantiate(promtPrefab) as GameObject;
        ButtonPrompt buttonPrompt = promtGO.GetComponent<ButtonPrompt>(); 
        buttonPrompt.SetText ("");
        buttonPrompt.SetConnectedTransform (interactionObject.transform);

        // setting the object under Canvas
        promtGO.transform.SetParent(UIManager.s_canvas.transform);
        promtGO.transform.localScale = Vector3.one;


        interactionObjects.Add(interactionObject, buttonPrompt);
        interactionObject.onInteractionDestroyed += OnInteractionDestroyed;
        interactionObject.onInteractionDisabled += OnInteractionDisabled;
    }

    private void OnInteractionExit( IInteractableObject interactionObject)
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
