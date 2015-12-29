﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SiljaBehaviour : MonoBehaviour 
{
	public Transform 			firstPersonRig;
	public Animator 			siljaAnimation;
    public GameObject           thisCamera;

    private MovementController 	moveCtrl;
	private Interactor 			interactor;

	[HideInInspector]
	public CameraTransitioner	camTransitioner;
	[HideInInspector]
	public CameraFollow 		cameraFollow;



	public Light 				teddyLight = null;
	public Light 				teddyLightFlash = null;
	public Light 				teddyLightFlash2 = null;

	private bool inDarkness = false;
	private bool thirdPersonInDark = false;

	private bool DarknessApproaching = false;
	private float DarknessApproachingTimer;
	public float LillebrorLightLifetime = 40.0f;
	public float TimeInDarknessTotal = 10.0f;
	public float darknessIntensifier = 10.0f;
	public float rechargeSpeed = 5.0f;


	public AudioSource heartBeatAudioSource;
	public AudioClip heartBeatClip;
	public AudioSource breathingAudioSource;
	public AudioClip breathingClip;
	public AudioSource monsterAudioSource;
	public AudioClip monsterSearching;
	public AudioClip monsterChase;

	public AudioSource siljaOnScaredAudio;
	public AudioSource audioRunTension;
	public AudioSource flashlightTurnAudio;

	private float volume = 1.0f;
	private float volume2 = 1.0f;
	private float volume3 = 1.0f;

	private float lightFlickerInterval = 0.0f;
	private float lightFlickerInterval2 = 0.0f;
	private bool isCaught = false;
	private float caughtTimer = 0.0f;
	public float captureTimeTotal = 4.0f;
	public AudioClip[] monsterCatchSiljaSound;
	public float blackscreenTime = 5.0f;
	
	static public bool isLookingInFP = false;
	static public bool isFlashlightEnabled = false;
	
	public bool isFlashLightCollected = false;
	public bool FlashLightCollected
	{
		get{ return isFlashLightCollected; }
		set{ isFlashLightCollected = value; }
	}

	[Range(0f, 0.020f)]
	public float fadingOutSpeed = 0.002f;

	[Range(0f, 0.020f)]
	public float fadingInSpeed = 0.012f;

	[Range(0f, 2.50f)]
	public float maximumIntensity = 1.75f;

	[Range(0f, 1.00f)]
	public float mimimumIntensity = 0.5f;

	private float flickerIntervalTimer = 0;
	private float flickerTime = 0;

	[Range(0f, 1.0f)]
	public float flickerDelay = 0.2f;

	private float flickerDelayTimer = 0;
	private float maxFlickerIntensity = 0;
	private float minFlickerIntensity = 0;

	[Range(0f, 0.20f)]
	public float flickerSpeed = 0.06f;

	private float colorTime = 1;

	[Range(0f, 0.10f)]
	public float colorSpeed = 0.02f;

	private Color GlowLightBasic = new Color(1f,1f,1f,1f);

	private float maximumGlow = 0.75f;
	private float minimumGlow = 0.25f;


	private bool lightWasGivenLastFrame = false;

	private float availableTimeInDark = 20f;
	private float _lightIntensity = 0f;
	private float _looseConditionTimer = 0f;

	private int lastCheckPoint = 0;

	private bool m_haveSeenRecently = false;
	private bool m_isScared = false;
	public bool IsScared 
	{ 
		get
		{
			return m_isScared;
		} 
		set
		{
			if( siljaOnScaredAudio != null  )
			{
				if(value && !m_haveSeenRecently)
				{
					siljaOnScaredAudio.Play();
				}
			}
			
			
			if( audioRunTension != null )
			{
				if(value && !m_haveSeenRecently)
				{
					audioRunTension.Play();
				}
			}

			if( value && !m_haveSeenRecently)
			{
				m_haveSeenRecently = true;
				Invoke("CleanSeenRecentlyFlag", 10f);
			}

			m_isScared = value;
		}
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

        interactor = gameObject.AddComponent<Interactor>();
        gameObject.AddComponent<SiljaShakeOnScary>();

		camTransitioner = thisCamera.GetComponent<CameraTransitioner>();

		cameraFollow = thisCamera.GetComponent<CameraFollow>();
        cameraFollow.Initialize(charAnimation);

        maxFlickerIntensity = mimimumIntensity * 2;
		minFlickerIntensity = mimimumIntensity * 1.5f;

		firstPersonRig.gameObject.SetActive(false);
	}
	

	void Start()
	{
		EnableThirdPerson();
		DarknessApproachingTimer = LillebrorLightLifetime + TimeInDarknessTotal;
	}

	public void SetLastCheckpointIndex( int idx )
	{
		if( idx > lastCheckPoint )
			lastCheckPoint = idx;
	}
	
	public float getTeddyLight()
	{
		return _lightIntensity;
	}

	public float getMimimumIntensity()
	{
		return mimimumIntensity;
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Y))
		{
			Application.LoadLevelAsync(1);
		}

		if(		Input.GetKeyUp( KeyCode.Q ) 
		   && 	camTransitioner.Mode != CameraTransitioner.CameraMode.Transitioning
		   &&	!interactor.isInteracting)
		{
			if( isLookingInFP )
			{
				isLookingInFP = false;
				ShiftToThirdPerson();
				EnableFlashlight ( false );
			}
			else
			{
				isLookingInFP = true;
				ShiftToFirstPerson();
				EnableFlashlight ( true );
			}
		}

		if( Input.GetKeyUp( KeyCode.F ) && isLookingInFP )
		{
			EnableFlashlight( !isFlashlightEnabled );
		}
	}

	public float ShiftDuration = 1f;

	public void ShiftToFirstPerson()
	{
		cameraFollow.CamControlType = CameraFollow.CameraControlType.CCT_FPSLook;	
		camTransitioner.TransitionTPPtoFPP();
		Invoke("EnableFirstPerson",ShiftDuration);
	}

	public void EnableFirstPerson() {

		teddyLight.enabled = true;

		GlowLightBasic = teddyLight.color;

        moveCtrl.SetWalkingSpeed(1.5f, 1.5f);

//		isLookingInFP = true;

	}

	public void ShiftToThirdPerson()
	{
		camTransitioner.TransitionFPPtoTPP();

		Invoke("EnableThirdPerson",ShiftDuration);
	}
	
	public void EnableThirdPerson() {

		teddyLight.enabled = false;

        moveCtrl.SetWalkingSpeed(1.1f, 0.9f);

		cameraFollow.CamControlType = CameraFollow.CameraControlType.CCT_Default;
	}

	public void EnableFlashlight(bool state)
	{
		if( !isFlashLightCollected )
			return;

		if( flashlightTurnAudio != null  )
			flashlightTurnAudio.Play();
		else
			Debug.LogError("No flash light audio source found");

		isFlashlightEnabled = state;
		firstPersonRig.gameObject.SetActive(state);
	}
	
	public void RetriveLightProbeResult(float intensity)
	{
		_lightIntensity += fadingInSpeed;

		SetLightIntensity();

		lightWasGivenLastFrame = true;
	}

	private void LateUpdate()
	{
		if( !lightWasGivenLastFrame )
		{
			if(!DarknessApproaching)
			{
				DarknessApproachingTimer = LillebrorLightLifetime + TimeInDarknessTotal;
			}

			_lightIntensity -= fadingOutSpeed;
			SetLightIntensity();

			if(DarknessApproachingTimer >= TimeInDarknessTotal && (!isLookingInFP || IsScared))
			{
				DarknessApproachingTimer -= Time.deltaTime * darknessIntensifier;
				thirdPersonInDark = true;
			}
			else
			{
				thirdPersonInDark = false;
				DarknessApproachingTimer -= Time.deltaTime;
			}
			inDarkness = true;
		}
		else
		{
			if(DarknessApproachingTimer < (LillebrorLightLifetime + TimeInDarknessTotal))
			{
				DarknessApproachingTimer += Time.deltaTime * rechargeSpeed;
			}
			else
			{
				DarknessApproachingTimer = LillebrorLightLifetime + TimeInDarknessTotal;
			}
			
			inDarkness = false;
		}

		_looseConditionTimer = _lightIntensity == 0f ? _looseConditionTimer + Time.deltaTime : 0f;

		/*
		if( _lightIntensity == 0f )
			healthController.PlayScaredAudio();
		*/

		lightWasGivenLastFrame = false;
	}

	private void OnGUI()
	{
		/*
		GUILayout.Label("Light intensity: " + _lightIntensity.ToString("0.0"));
		GUILayout.Label("Time left: " + DarknessApproachingTimer.ToString("0"));
		*/
	}

	public void SetLightIntensity()
	{
		_lightIntensity = Mathf.Clamp(_lightIntensity, mimimumIntensity, maximumIntensity);
		
		// map from 0 to 1
		float glowIntensity = (_lightIntensity - mimimumIntensity) / (maximumIntensity - mimimumIntensity);
		
		// from minimum glow to maximum glow
		glowIntensity = glowIntensity * (maximumGlow - minimumGlow) + minimumGlow;
		
		
		//Sounds of darkness & fear
		if(heartBeatAudioSource != null && !heartBeatAudioSource.isPlaying)
		{
			heartBeatAudioSource.Play();
		}
		
		volume = 2.0f - (DarknessApproachingTimer*2 / (LillebrorLightLifetime + TimeInDarknessTotal));

		if( heartBeatAudioSource != null )
			heartBeatAudioSource.volume = volume;
		
		if(breathingAudioSource != null && !breathingAudioSource.isPlaying)
		{
			breathingAudioSource.Play();
		}
		
		volume2 = 1.0f - Mathf.Pow(DarknessApproachingTimer / (LillebrorLightLifetime + TimeInDarknessTotal),0.4f);
		if( breathingAudioSource != null )
			breathingAudioSource.volume = volume2;
		
		if(monsterAudioSource != null && !monsterAudioSource.isPlaying)
		{
			monsterAudioSource.Play();
		}
		
		if(thirdPersonInDark || teddyLightFlash.intensity == 0 && volume3 < 1)
		{
			volume3 += Time.deltaTime;
		}
		else if(volume3 > 0)
		{
			volume3 -= Time.deltaTime;
		}

		if( monsterAudioSource != null )
			monsterAudioSource.volume = volume3;
		
		//Lights charge & flicker
		if(inDarkness)
		{
			if(DarknessApproachingTimer <= TimeInDarknessTotal + (TimeInDarknessTotal / 1.3f) && DarknessApproachingTimer >= TimeInDarknessTotal + (TimeInDarknessTotal / 1.4f))
			{
				lightFlickerInterval += Time.deltaTime;
				
				if(lightFlickerInterval < 0.2f)
				{
					teddyLight.intensity = 0.3f;
					teddyLightFlash.intensity = 1.0f;
					teddyLightFlash2.intensity = 1.0f;
				}
				else if(lightFlickerInterval >= 0.2f && lightFlickerInterval < ((TimeInDarknessTotal / 1.3f) - (TimeInDarknessTotal / 1.35f)))
				{
					teddyLight.intensity = 1.0f;
					teddyLightFlash.intensity = 2.0f;
					teddyLightFlash2.intensity = 2.0f;
				}
				else
				{
					teddyLight.intensity = 0.3f;
					teddyLightFlash.intensity = 1.0f;
					teddyLightFlash2.intensity = 1.0f;
				}
			}
			
			if(DarknessApproachingTimer <= TimeInDarknessTotal + 1.0f)
			{
				float lightIntensity = DarknessApproachingTimer - TimeInDarknessTotal;
				
				teddyLightFlash.intensity = lightIntensity;
				teddyLightFlash2.intensity = lightIntensity;
			}
		}
		else
		{
			lightFlickerInterval = 0.0f;
			
			float lightIntensity = DarknessApproachingTimer*2 / (LillebrorLightLifetime + TimeInDarknessTotal);

			_lightIntensity = (lightIntensity / 2);
			teddyLight.intensity = (lightIntensity / 2);
			teddyLightFlash.intensity = lightIntensity;
			teddyLightFlash2.intensity = lightIntensity;
		}
		
		//Out of light, monster coming
		if(DarknessApproachingTimer <= TimeInDarknessTotal)
		{
			teddyLight.intensity = 0.3f;
			teddyLightFlash.intensity = 0.0f;
			teddyLightFlash2.intensity = 0.0f;
		}

		if( monsterAudioSource != null )
		{
			if(DarknessApproachingTimer <= TimeInDarknessTotal - (TimeInDarknessTotal / 1.3f) && moveCtrl.isMoving())
			{
				monsterAudioSource.clip = monsterChase;
			}
			else
			{
				monsterAudioSource.clip = monsterSearching;
			}
		}
		
		if(DarknessApproachingTimer <= 0)
		{
			//Monster comes
			caughtTimer += Time.deltaTime;
			moveCtrl.EnableMoving(false);
			
			AudioSource soundSource = GameObject.Find("HeadAudioSource").transform.GetComponent<AudioSource>();
			if(!soundSource.isPlaying && caughtTimer < 0.5f)
			{
				soundSource.clip = monsterCatchSiljaSound[Random.Range(0, monsterCatchSiljaSound.Length)];
				soundSource.Play();
			}
			
			if(caughtTimer > captureTimeTotal - 0.1f && caughtTimer <= captureTimeTotal)
			{
				volume = 0.0f;
				Fader.Instance.FadeScreen(true, 1.0f);
			}
			else if(caughtTimer >= captureTimeTotal + blackscreenTime)
			{
				isCaught = true;
			}
		}
		
		if(isCaught)
		{
			caughtTimer = 0;
            moveCtrl.EnableMoving(true);
			
			DarknessApproachingTimer = LillebrorLightLifetime + TimeInDarknessTotal;
//			transform.position = spawnPoint.position;
			ObjectsTranslator.MoveObjectTo( lastCheckPoint );

			teddyLight.intensity = 1.0f;
			teddyLightFlash.intensity = 2.0f;
			teddyLightFlash2.intensity = 2.0f;
			
			EnableFirstPerson();
			StartCoroutine(BlinkingEffect());
			GetComponent<Animation>().Play("waking_up");
			isCaught = false;
		}
		
		/*
		if( intensity > lightTreshold )
			teddyLight.intensity += fadingInSpeed;
		else
			teddyLight.intensity -= fadingOutSpeed;

		Debug.Log(teddyLight.intensity);
	
		SetLightIntensity(teddyLight.intensity);
		*/
		
		teddyLightFlash2.intensity = teddyLightFlash.intensity;
		
		//		if( _lightIntensity > mimimumIntensity )
		//		{
		//			foreach( AIBehaviour aiEntity in aiEntities )
		//				aiEntity.DespawnAI();
		//		}
	}

	public void isDarknessApproaching(bool boolean)
	{
		DarknessApproaching = boolean;
	}

	private IEnumerator BlinkingEffect()
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
}
