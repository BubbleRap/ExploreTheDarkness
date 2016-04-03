using System;
using UnityEngine;
using System.Collections;

using UnityStandardAssets.ImageEffects;

using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LookInDetailInteraction : IInteractableObject 
{
    [Serializable]
    public class InteractionComponent
    {
        public Collider collider;
        public UnityEvent onInteract;
    }

	public enum ObjectOrientation
	{
		Y_up,
		Z_up
	}

	public ObjectOrientation orientation = ObjectOrientation.Y_up;

	public float _faceDistance = 0.25f;
    public float _inertionDamp = 0.5f;
    public float _maxDelta = 0.15f;
    public float _dragSpeed = 3f;

    public InteractionComponent[] m_interactiveComponents;

	private SiljaBehaviour _siljaBeh;

	private Vector3 _originalPos;
	private Quaternion _originalRot;

	private Collider _collider;

	private DepthOfField _dof;
    private Vector2 m_inertion;

    private Animator m_animator;
    private bool m_horizontalDrag = false;
    private Vector3 m_currentRotationAxis = Vector3.up;

	void Awake()
	{
        base.Awake();

		_originalPos = transform.position;
		_originalRot = transform.rotation;

		_collider = GetComponent<Collider>();

		_dof = Camera.main.GetComponent<DepthOfField>();

        m_animator = GetComponentInParent<Animator>();

        foreach( InteractionComponent com in m_interactiveComponents )
        {
            OnMouseClick onClick = com.collider.gameObject.AddComponent<OnMouseClick>();
            onClick.onMouseClick += OnComponentClicked;

            OnMouseDrag onDrag = com.collider.gameObject.AddComponent<OnMouseDrag>();
            onDrag.onMouseDragBegin += OnComponentDragBegin;
            onDrag.onMouseDrag += OnComponentDragged;
            onDrag.onMouseDragEnd += OnComponentDragEnd;
        }
	}

    private new void Update()
    {
        base.Update();

        m_inertion = Vector2.MoveTowards(m_inertion, Vector2.zero, _inertionDamp * Time.deltaTime);

        transform.Rotate(m_inertion.x * Camera.main.transform.up * _dragSpeed * 1000f * Time.deltaTime, Space.World);
        transform.Rotate(m_inertion.y * Camera.main.transform.right * _dragSpeed * 1000f * Time.deltaTime, Space.World);
    }

	public override bool Activate()
	{
		if( _siljaBeh == null )
		{
			GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
			_siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();
		}

		if( !ObjectivesManager.Instance.IsInteractionEligable( this ) )
			return false;

		interactionIsActive = !interactionIsActive;


		if( interactionIsActive )
		{
			OnInvestigateEnabled();

			ObjectivesManager.Instance.OnInteractionComplete( this, true );

			m_onInteractionActivated.Invoke();
		}
		else
		{
			OnInvestigateDisabled();
		}

		return interactionIsActive;
	}

	private void OnInvestigateEnabled()
	{
		CameraTransitioner transitioner = _siljaBeh.thisCamera.GetComponent<CameraTransitioner>();
		CameraFollow camControl = _siljaBeh.thisCamera.GetComponent<CameraFollow>();


		_siljaBeh.IsMoveEnabled = false;

		//_dof.enabled = true;
			
		transitioner.AddFPPCompleteAction( () =>
		{
			camControl.CamControlType = CameraFollow.CameraControlType.CCT_Overwritten;

			Transform fpCamTransform = transitioner.FPPCameraTransform;
			transform.position = fpCamTransform.TransformPoint(Vector3.forward * _faceDistance);

			_dof.focalLength = _faceDistance;
			_dof.aperture = 50f;

			transform.LookAt(fpCamTransform);

			if( orientation == ObjectOrientation.Z_up )
				transform.Rotate(new Vector3(90f, 0f, 0f));

			if( _collider != null )
				_collider.enabled = false;

		});
		
		
		_siljaBeh.ShiftToFirstPerson();
	}

	private void OnInvestigateDisabled()
	{
		CameraTransitioner transitioner = _siljaBeh.thisCamera.GetComponent<CameraTransitioner>();
		CameraFollow camControl = _siljaBeh.thisCamera.GetComponent<CameraFollow>();

		//_dof.enabled = false;

		transitioner.AddTPPCompleteAction( () =>
		                                  {	
			// setting up the character motion state

			_siljaBeh.IsMoveEnabled = true;
			camControl.CamControlType = CameraFollow.CameraControlType.CCT_Default;
			
			if( _collider != null )
				_collider.enabled = true;
		});
		
		
		// reversing the previous object's transform
		transform.position = _originalPos;
		transform.rotation = _originalRot;
		
		if (LightStatesMachine.Instance.IsLightOn())
		    _siljaBeh.ShiftToThirdPerson();
        else
            _siljaBeh.ShiftToFirstPerson();
    }

    private void OnComponentClicked(Collider collider, PointerEventData eventData)
    {
        if(eventData.dragging)
            return;
        
        if( !interactionIsActive )
            return;

        foreach(InteractionComponent component in m_interactiveComponents)
        {
            if(component.collider == collider)
            {
                component.onInteract.Invoke();
                return;
            }
        }

    }

    private void OnComponentDragBegin(PointerEventData data)
    {
        if( !interactionIsActive )
            return;  


        m_horizontalDrag = Mathf.Abs(data.delta.x) > Mathf.Abs(data.delta.y);
        m_currentRotationAxis = m_horizontalDrag ? Camera.main.transform.up :  Camera.main.transform.right;
    }

    private void OnComponentDragged(PointerEventData data)
    {
        if( !interactionIsActive )
            return;

        if(m_horizontalDrag)
        {
            float horizAmount = -data.delta.x / (float) Screen.width;
            m_inertion.x = Mathf.Clamp(horizAmount, -_maxDelta, _maxDelta);
        }
        else
        {
            float vertAmount = data.delta.y / (float) Screen.height;
            m_inertion.y = Mathf.Clamp(vertAmount, -_maxDelta, _maxDelta);
        }
    }

    private void OnComponentDragEnd(PointerEventData data)
    {
        if( !interactionIsActive )
            return;
    }
}
