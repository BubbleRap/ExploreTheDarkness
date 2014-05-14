using UnityEngine;
using System.Collections;

public class SiljaBehaviour : MonoBehaviour 
{
	public Health healthController;
	public Transform oneHandJoint;
	public Transform twoHandsJoint;

	public Animator firstPersonAnimator;
	public GameObject[] limbs;
	public GameObject[] limbPrefabs;

	private CharacterMotor charMotor = null;
	private MovementController moveCtrl = null;
	private FPSInputController fpsInputCtrl = null;
	private MouseLook mLook = null;

	private GameObject firstPersonCamera = null;
	private interact interactScript = null;
	
	private Light teddyLight = null;
	private DynamicLightProbe dLightProbe = null;

	[HideInInspector]
	public bool darkMode = false;

	public Material lilbroGlowMaterial = null;

	private AIBehaviour[] aiEntities = null;

	private float lightTreshold = 0.15f;
	private float fadingOutSpeed = 0.006f;
	private float fadingInSpeed = 0.012f;

	private float maximumIntensity = 1.75f;
	private float mimimumIntensity = 0.25f;

	private float maximumGlow = 0.75f;
	private float minimumGlow = 0.25f;

	//private SkinnedMeshRenderer siljaRenderer = null;
	private CameraShaker firstPersonCameraShaker;

	void Awake()
	{
		firstPersonCamera = transform.FindChild("1st Person Camera").gameObject;

		charMotor = GetComponent<CharacterMotor>();
		moveCtrl = GetComponent<MovementController>();
		fpsInputCtrl = GetComponent<FPSInputController>();
		mLook = firstPersonCamera.GetComponent<MouseLook>();
		interactScript = GetComponent<interact>();

		teddyLight = twoHandsJoint.GetComponentInChildren<Light>();

		dLightProbe = GetComponentInChildren<DynamicLightProbe> ();
		aiEntities = FindObjectsOfType<AIBehaviour>();

		//siljaRenderer = transform.FindChild("Silja_Animated").GetComponentInChildren<SkinnedMeshRenderer>();

		firstPersonCameraShaker = firstPersonCamera.GetComponent<CameraShaker>();
	}

	public void refreshAIReferences(){
		aiEntities = FindObjectsOfType<AIBehaviour>();
	}

	void Start()
	{
		//teddyLight.enabled = true;
		teddyLight.intensity = maximumIntensity;

		lilbroGlowMaterial.color = new Color(1f,1f,1f,0f);
	}
	
	public float getTeddyLight()
	{
		return teddyLight.intensity;
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
		refreshAIReferences();

		teddyLight.enabled = true;

		charMotor.movement.maxForwardSpeed = 1.5f;
		charMotor.movement.maxSidewaysSpeed = 1.5f;
		moveCtrl.enabled = false;
		
		fpsInputCtrl.enabled = true;
		mLook.enabled = true;
		firstPersonCamera.SetActive(true);

		oneHandJoint.gameObject.SetActive(false);
		twoHandsJoint.gameObject.SetActive(true);

		dLightProbe.enabled = true;

		lilbroGlowMaterial.color = new Color(1f,1f,1f,1f);

		//siljaRenderer.material.shader = Shader.Find("Custom/TransparentInvisibleShadowCaster");

		darkMode = true;
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

		oneHandJoint.gameObject.SetActive(true);
		twoHandsJoint.gameObject.SetActive(false);

		dLightProbe.enabled = false;

		lilbroGlowMaterial.color = new Color(1f,1f,1f,0f);

		//siljaRenderer.material.shader = Shader.Find("Custom/DoubleSided/Diffuse");

		darkMode = false;
	}

	// is sent by light probe itself
	public void RetriveLightProbeResult(float intensity)
	{
		if( intensity > lightTreshold )
			teddyLight.intensity += fadingInSpeed;
		else
			teddyLight.intensity -= fadingOutSpeed;
	
		SetLightIntensity(teddyLight.intensity);
		
		if( teddyLight.intensity <= mimimumIntensity )
		{
			foreach( AIBehaviour aiEntity in aiEntities )
				aiEntity.SpawnAI();
			
			if(RenderSettings.ambientLight.b < 0.14f)
				RenderSettings.ambientLight = new Color(RenderSettings.ambientLight.b/2, RenderSettings.ambientLight.b/2, RenderSettings.ambientLight.b + 0.002f, 0.0f);
		}

		if( teddyLight.intensity >= maximumIntensity )
		{
			foreach( AIBehaviour aiEntity in aiEntities )
				aiEntity.DespawnAI();
			
			if(RenderSettings.ambientLight.b > 0.00f)
				RenderSettings.ambientLight = new Color(RenderSettings.ambientLight.r - 0.001f, RenderSettings.ambientLight.g - 0.001f, RenderSettings.ambientLight.b - 0.002f, 0.0f);
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

		GameObject newLimb = Instantiate(limbPrefabs[index], teddyLight.transform.position, teddyLight.transform.rotation) as GameObject;
		newLimb.AddComponent<DestroyInSeconds>();
		newLimb.rigidbody.AddForce(Camera.main.transform.forward * 30f, ForceMode.Impulse);

		limbs[index].SetActive(false);


		fpsInputCtrl.enabled = true;
		mLook.enabled = true;
	}
}
