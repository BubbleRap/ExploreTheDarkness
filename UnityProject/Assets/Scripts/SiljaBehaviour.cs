using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SiljaBehaviour : MonoBehaviour 
{
	public Health healthController;
	public Transform oneHandJoint;
	public Transform twoHandsJoint;

	public Animator siljaAnimation;
	public Animator firstPersonAnimator;
	public GameObject[] limbs;
	public GameObject[] limbPrefabs;

	private CharacterMotor charMotor = null;
	private MovementController moveCtrl = null;
	private FPSInputController fpsInputCtrl = null;
	private MouseLook mLook = null;
	private CameraInput camInput = null;

	public GameObject firstPersonCamera = null;
	public GameObject thirdPersonCamera = null;

	public GameObject lightProbeOnSilja;

	private float ambientGlow = 0.0f;

	public Light teddyLight = null;
	public Light teddyLightFlash = null;
	public Light teddyLightFlash2 = null;
	private DynamicLightProbe dLightProbe = null;

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
	public Transform spawnPoint;

	[HideInInspector]
	static public bool darkMode = false;

	public Material lilbroGlowMaterial = null;

	private AIBehaviour[] aiEntities = null;

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

//	private SkinnedMeshRenderer siljaRenderer = null;

	private CameraShaker firstPersonCameraShaker;
	private CameraFollow cameraFollowCom = null;

	private bool lightWasGivenLastFrame = false;

	private float availableTimeInDark = 20f;
	private float _lightIntensity = 0f;
	private float _looseConditionTimer = 0f;


	// accessors
	public bool IsFPSLookEnabled 
	{
		get { return mLook.enabled; }
		set { mLook.enabled = value; }
	}

	public bool IsFPSMoveEnabled 
	{
		get { return fpsInputCtrl.enabled; }
		set 
		{
			charMotor.inputMoveDirection = Vector3.zero;
			fpsInputCtrl.enabled = value; 
		}
	}

	void Awake()
	{
		charMotor = GetComponent<CharacterMotor>();
		moveCtrl = GetComponent<MovementController>();
		fpsInputCtrl = GetComponent<FPSInputController>();
		mLook = firstPersonCamera.GetComponent<MouseLook>();
		camInput = GetComponentInChildren<CameraInput>();

		dLightProbe = GetComponentInChildren<DynamicLightProbe> ();
		aiEntities = FindObjectsOfType<AIBehaviour>();

		firstPersonCameraShaker = firstPersonCamera.GetComponent<CameraShaker>();
		cameraFollowCom = thirdPersonCamera.GetComponent<CameraFollow>();

		maxFlickerIntensity = mimimumIntensity * 2;
		minFlickerIntensity = mimimumIntensity * 1.5f;
	}

	public void refreshAIReferences(){
//		aiEntities = FindObjectsOfType<AIBehaviour>();
	}

	void Start()
	{
		EnableStoryMode();
		DarknessApproachingTimer = LillebrorLightLifetime + TimeInDarknessTotal;
	}
	
	public float getTeddyLight()
	{
		return _lightIntensity;
	}

	public float getMimimumIntensity()
	{
		return mimimumIntensity;
	}

	public void TakeALimb(Transform entity)
	{
		StartCoroutine(RipLimbDelayedAction(entity, 0.3f));
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Y))
		{
			Application.LoadLevelAsync(1);
		}

		if( Input.GetKeyUp( KeyCode.Q ) )
		{
			if( darkMode )
				EnableStoryMode();
			else
				EnableDarkMode();
		}
	}

	public void EnableDarkMode()
	{
//		StartCoroutine(BlinkingEffect());
//		refreshAIReferences();


		teddyLight.enabled = true;
		lightProbeOnSilja.SetActive(false); 


		lilbroGlowMaterial.color = new Color(1f,1f,1f,0f);
		GlowLightBasic = teddyLight.color;


		charMotor.movement.maxForwardSpeed = 1.5f;
		charMotor.movement.maxSidewaysSpeed = 1.5f;
		moveCtrl.enabled = false;
		
		fpsInputCtrl.enabled = true;
		mLook.enabled = true;
		firstPersonCamera.SetActive(true);
		thirdPersonCamera.SetActive(false);

		oneHandJoint.gameObject.SetActive(false);
		twoHandsJoint.gameObject.SetActive(true);

		lilbroGlowMaterial.color = new Color(1f,1f,1f,1f);

//		GetComponent<Animation>().Play("waking_up");

		darkMode = true;
	}

	public void EnableStoryMode()
	{
		teddyLight.enabled = false;
		lightProbeOnSilja.SetActive(false); 

		charMotor.movement.maxForwardSpeed = 1;
		charMotor.movement.maxSidewaysSpeed = 1;
		moveCtrl.enabled = true;
		
		fpsInputCtrl.enabled = false;
		mLook.enabled = false;
		firstPersonCamera.SetActive(false);
		thirdPersonCamera.SetActive(true);

		oneHandJoint.gameObject.SetActive(true);
		twoHandsJoint.gameObject.SetActive(false);


		lilbroGlowMaterial.color = new Color(1f,1f,1f,0f);

		darkMode = false;
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

			if(DarknessApproachingTimer >= TimeInDarknessTotal && !darkMode)
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
		GUILayout.Label("Light intensity: " + _lightIntensity.ToString("0.0"));
		GUILayout.Label("Time left: " + DarknessApproachingTimer.ToString("0"));
	}

	public void SetLightIntensity()
	{
		_lightIntensity = Mathf.Clamp(_lightIntensity, mimimumIntensity, maximumIntensity);
		
		// map from 0 to 1
		float glowIntensity = (_lightIntensity - mimimumIntensity) / (maximumIntensity - mimimumIntensity);
		
		// from minimum glow to maximum glow
		glowIntensity = glowIntensity * (maximumGlow - minimumGlow) + minimumGlow;
		
		float colorIntensity = glowIntensity;
		lilbroGlowMaterial.color = new Color(colorIntensity, colorIntensity, colorIntensity, glowIntensity);
		//teddyLight.intensity = _lightIntensity;
		
		//Sounds of darkness & fear
		if(!heartBeatAudioSource.isPlaying)
		{
			heartBeatAudioSource.Play();
		}
		
		volume = 2.0f - (DarknessApproachingTimer*2 / (LillebrorLightLifetime + TimeInDarknessTotal));
		heartBeatAudioSource.volume = volume;
		
		if(!breathingAudioSource.isPlaying)
		{
			breathingAudioSource.Play();
		}
		
		volume2 = 1.0f - Mathf.Pow(DarknessApproachingTimer / (LillebrorLightLifetime + TimeInDarknessTotal),0.4f);
		breathingAudioSource.volume = volume2;
		
		if(!monsterAudioSource.isPlaying)
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

		if(DarknessApproachingTimer <= TimeInDarknessTotal - (TimeInDarknessTotal / 1.3f) && moveCtrl.isMoving())
		{
			monsterAudioSource.clip = monsterChase;
		}
		else
		{
			monsterAudioSource.clip = monsterSearching;
		}
		
		if(DarknessApproachingTimer <= 0)
		{
			//Monster comes
			caughtTimer += Time.deltaTime;
			moveCtrl.canMove = false;
			
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
			moveCtrl.canMove = true;
			
			DarknessApproachingTimer = LillebrorLightLifetime + TimeInDarknessTotal;
			transform.position = spawnPoint.position;

			teddyLight.intensity = 1.0f;
			teddyLightFlash.intensity = 2.0f;
			teddyLightFlash2.intensity = 2.0f;
			
			EnableDarkMode();
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

	IEnumerator RipLimbDelayedAction(Transform entity, float ripLimbIn)
	{
		fpsInputCtrl.enabled = false;
		mLook.enabled = false;


		float t = 0f;
		Quaternion originalLook = transform.rotation;
		while(t < ripLimbIn)
		{
			Vector3 direction = (entity.position - transform.position).normalized;
			direction.y = 0f;

			transform.rotation = Quaternion.Lerp(originalLook, Quaternion.LookRotation(direction), (t/ripLimbIn));

			t += Time.deltaTime;
			yield return null;
		}

		healthController.looseLife();
		firstPersonAnimator.SetTrigger("riplimb" + healthController.health);

		firstPersonCameraShaker.StartShake(1f);

		_lightIntensity = 1f;
		SetLightIntensity();

		yield return new WaitForSeconds(ripLimbIn);
		int index = healthController.health;

//		GameObject newLimb = Instantiate(limbPrefabs[index], teddyLight.transform.position, teddyLight.transform.rotation) as GameObject;
//		newLimb.AddComponent<DestroyInSeconds>();
//		newLimb.rigidbody.AddForce(Camera.main.transform.forward * 30f, ForceMode.Impulse);

		limbs[index].SetActive(false);


		fpsInputCtrl.enabled = true;
		mLook.enabled = true;
	}

	public void LookAtPointFP(bool state, Transform lookAtObject, Transform lookFromPoint)
	{
//		firstPersonCamera.gameObject.SetActive(state);
//		LookAtPoint(state, lookAtObject, lookFromPoint);

		cameraFollowCom.isFocusing = state;
		cameraFollowCom.focusPoint = lookAtObject;

		camInput.enabled = !state;
	}

	public void LookAtPoint(bool state, Transform lookAtObject, Transform lookFromPoint)
	{
		charMotor.enabled = !state;
		//if( state )
		//{
			//if( lookFromPoint != null )
			//{
				Vector3 lookFrom = lookFromPoint.position;
				transform.position = new Vector3(lookFrom.x, transform.position.y, lookFrom.z);
			//}
			
			firstPersonCamera.transform.LookAt(lookAtObject);
		//}
		transform.gameObject.GetComponent<MovementController>().canMove = !state;
		camInput.enabled = !state;
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
