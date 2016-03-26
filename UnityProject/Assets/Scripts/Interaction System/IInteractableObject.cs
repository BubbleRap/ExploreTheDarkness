using UnityEngine;
using System.Collections;

using UnityEngine.Events;

public class IInteractableObject : MonoBehaviour 
{
	public enum WorkState
	{
		WorksAlways,
		DarkModeOnly,
		LightModeOnly,
		Never
	}

	public WorkState ActiveWhen;
	
    public bool interactionIsActive = false;
	private Renderer m_renderer;
	
	public virtual bool Activate() { return (interactionIsActive = !interactionIsActive); }
	
	public bool disableOnActive = true;

    private SiljaBehaviour m_character;
	private Interactor interactor;

	//private bool objectIsFar = false;
	private bool objectIsClose = false;
	//public float distance = 1f;
	
	public string TextToDisplay = "some string";
	public string ActionsToDisplay = "Look";

	private Vector3 m_cameraRelativePosition;

	public UnityEvent m_onInteractionActivated = new UnityEvent();

    protected CharacterBehaviour m_interactingBehaviour;

    private ButtonPrompt buttonPrompt;

    protected void Awake()
    {
        GameObject promtGO = Instantiate(Resources.Load<GameObject>("buttonPrompt")) as GameObject;
        promtGO.transform.parent = transform;

        buttonPrompt = promtGO.GetComponent<ButtonPrompt>();

        buttonPrompt.SetText ("default");
        buttonPrompt.SetConnectedTransform (this.transform);

        //buttonPrompt.gameObject.SetActive(false);
    }

    protected void Start()
	{     
        m_character = DarknessManager.Instance.m_mainCharacter;

		if (TextToDisplay == "")
			TextToDisplay = gameObject.name;

		if (ActionsToDisplay == "")
			ActionsToDisplay = gameObject.name;
    }
	
	protected void Update()
	{
		if( interactor == null )
			interactor = FindObjectOfType(typeof(Interactor)) as Interactor;
		if( m_renderer == null )
			m_renderer = GetComponent<Renderer>();

		m_cameraRelativePosition = Camera.main.transform.InverseTransformPoint(transform.position);

		bool isEligable = interactionIsActive || ActiveWhen == WorkState.WorksAlways;

		if( ActiveWhen != WorkState.WorksAlways )
		{
			if( ActiveWhen == WorkState.Never )
			{
				isEligable = false;
			}
			else
			{
				isEligable = true   && (  (ActiveWhen == WorkState.DarkModeOnly && !LightStatesMachine.Instance.IsLightOn()) 
			                            		||(ActiveWhen == WorkState.LightModeOnly && !LightStatesMachine.Instance.IsLightOn()));
			}
		}
		
        string textToOutput = "";
        bool closeInteraction = interactor.IsInteracting  && interactionIsActive;

        if(isEligable)
        {
            // close object
            if( IsCharCloserThan(2f) && IsVisibleWithin(3f * m_cameraRelativePosition.magnitude))
            {
                textToOutput = ActionsToDisplay;
                closeInteraction = true;
            }
            // far object
            else if(IsCharCloserThan(5f) && IsVisibleWithin(90f))
            {
                textToOutput = TextToDisplay;
            }
        }
        // use null string for nothing
        buttonPrompt.SetText (textToOutput);

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

	public void changePromt(string action)
	{
		ActionsToDisplay = action;
		buttonPrompt.SetText (ActionsToDisplay);
	}
	
	public bool IsPromtActivated(){
        return buttonPrompt.gameObject.activeInHierarchy;
	}

	public bool IsInteracting
	{
		get { return interactionIsActive;}
	}

	public bool IsVisibleWithin(float angle)
	{
		// normalized angle view, where 1 is the field of view angle
		float angleFraction = angle / Camera.main.fieldOfView * Camera.main.aspect / 1.78f;

		bool isVisible = m_renderer == null || m_renderer.isVisible;
		return m_cameraRelativePosition.x < angleFraction * m_cameraRelativePosition.z 
			&& m_cameraRelativePosition.x > -angleFraction * m_cameraRelativePosition.z
			&& isVisible;
			//&& m_cameraRelativePosition.z < 5f;
	}

	public bool IsCamCloserThan( float dist )
	{
		return m_cameraRelativePosition.magnitude < dist;
	}

    public bool IsCharCloserThan(float dist)
    {
        return (m_character.transform.position - transform.position).magnitude < dist;
    }

	public bool isObjectClose()
	{
		return objectIsClose;
	}
	
	public void OnInteractionClose( bool state )
	{
		if( objectIsClose == state)
			return;
		
		if( state )
		{
            if(interactor.CloseInteractionsCount > 0)
			{
				objectIsClose = !state;
				return;
			}
			
            buttonPrompt.SetText (ActionsToDisplay);

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

    private void OnTriggerEnter(Collider other)
    {
        m_interactingBehaviour = other.gameObject.GetComponent<CharacterBehaviour>();
    }

    private void OnTriggerExit(Collider other)
    {
        m_interactingBehaviour = null;
    }
}
