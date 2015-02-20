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

	private GameObject firstPersonCamera = null;
	private GameObject thirdPersonCamera = null;

	private GameObject lightProbeOnSilja; // The lightprobe gameobject so it can be switched on/off -> disabling it in void Start and enabling it again in void EnableDarkMode

	private float ambientGlow = 0.0f;

	private Light teddyLight = null;
	private Light teddyLightFlash = null;
	private Light teddyLightFlash2 = null;
	private DynamicLightProbe dLightProbe = null;

	[HideInInspector]
	public bool darkMode = false;

	public Material lilbroGlowMaterial = null;

	private AIBehaviour[] aiEntities = null;

	[Range(0f, 0.10f)]
	public float lightTreshold = 0.08f;

	[Range(0f, 0.020f)]
	public float fadingOutSpeed = 0.006f;

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

	void Awake()
	{
		firstPersonCamera = transform.FindChild("1st Person Camera").gameObject;
		thirdPersonCamera = transform.FindChild("3rd Person Camera").gameObject;

		lightProbeOnSilja = transform.FindChild("LightProbe_Silja").gameObject;

		charMotor = GetComponent<CharacterMotor>();
		moveCtrl = GetComponent<MovementController>();
		fpsInputCtrl = GetComponent<FPSInputController>();
		mLook = firstPersonCamera.GetComponent<MouseLook>();
		camInput = GetComponentInChildren<CameraInput>();

		teddyLight = twoHandsJoint.GetComponentsInChildren<Light>()[0];
		teddyLightFlash = twoHandsJoint.GetComponentsInChildren<Light>()[0];
		teddyLightFlash2 = twoHandsJoint.GetComponentsInChildren<Light>()[1];

		dLightProbe = GetComponentInChildren<DynamicLightProbe> ();
		aiEntities = FindObjectsOfType<AIBehaviour>();

//		siljaRenderer = transform.FindChild("Silja_Animated").GetComponentInChildren<SkinnedMeshRenderer>();
//		siljaAnimation = transform.FindChild("Silja_Animated").GetComponentInChildren<Animator>();

		firstPersonCameraShaker = firstPersonCamera.GetComponent<CameraShaker>();
		cameraFollowCom = thirdPersonCamera.GetComponent<CameraFollow>();

		maxFlickerIntensity = mimimumIntensity * 2;
		minFlickerIntensity = mimimumIntensity * 1.5f;
	}

	public void refreshAIReferences(){
		aiEntities = FindObjectsOfType<AIBehaviour>();
	}

	void Start()
	{
		if(darkMode == false)
		{
			lightProbeOnSilja.SetActive(false); //Lightprobe disabled in adventure mode
		}

		//teddyLight.enabled = true;
		teddyLight.intensity = maximumIntensity;

		lilbroGlowMaterial.color = new Color(1f,1f,1f,0f);
		GlowLightBasic = teddyLight.color;
	}
	
	public float getTeddyLight()
	{
		return teddyLight.intensity;
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
	}

	public void EnableDarkMode()
	{
		StartCoroutine(BlinkingEffect());

		refreshAIReferences();

		teddyLight.enabled = true;
		lightProbeOnSilja.SetActive(true); //Lightprobe enabled in adventure mode

		charMotor.movement.maxForwardSpeed = 1.5f;
		charMotor.movement.maxSidewaysSpeed = 1.5f;
		moveCtrl.enabled = false;
		
		fpsInputCtrl.enabled = true;
		mLook.enabled = true;
		firstPersonCamera.SetActive(true);
		thirdPersonCamera.SetActive(false);

		oneHandJoint.gameObject.SetActive(false);
		twoHandsJoint.gameObject.SetActive(true);

		//dLightProbe.enabled = true;

		lilbroGlowMaterial.color = new Color(1f,1f,1f,1f);

		animation.Play("waking_up");

//		siljaRenderer.material.shader = Shader.Find("Custom/TransparentInvisibleShadowCaster");

		darkMode = true;
//		siljaAnimation.SetBool ("darkmode", darkMode);
	}

	public void EnableStoryMode()
	{
		teddyLight.enabled = false;

		charMotor.movement.maxForwardSpeed = 1;
		charMotor.movement.maxSidewaysSpeed = 1;
		moveCtrl.enabled = true;
		
		fpsInputCtrl.enabled = false;
		mLook.enabled = false;
		firstPersonCamera.SetActive(false);
		thirdPersonCamera.SetActive(true);

		oneHandJoint.gameObject.SetActive(true);
		twoHandsJoint.gameObject.SetActive(false);

		//dLightProbe.enabled = false;

		lilbroGlowMaterial.color = new Color(1f,1f,1f,0f);

//		siljaRenderer.material.shader = Shader.Find("Custom/DoubleSided/Diffuse");

		darkMode = false;
//		siljaAnimation.SetBool ("darkmode", darkMode);
	}

	// is sent by light probe itself
	public void RetriveLightProbeResult(float intensity)
	{
		if( intensity > lightTreshold )
			teddyLight.intensity += fadingInSpeed;
		else
			teddyLight.intensity -= fadingOutSpeed;
	
		SetLightIntensity(teddyLight.intensity);

		RenderSettings.ambientLight = new Color(RenderSettings.ambientLight.b/2, (RenderSettings.ambientLight.b/2)/* * GlowLightBasic.g*/, ambientGlow/* * GlowLightBasic.b*/, 0.0f);
		
		lilbroGlowMaterial.color = new Color(lilbroGlowMaterial.color.r, lilbroGlowMaterial.color.g, lilbroGlowMaterial.color.b, 1.0f);

		//teddyLight.color = new Color(GlowLightBasic.r, GlowLightBasic.g, GlowLightBasic.b);

		//Debug.Log(flickerIntervalTimer);
		if( teddyLight.intensity <= maxFlickerIntensity)
		{
			flickerIntervalTimer += Time.deltaTime;

			/*
			if(intensity <= lightTreshold && maxFlickerIntensity >= (minFlickerIntensity * 1.2f) && flickerIntervalTimer > 4)
			{
				flickerTime += flickerSpeed;
				teddyLight.intensity = Mathf.Lerp(maxFlickerIntensity, minFlickerIntensity, flickerTime);

				if( teddyLight.intensity <= minFlickerIntensity)
				{
					flickerDelayTimer += Time.deltaTime;
					if(flickerDelayTimer > flickerDelay)
					{
						flickerDelayTimer = 0;
						flickerTime = 0;
						if(flickerIntervalTimer > 8)
						{
							flickerDelay = flickerDelay * Random.Range(0.90f, 1.20f);
							flickerSpeed = flickerSpeed * Random.Range(0.85f, 1.15f);
							maxFlickerIntensity = maxFlickerIntensity * Random.Range(0.80f, 0.90f);
						}
						else
						{
							maxFlickerIntensity = maxFlickerIntensity * Random.Range(0.80f, 1.30f);
						}
						minFlickerIntensity = minFlickerIntensity;
					}
				}
			}
			*/

			/*
			if(flickerIntervalTimer > 6)
			{
				colorTime += colorSpeed;
			}
			*/
		}
		else
		{
			/*
			if(colorTime > 1)
			{
				colorTime -= (colorSpeed * 3);
			}
			*/
		}

		if(teddyLight.intensity >= (maximumIntensity * 0.95) /* && maxFlickerIntensity < (mimimumIntensity * 5) */)
		{
			flickerTime = 0;
			flickerIntervalTimer = 0;
			flickerSpeed = 0.06f;
			flickerDelay = 0.2f;
			maxFlickerIntensity = mimimumIntensity * 2;
			minFlickerIntensity = mimimumIntensity * 1.5f;
		}

		if( teddyLight.intensity <= mimimumIntensity)
		{
			if(ambientGlow < 0.1f)
			{
				ambientGlow += 0.001f;
			}
		}
		else
		{
			if(ambientGlow > 0.0f)
			{
				ambientGlow -= 0.005f;
			}
		}

		teddyLightFlash2.intensity = teddyLightFlash.intensity;

		if( teddyLight.intensity > mimimumIntensity )
		{
			foreach( AIBehaviour aiEntity in aiEntities )
				aiEntity.DespawnAI();
		}
	}

	public void SetLightIntensity(float intensity)
	{
		intensity = Mathf.Clamp(intensity, mimimumIntensity, maximumIntensity);
		
		// map from 0 to 1
		float glowIntensity = (intensity - mimimumIntensity) / (maximumIntensity - mimimumIntensity);
		
		// from minimum glow to maximum glow
		glowIntensity = glowIntensity * (maximumGlow - minimumGlow) + minimumGlow;
		
		float colorIntensity = glowIntensity;
		lilbroGlowMaterial.color = new Color(colorIntensity, colorIntensity, colorIntensity, glowIntensity);
		teddyLight.intensity = intensity;
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
		SetLightIntensity(1f);

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
