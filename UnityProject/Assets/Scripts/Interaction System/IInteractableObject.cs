using UnityEngine;
using System.Collections;

using UnityEngine.Events;
/// <summary>
/// TODO: Add states, for uncomplete, complete etc;
/// Integrate Promt script
/// Render text concerning what objecting is being completed
/// </summary>

//[RequireComponent(typeof(PromtButtonInteractionObject))]
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

//	[HideInInspector]
//	public UnityEvent m_onInteractionComplete = new UnityEvent();


//	public delegate void CompletionCallback( bool state );
//	public event CompletionCallback m_onInteractionComplete;
//
//	public void CallbackCompletion(bool state)
//	{
//		if( m_onInteractionComplete != null )
//			m_onInteractionComplete.Invoke( state );
//	}


//	protected bool m_isCompleted = false;
//	public bool IsCompleted
//	{
//		get 
//		{ 
//			return m_isCompleted; 
//		}
//		set 
//		{
//			bool prevValue = m_isCompleted;
//			m_isCompleted = value;
//
//			// so if a job was'nt complete before, but it is complete now,
//			// then invoke the function
//			if( m_isCompleted && !prevValue )
//				m_onInteractionComplete.Invoke();
//		}
//	}

	protected bool interactionIsActive = false;
//	protected bool m_isInitialized = false;
	private Renderer m_renderer;

//	public virtual void Initialize() { m_isInitialized = !m_isInitialized; }
	public virtual bool Activate() { return (interactionIsActive = !interactionIsActive); }
	
	public bool disableOnActive = true;
//	public bool interactionWorksInFP = false;
	
	private GameObject buttonPrompt;
	private Interactor interactor;
	
	private bool objectIsClose = false;
	public float distance = 1f;
	
	public string TextToDisplay = "some string";

	private Vector3 m_cameraRelativePosition;

	void Start()
	{
		if (TextToDisplay == "")
			TextToDisplay = gameObject.name;
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
				isEligable = isEligable   && (  (perspectiveMode == WorkState.FirstPersonOnly && SiljaBehaviour.isLookingInFP) 
			                            		||(perspectiveMode == WorkState.ThirdPersonOnly && !SiljaBehaviour.isLookingInFP));
			}
		}

		OnInteractionClose( (interactor.isInteracting  && interactionIsActive)
						|| (isClose
		                && IsVisibleWithin(15f) 
		                && isEligable));

		if( buttonPrompt == null )
		{
			buttonPrompt = Instantiate(Resources.Load<GameObject>("buttonPrompt")) as GameObject;
			buttonPrompt.GetComponent<ButtonPrompt> ().SetText (this.TextToDisplay);
			buttonPrompt.GetComponent<ButtonPrompt> ().SetConnectedTransform (this.transform);
			
			buttonPrompt.SetActive(false);
		}
		
		Vector3 direction = ((transform.position - Vector3.up * 1.5f) - Camera.main.transform.position).normalized;
		buttonPrompt.transform.position = transform.position - direction * 0.25f;
		buttonPrompt.transform.LookAt(Camera.main.gameObject.transform);
	}
	
	public void ActivatePromtButton( bool state )
	{
		buttonPrompt.SetActive(state && (!interactionIsActive || !disableOnActive));
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
	
	public void OnInteractionClose( bool state )
	{
		if( objectIsClose == state )
			return;
		
		if( state )
		{
			interactor.OnInteractionEnter( gameObject );
		}
		else
		{
			interactor.OnInteractionExit( gameObject );
		}
		
		objectIsClose = state;
	}
	
	void OnDestroy()
	{
		Destroy(buttonPrompt);
		buttonPrompt = null;
		Resources.UnloadUnusedAssets();
	}

	// Exposed for inspector purposes
	public void ActivateInteraction()
	{
		Activate();
	}
}
