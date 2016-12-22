using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityStandardAssets.CinematicEffects;

public class LookInDetailInteraction : IInteractableObject 
{
    [Serializable]
    public class InteractionComponent
    {
        public Collider collider;
        public UnityEvent onInteract;
    }

    [Serializable]
    public class ObjectMaterialContainer
    {
        public Renderer renderer;
        public Material materialDefault;
        public Material materialInteracting;
    }
       
        
	public float _faceDistance = 0.25f;
    private float _dragSpeed = 10f;
    private float m_acceleration = 10f;

    public InteractionComponent[] m_interactiveComponents;
    public ObjectMaterialContainer[] m_materialsToChange;


	private SiljaBehaviour _siljaBeh;

	private Vector3 _originalPos;
	private Quaternion _originalRot;

	private Collider _collider;
   

    private Vector2 m_velocity;
    private Vector2 m_targetVelocity;

    private Animator m_animator;
    private bool m_horizontalDrag = false;
	private bool m_isDragging = false;

	private bool m_isMouseOver = false;
	private bool m_isMouseOverInteraction = false;

	private Texture2D cursorDrag;
	private Texture2D cursorDraging;
	private Texture2D cursorClick;
	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = new Vector2(30,40);

	void Awake()
	{
        _originalPos = transform.localPosition;
        _originalRot = transform.localRotation;

        base.Awake();

		_collider = GetComponent<Collider>();
       

        m_animator = GetComponentInParent<Animator>();

        foreach( InteractionComponent com in m_interactiveComponents )
        {
            // caching lambda
            InteractionComponent currentComponent = com;

            OnMouseClick onClick = com.collider.gameObject.AddComponent<OnMouseClick>();
            OnMouseOver onOver = com.collider.gameObject.AddComponent<OnMouseOver>();
            OnMouseDrag onDrag = gameObject.AddComponent<OnMouseDrag>();

            onDrag.onMouseDragBegin += OnComponentDragBegin;
            onDrag.onMouseDrag += OnComponentDragged;
            onDrag.onMouseDragEnd += OnComponentDragEnd;

            onClick.onMouseClick += (PointerEventData eventData) =>
            {
                if(eventData.dragging)
                    return;

                if(!IsInteracting )
                    return;

                currentComponent.onInteract.Invoke();
            };
                
            onOver.onMouseOver += (PointerEventData eventData) => 
            {
                if( !IsInteracting )
                    return;

                m_isMouseOverInteraction = false;

                if(currentComponent.onInteract.GetPersistentEventCount() > 0)
                {
                    m_isMouseOverInteraction = true;
                }

                if(!m_isDragging)
                    Cursor.SetCursor(cursorDrag, hotSpot, cursorMode);

                if(m_isMouseOverInteraction && !m_isDragging)
                    Cursor.SetCursor(cursorClick, hotSpot, cursorMode);

                m_isMouseOver = true;
            };

            onOver.onMouseOut += (PointerEventData data) =>
            {
                if(!IsInteracting )
                    return;

                if(!m_isDragging)
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);

                m_isMouseOver = false;
            };
        }
	}

    private void Update()
    {
		if(cursorDrag == null)
		{
			cursorDrag = UIManager.Instance.m_cursorDrag;
			cursorDraging = UIManager.Instance.m_cursorDragging;
			cursorClick = UIManager.Instance.m_cursorClick;
		}

        if (IsInteracting)
        {
            // horizontal movement
            m_velocity.x = Mathf.Lerp(m_velocity.x, m_targetVelocity.x, m_acceleration * Time.deltaTime);
            transform.Rotate(m_velocity.x * Camera.main.transform.up * Time.deltaTime, Space.World);

            // vertical movement
            m_velocity.y = Mathf.Lerp(m_velocity.y, m_targetVelocity.y, m_acceleration * Time.deltaTime);
            transform.Rotate(m_velocity.y * -transform.right * Time.deltaTime, Space.World);
        }
    }

    private void LateUpdate()
    {
        m_targetVelocity = Vector3.zero;
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

        base.Activate();
       

        if(IsInteracting)
		{
			OnInvestigateEnabled();

			ObjectivesManager.Instance.OnInteractionComplete( this, true );

			m_onInteractionActivated.Invoke();
		}
		else
		{
			OnInvestigateDisabled();
			UIManager.Instance.hideCursor();
			UIManager.Instance.lookAtUI(false);
		}

        return IsInteracting;
	}

	private void OnInvestigateEnabled()
	{
        _siljaBeh.interactor.m_backgroundDrag.onMouseDragBegin += OnComponentDragBegin;
        _siljaBeh.interactor.m_backgroundDrag.onMouseDrag += OnComponentDragged;
        _siljaBeh.interactor.m_backgroundDrag.onMouseDragEnd += OnComponentDragEnd;

        CameraTransitioner transitioner = _siljaBeh.camTransitioner;
        CameraFollow camControl = _siljaBeh.cameraFollow;

		_siljaBeh.IsMoveEnabled = false;
			
		transitioner.AddFPPCompleteAction( () =>
		{
			camControl.CamControlType = CameraFollow.CameraControlType.CCT_Overwritten;

			UIManager.Instance.showCursor();
			UIManager.Instance.lookAtUI(true);

			Transform fpCamTransform = transitioner.FPPCameraTransform;
			transform.position = fpCamTransform.TransformPoint(Vector3.forward * _faceDistance);


			transform.LookAt(fpCamTransform);

			if( _collider != null )
				_collider.enabled = false;

            // change materials
            for(int i = 0; i < m_materialsToChange.Length; i++)
                m_materialsToChange[i].renderer.sharedMaterial = m_materialsToChange[i].materialInteracting;

		});
			
		_siljaBeh.ShiftToFirstPerson();
	}

	private void OnInvestigateDisabled()
    {
        _siljaBeh.interactor.m_backgroundDrag.onMouseDragBegin -= OnComponentDragBegin;
        _siljaBeh.interactor.m_backgroundDrag.onMouseDrag -= OnComponentDragged;
        _siljaBeh.interactor.m_backgroundDrag.onMouseDragEnd -= OnComponentDragEnd;

        CameraTransitioner transitioner = _siljaBeh.camTransitioner;
        CameraFollow camControl = _siljaBeh.cameraFollow;


        UnityAction finishAction = () =>
        {   
            // setting up the character motion state

            _siljaBeh.IsMoveEnabled = true;

            if( _collider != null )
                _collider.enabled = true;
        };

        // change materials
        for(int i = 0; i < m_materialsToChange.Length; i++)
            m_materialsToChange[i].renderer.sharedMaterial = m_materialsToChange[i].materialDefault;

        if (LightStatesMachine.Instance.IsLightOn())
            transitioner.AddTPPCompleteAction(finishAction);
        else
            transitioner.AddFPPCompleteAction(finishAction);
		
		// reversing the previous object's transform
		transform.localPosition = _originalPos;
        transform.localRotation = _originalRot;
		
		if (LightStatesMachine.Instance.IsLightOn())
		    _siljaBeh.ShiftToThirdPerson();
        else
            _siljaBeh.ShiftFirstToFirstPerson();

		Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    private void OnComponentDragBegin(PointerEventData data)
    {
        if( !IsInteracting )
            return;  

        m_horizontalDrag = Mathf.Abs(data.delta.x) > Mathf.Abs(data.delta.y);
        m_isDragging = true;

		Cursor.SetCursor(cursorDraging, hotSpot, cursorMode);
    }

    private void OnComponentDragged(PointerEventData data)
    {
        if( !IsInteracting )
            return;

        if(m_horizontalDrag)
            m_targetVelocity.x =  -data.delta.x * _dragSpeed;
        else
            m_targetVelocity.y = data.delta.y * _dragSpeed;
    }

	private void OnComponentDragEnd(PointerEventData eventData)
    {
        if( !IsInteracting )
            return;

		m_isDragging = false;

		if(m_isMouseOver && !m_isMouseOverInteraction)
		{
			Cursor.SetCursor(cursorDrag, hotSpot, cursorMode);
		}
		else if(m_isMouseOver && m_isMouseOverInteraction)
		{
			Cursor.SetCursor(cursorClick, hotSpot, cursorMode);
		}
		else
		{
			Cursor.SetCursor(null, Vector2.zero, cursorMode);
		}
    }
}
