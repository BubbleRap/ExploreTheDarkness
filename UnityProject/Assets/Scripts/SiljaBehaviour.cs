using UnityEngine;
using System.Collections;

public class SiljaBehaviour : MonoBehaviour 
{
	public Health healthController;
	public Transform oneHandJoint;
	public Transform twoHandsJoint;

	private Animator siljasAnimator = null;
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

	void Awake()
	{
		firstPersonCamera = transform.FindChild("1st Person Camera").gameObject;

		siljasAnimator = GetComponentInChildren<Animator>();
		charMotor = GetComponent<CharacterMotor>();
		moveCtrl = GetComponent<MovementController>();
		fpsInputCtrl = GetComponent<FPSInputController>();
		mLook = firstPersonCamera.GetComponent<MouseLook>();
		interactScript = GetComponent<interact>();

		teddyLight = twoHandsJoint.GetComponentInChildren<Light>();

		dLightProbe = GetComponentInChildren<DynamicLightProbe> ();
		aiEntities = FindObjectsOfType<AIBehaviour>();

	}

	void Start()
	{
		//teddyLight.enabled = true;
		teddyLight.intensity = 1.75f;

		lilbroGlowMaterial.color = new Color(1f,1f,1f,0f);
	}
	
	public float getTeddyLight()
	{
		return teddyLight.intensity;
	}

	public void TakeALimb()
	{
		healthController.looseLife();
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
		teddyLight.enabled = true;

		charMotor.movement.maxForwardSpeed = 1.5f;
		charMotor.movement.maxSidewaysSpeed = 1.5f;
		moveCtrl.enabled = false;
		
		fpsInputCtrl.enabled = true;
		mLook.enabled = true;
		firstPersonCamera.SetActive(true);

		oneHandJoint.gameObject.SetActive(false);
		twoHandsJoint.gameObject.SetActive(true);

		//siljasAnimator.SetBool("darkmode", true);
		dLightProbe.enabled = true;

		lilbroGlowMaterial.color = new Color(1f,1f,1f,1f);

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

		//siljasAnimator.SetBool("darkmode", false);
		dLightProbe.enabled = false;

		lilbroGlowMaterial.color = new Color(1f,1f,1f,0f);

		darkMode = false;
	}

	// is sent by light probe itself
	public void RetriveLightProbeResult(float intensity)
	{
		if( intensity > lightTreshold )
			teddyLight.intensity += fadingInSpeed;
		else
			teddyLight.intensity -= fadingOutSpeed;
		
		teddyLight.intensity = Mathf.Clamp(teddyLight.intensity, 0f, 1.75f);

		lilbroGlowMaterial.color = new Color(teddyLight.intensity, teddyLight.intensity, teddyLight.intensity, 1f);
		
		if( teddyLight.intensity == 0f )
		{
			foreach( AIBehaviour aiEntity in aiEntities )
				aiEntity.SpawnAI();
			
			if(RenderSettings.ambientLight.b < 0.14f)
				RenderSettings.ambientLight = new Color(RenderSettings.ambientLight.b/2, RenderSettings.ambientLight.b/2, RenderSettings.ambientLight.b + 0.002f, 0.0f);
		}
		else
		{
			foreach( AIBehaviour aiEntity in aiEntities )
				aiEntity.DespawnAI();
			
			if(RenderSettings.ambientLight.b > 0.00f)
				RenderSettings.ambientLight = new Color(RenderSettings.ambientLight.r - 0.001f, RenderSettings.ambientLight.g - 0.001f, RenderSettings.ambientLight.b - 0.002f, 0.0f);
		}
	}
}
