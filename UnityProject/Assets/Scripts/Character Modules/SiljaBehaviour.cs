﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SiljaBehaviour : CharacterBehaviour, IInput
{
	public Transform 			firstPersonRig;
	public Animator 			siljaAnimation;
    public GameObject           thisCamera;

   
    [HideInInspector]
    public Interactor 			interactor;
    [HideInInspector]
    public FlashlightController flshCtrl;

	[HideInInspector]
	public CameraTransitioner	camTransitioner;
	[HideInInspector]
	public CameraFollow 		cameraFollow;

    public float ShiftDuration = 1f;
	private bool m_isLookingInFP = false;

    public float m_normalDischargeSpeed = 1f;
    public float m_scaredDischargeSpeed = 6f;
	
	private bool isFlashLightCollected = true;
	public bool FlashLightCollected
	{
		get{ return isFlashLightCollected; }
		set{ isFlashLightCollected = value; }
	}
        

    private bool m_haveSeenRecently = false;
	private bool m_isScared = false;
	public bool IsScared 
	{ 
		get { return m_isScared; } 
	}

	public bool IsSeenRecently 
	{ 
		get { return m_haveSeenRecently; } 
	}

    public bool IsFirstPerson
    {
        get {  return m_isLookingInFP; }
    }

    public void SetScaredState(bool state)
    {
        if (state && !m_haveSeenRecently)
            m_characterAudio.PlayScaredLoop();

        if (state && !m_haveSeenRecently)
            m_characterAudio.PlayTensionLoop();

        if (state && !m_haveSeenRecently)
        {
            m_haveSeenRecently = true;
            Invoke("CleanSeenRecentlyFlag", 10f);
        }

        m_isScared = state;
    }

	public void CleanSeenRecentlyFlag()
	{
		m_haveSeenRecently = false;
	}

	public bool IsMoveEnabled 
	{
        get { return m_movementController.CanMove; }
		set { m_movementController.EnableMoving(value); }
	}

	void Awake()
	{
        base.Awake();

        m_movementController.InitializeCharacterController(45f, 0.3f, 0.01f, 0.25f, 1.76f);
        m_movementController.InitializeCharacterMotor(false, 1f, 1f, 1f, false, false, false);


        interactor = gameObject.GetComponent<Interactor>();
        gameObject.AddComponent<SiljaShakeOnScary>();

        camTransitioner = thisCamera.GetComponent<CameraTransitioner>();
		cameraFollow = thisCamera.GetComponent<CameraFollow>();
        flshCtrl = gameObject.GetComponent<FlashlightController>();

		firstPersonRig.gameObject.SetActive(false);
	}
	

	void Start()
	{
		EnableThirdPerson();
	}
        
	void Update () 
	{
        UpdateInput();
        UpdateFlashlight();
        UpdateAudio();
    }

    private void UpdateInput()
    {
        UpdateMovementInput();


        if (Input.GetKeyUp(KeyCode.Q)
        && camTransitioner.Mode != CameraTransitioner.CameraMode.Transitioning
        && !interactor.isInteracting)
        {
            if (m_isLookingInFP)
            {
                m_isLookingInFP = false;
                ShiftToThirdPerson();
                EnableFlashlight(false);
            }
            else
            {
                m_isLookingInFP = true;
                ShiftToFirstPerson();
                EnableFlashlight(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.F) && m_isLookingInFP)
        {
            EnableFlashlight(!flshCtrl.IsEnabled);
        }
         
    }

    private void UpdateFlashlight()
    {
        float dischargeSpeed = flshCtrl.IsDischarging && (!m_isLookingInFP || m_isScared) ? m_scaredDischargeSpeed : m_normalDischargeSpeed;
        flshCtrl.SetDischargeSpeed(dischargeSpeed);
    }

    private void UpdateAudio()
    {
        m_characterAudio.PlayHeartbeatLoop();
        m_characterAudio.SetHeartbeatVolume(2.0f - (flshCtrl.ChargeLeft * 2 / flshCtrl.m_batteryLife));

        m_characterAudio.PlayBreathingLoop();
        m_characterAudio.SetBreathingVolume(1.0f - Mathf.Pow(flshCtrl.ChargeLeft / flshCtrl.m_batteryLife, 0.4f));
    }


	public void ShiftToFirstPerson()
	{
		cameraFollow.CamControlType = CameraFollow.CameraControlType.CCT_FPSLook;	
		camTransitioner.TransitionTPPtoFPP();
		Invoke("EnableFirstPerson",ShiftDuration);
	}

	public void EnableFirstPerson() {

        flshCtrl.EnableFlashlight(true);		

        m_movementController.SetMaxWalkingSpeed(1.5f, 1.5f);

	}

	public void ShiftToThirdPerson()
	{
		camTransitioner.TransitionFPPtoTPP();

		Invoke("EnableThirdPerson",ShiftDuration);
	}
	
	public void EnableThirdPerson() {

        flshCtrl.EnableFlashlight(false);

        m_movementController.SetMaxWalkingSpeed(1.1f, 0.9f);

		cameraFollow.CamControlType = CameraFollow.CameraControlType.CCT_Default;
	}

	public void EnableFlashlight(bool state)
	{
		if( !isFlashLightCollected )
			return;

        flshCtrl.EnableFlashlight(state);

        m_characterAudio.PlayFlashlightSound();
		firstPersonRig.gameObject.SetActive(state);
	}

	public IEnumerator BlinkingEffect()
	{
		// keep in mind, that fader takes the time, you specify,
		// which means, you gotta yield the time first, and plus the pause

		Fader.Instance.FadeScreen(false, 2.0f);
		yield return new WaitForSeconds(2.5f);
		Fader.Instance.FadeScreen(true, 1.0f);
		yield return new WaitForSeconds(2f);
		Fader.Instance.FadeScreen(false, 1.0f);
		yield return new WaitForSeconds(1.5f);
		Fader.Instance.FadeScreen(true, 0.5f);
		yield return new WaitForSeconds(0.5f);
		Fader.Instance.FadeScreen(false, 0.25f);
	}

    public void ResetCharacter()
    {
        m_movementController.EnableMoving(true);

        flshCtrl.RechargeFlashlight();

        EnableFirstPerson();
        StartCoroutine(BlinkingEffect());

    }

    private void UpdateMovementInput()
    {
        Vector3 forward = thisCamera.transform.forward;

        forward.y = 0;
        forward.Normalize();


        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 moveDir = (h * right + v * forward).normalized;


        if( moveDir.magnitude > 0f )
            RotateCharacterTowards(forward);
        MoveCharacterTowards(moveDir, new Vector2(h, v));
    }

    public void PlaySiljaCaughtRandomSound()
    {
        m_characterAudio.PlaySiljaCaughtRandomSound();
    }

    public void SetHeartbeatVolume(float volume)
    {
        m_characterAudio.SetHeartbeatVolume(volume);
    }
}
