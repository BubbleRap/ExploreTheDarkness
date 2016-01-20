using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SiljaBehaviour : MonoBehaviour 
{
	public Transform 			firstPersonRig;
	public Animator 			siljaAnimation;
    public GameObject           thisCamera;

    [HideInInspector]
    public MovementController 	moveCtrl;
    [HideInInspector]
    public Interactor 			interactor;
    [HideInInspector]
    public CharacterAudio      charAudio;
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
            charAudio.PlayScaredLoop();

        if (state && !m_haveSeenRecently)
            charAudio.PlayTensionLoop();

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
		get { return moveCtrl.enabled; }
		set 
		{
            moveCtrl.EnableMoving(value);
		}
	}

	void Awake()
	{
        CharacterAnimation charAnimation = siljaAnimation.gameObject.GetComponent<CharacterAnimation>();

        moveCtrl = gameObject.AddComponent<MovementController>();
        moveCtrl.Initialize(thisCamera.transform, charAnimation);

        interactor = gameObject.GetComponent<Interactor>();
        gameObject.AddComponent<SiljaShakeOnScary>();
        charAudio = siljaAnimation.gameObject.GetComponent<CharacterAudio>();

        camTransitioner = thisCamera.GetComponent<CameraTransitioner>();

		cameraFollow = thisCamera.GetComponent<CameraFollow>();
        cameraFollow.Initialize(charAnimation);

        flshCtrl = gameObject.GetComponent<FlashlightController>();

		firstPersonRig.gameObject.SetActive(false);
	}
	

	void Start()
	{
		EnableThirdPerson();
	}

		void OnGUI()
		{
				GUILayout.Label(flshCtrl.ChargeLeft.ToString("0.0"));
		}

	void Update () 
	{
        UpdateInput();
        UpdateFlashlight();
        UpdateAudio();
    }

    private void UpdateInput()
    {
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
        charAudio.PlayHeartbeatLoop();
        charAudio.SetHeartbeatVolume(2.0f - (flshCtrl.ChargeLeft * 2 / flshCtrl.m_batteryLife));

        charAudio.PlayBreathingLoop();
        charAudio.SetBreathingVolume(1.0f - Mathf.Pow(flshCtrl.ChargeLeft / flshCtrl.m_batteryLife, 0.4f));
    }


	public void ShiftToFirstPerson()
	{
		cameraFollow.CamControlType = CameraFollow.CameraControlType.CCT_FPSLook;	
		camTransitioner.TransitionTPPtoFPP();
		Invoke("EnableFirstPerson",ShiftDuration);
	}

	public void EnableFirstPerson() {

        flshCtrl.EnableFlashlight(true);
		

        moveCtrl.SetWalkingSpeed(1.5f, 1.5f);

	}

	public void ShiftToThirdPerson()
	{
		camTransitioner.TransitionFPPtoTPP();

		Invoke("EnableThirdPerson",ShiftDuration);
	}
	
	public void EnableThirdPerson() {

        flshCtrl.EnableFlashlight(false);

        moveCtrl.SetWalkingSpeed(1.1f, 0.9f);

		cameraFollow.CamControlType = CameraFollow.CameraControlType.CCT_Default;
	}

	public void EnableFlashlight(bool state)
	{
		if( !isFlashLightCollected )
			return;

        flshCtrl.EnableFlashlight(state);

        charAudio.PlayFlashlightSound();
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
        moveCtrl.EnableMoving(true);

        flshCtrl.RechargeFlashlight();

        EnableFirstPerson();
        StartCoroutine(BlinkingEffect());

    }
}
