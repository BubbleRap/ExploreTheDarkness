using UnityEngine;
using System.Collections;

using UnityEngine.Events;
using System;

public class IInteractableObject : MonoBehaviour 
{
	public enum WorkState
	{
		WorksAlways,
		Switchable
	}

    public WorkState ActiveWhen;
    public string TextToDisplay = "some string";
    public string ActionsToDisplay = "Look";

    public bool IsInteracting { get; set; }
    public bool IsSelected { get; set; }
    public bool IsObjectClose { get; set; }
	
    public Action<IInteractableObject> onInteractionDestroyed = (IInteractableObject) => {};
    public Action<IInteractableObject> onInteractionDisabled = (IInteractableObject) => {};

    public UnityEvent m_onInteractionActivated = new UnityEvent();	

    private Renderer m_renderer;

    protected virtual void Awake()
    {
        m_renderer = GetComponent<Renderer>();    
    }

    protected void Start()
	{     
		if (TextToDisplay == "")
			TextToDisplay = gameObject.name;

		if (ActionsToDisplay == "")
			ActionsToDisplay = gameObject.name;
    }

    public virtual bool Activate() 
    { 
        return IsInteracting = !IsInteracting; 
    }
       
    public bool isVisible()
    {
        return IsInteracting || (LightStatesMachine.Instance.State is LightStateOff);
    }
        
    // Exposed for inspector purposes
    public void ActivateInteraction()
    {
        Activate();
    }

    // Exposed for inspector purposes
    public void DestroyMyself()
    {
        Destroy(this);
    }

    private void OnDisable()
    {
        onInteractionDisabled(this);
    }

    private void OnDestroy()
    {
        onInteractionDestroyed(this);
    }
}
