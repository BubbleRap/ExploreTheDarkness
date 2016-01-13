using UnityEngine;
using System.Collections;

using UnityEngine.Events;

public class IInteractableObject : MonoBehaviour 
{
	public enum WorkState
	{
		WorksAlways,
		FirstPersonOnly,
		ThirdPersonOnly,
		Never
	}

	public WorkState perspectiveMode;
	

	protected bool interactionIsActive = false;
	private Renderer m_renderer;
	
	public virtual bool Activate() { return (interactionIsActive = !interactionIsActive); }
	
	public bool disableOnActive = true;

	private GameObject label;
	private GameObject buttonPrompt;

    private SiljaBehaviour m_character;
	private Interactor interactor;

	private bool objectIsFar = false;
	private bool objectIsClose = false;
	public float distance = 1f;
	
	public string TextToDisplay = "some string";
	public string ActionsToDisplay = "Look";

	private Vector3 m_cameraRelativePosition;

	public UnityEvent m_onInteractionActivated = new UnityEvent();

	void Start()
	{
		if (TextToDisplay == "")
			TextToDisplay = gameObject.name;

		if (ActionsToDisplay == "")
			ActionsToDisplay = gameObject.name;

  //      interactor = DarknessManager.Instance.m_mainCharacter.interactor;
        m_character = DarknessManager.Instance.m_mainCharacter;
    }
	
	protected void Update()
	{
		if( interactor == null )
			interactor = FindObjectOfType(typeof(Interactor)) as Interactor;
		if( m_renderer == null )
			m_renderer = GetComponent<Renderer>();

		m_cameraRelativePosition = Camera.main.transform.InverseTransformPoint(transform.position);

		bool isClose = (transform.position - interactor.transform.position).magnitude < distance;
		bool isEligable = interactionIsActive || perspectiveMode == WorkState.WorksAlways;

		if( perspectiveMode != WorkState.WorksAlways )
		{
			if( perspectiveMode == WorkState.Never )
			{
				isEligable = false;
			}
			else
			{
				isEligable = isEligable   && (  (perspectiveMode == WorkState.FirstPersonOnly && m_character.IsFirstPerson) 
			                            		||(perspectiveMode == WorkState.ThirdPersonOnly && !m_character.IsFirstPerson));
			}
		}

		OnInteractionClose( (interactor.isInteracting  && interactionIsActive)
						|| (isClose
				&& IsVisibleWithin(3f * m_cameraRelativePosition.magnitude)
		                && isEligable));

		OnInteractionFar(IsVisibleWithin(90f) && IsCamCloserThan(5f) && !isObjectClose());

		if( label == null )
		{
			label = Instantiate(Resources.Load<GameObject>("buttonPrompt")) as GameObject;
			label.GetComponent<ButtonPrompt> ().SetText (this.TextToDisplay);
			label.GetComponent<ButtonPrompt> ().SetConnectedTransform (this.transform);
			
			label.SetActive(false);
		}

		if( buttonPrompt == null )
		{
			buttonPrompt = Instantiate(Resources.Load<GameObject>("buttonPrompt")) as GameObject;
			buttonPrompt.GetComponent<ButtonPrompt> ().SetText (this.ActionsToDisplay);
			buttonPrompt.GetComponent<ButtonPrompt> ().SetConnectedTransform (this.transform);

			buttonPrompt.SetActive(false);
		}
		
		Vector3 direction = ((transform.position - Vector3.up * 1.5f) - Camera.main.transform.position).normalized;
		label.transform.position = transform.position - direction * 0.25f;
		label.transform.LookAt(Camera.main.gameObject.transform);
		buttonPrompt.transform.position = transform.position - direction * 0.25f;
		buttonPrompt.transform.LookAt(Camera.main.gameObject.transform);
	}

	public void ActivateLabel( bool state )
	{
		label.SetActive(state && (!interactionIsActive || !disableOnActive));
	}

	public void ActivatePromtButton( bool state )
	{
		buttonPrompt.SetActive(state && (!interactionIsActive || !disableOnActive));
	}

	public void changePromt(string action)
	{
		ActionsToDisplay = action;
		buttonPrompt.GetComponent<ButtonPrompt> ().SetText (this.ActionsToDisplay);
	}
	
	public bool IsActivated(){
		return buttonPrompt != null && buttonPrompt.activeSelf;
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
			if(interactor.interactionObjects.Count > 0)
			{
				objectIsClose = !state;
				return;
			}
					
			interactor.OnInteractionEnter( gameObject );
		}
		else
		{
			interactor.OnInteractionExit( gameObject );
		}
		
		objectIsClose = state;
	}

	public void OnInteractionFar( bool state )
	{
		if( objectIsFar == state )
			return;

		if( state )
		{
			interactor.OnHighlightEnter( gameObject );
		}
		else
		{
			interactor.OnHighlightExit( gameObject );
		}

		objectIsFar = state;
	}
	
	void OnDestroy()
	{
		Destroy(label);
		Destroy(buttonPrompt);
		label = null;
		buttonPrompt = null;
		Resources.UnloadUnusedAssets();
	}

	// Exposed for inspector purposes
	public void ActivateInteraction()
	{
		Activate();
	}
}
