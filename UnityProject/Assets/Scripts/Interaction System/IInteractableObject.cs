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
	
    protected bool interactionIsActive = false;

	private Renderer m_renderer;
	
	public virtual bool Activate() { return (interactionIsActive = !interactionIsActive); }
	
	public bool disableOnActive = true;

    public bool isSelected = false;

	private bool objectIsClose = false;
	
	public string TextToDisplay = "some string";
	public string ActionsToDisplay = "Look";

	private Vector3 m_cameraRelativePosition;

	public UnityEvent m_onInteractionActivated = new UnityEvent();

    private ButtonPrompt buttonPrompt;

    protected virtual void Awake()
    {
        GameObject promtGO = Instantiate(Resources.Load<GameObject>("buttonPrompt")) as GameObject;


        buttonPrompt = promtGO.GetComponent<ButtonPrompt>();

        buttonPrompt.SetText ("");
        buttonPrompt.SetConnectedTransform (this.transform);
    }

    protected void Start()
	{     
		if (TextToDisplay == "")
			TextToDisplay = gameObject.name;

		if (ActionsToDisplay == "")
			ActionsToDisplay = gameObject.name;
    }

    internal bool isVisible()
    {
        return interactionIsActive || 
            LightStatesMachine.Instance.State is LightStateOff ||
            (buttonPrompt.gameObject.activeInHierarchy &&
            buttonPrompt.isVisible);
    }

    protected void Update()
	{
		if( m_renderer == null )
			m_renderer = GetComponent<Renderer>();

		m_cameraRelativePosition = Camera.main.transform.InverseTransformPoint(transform.position);

        bool isEligable = 
            interactionIsActive || ActiveWhen == WorkState.WorksAlways;

		if( ActiveWhen != WorkState.WorksAlways )
            isEligable = true && !LightStatesMachine.Instance.IsLightOn();

        Interactor interactor = DarknessManager.Instance.m_mainCharacter.interactor;
        bool closeInteraction = interactor.IsInteracting  && interactionIsActive;

       
        closeInteraction = isEligable && IsCharCloserThan(1.5f);


        // use null string for nothing
        bool showText = LightStatesMachine.Instance.IsLightOn() && !IsInteracting;
        if((this as DoorInteraction) != null)
            showText = true;

        string textToOutput = isSelected ? ActionsToDisplay : TextToDisplay;       
        buttonPrompt.SetText (showText ? textToOutput : "");

        // interaction logic call
        OnInteractionClose(closeInteraction);
         
		
		Vector3 direction = ((transform.position - Vector3.up * 1.5f) - Camera.main.transform.position).normalized;

		buttonPrompt.transform.position = transform.position - direction * 0.25f;
		buttonPrompt.transform.LookAt(Camera.main.gameObject.transform);
	}

	public void ActivatePromtButton( bool state )
	{
        buttonPrompt.gameObject.SetActive(state && (!interactionIsActive || !disableOnActive)
        );
	}
	
	public bool IsPromtActivated(){
        return buttonPrompt.gameObject.activeInHierarchy;
	}

	public bool IsInteracting
	{
		get { return interactionIsActive;}
	}

	public bool IsCamCloserThan( float dist )
	{
		return m_cameraRelativePosition.magnitude < dist;
	}

    public bool IsCharCloserThan(float dist)
    {
        SiljaBehaviour character = DarknessManager.Instance.m_mainCharacter;
        return (character.transform.position - transform.position).magnitude < dist;
    }

	public bool isObjectClose()
	{
		return objectIsClose;
	}
	
	public void OnInteractionClose( bool state )
	{
		if( objectIsClose == state)
			return;
		
        Interactor interactor = DarknessManager.Instance.m_mainCharacter.interactor;

		if( state )
		{
			interactor.OnInteractionEnter( this );
		}
		else
		{
			interactor.OnInteractionExit( this );
		}
		
		objectIsClose = state;
	}

	// Exposed for inspector purposes
	public void ActivateInteraction()
	{
		Activate();
	}
}
