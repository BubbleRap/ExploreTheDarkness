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

	private ShadowModeController shadowController = null;
	private Light teddyLight = null;
	private DynamicLightProbe dLightProbe = null;

	[HideInInspector]
	public bool darkMode = false;

	void Awake()
	{
		firstPersonCamera = transform.FindChild("1st Person Camera").gameObject;

		siljasAnimator = GetComponentInChildren<Animator>();
		charMotor = GetComponent<CharacterMotor>();
		moveCtrl = GetComponent<MovementController>();
		fpsInputCtrl = GetComponent<FPSInputController>();
		mLook = firstPersonCamera.GetComponent<MouseLook>();
		interactScript = GetComponent<interact>();

		shadowController = GetComponent<ShadowModeController>();
		teddyLight = twoHandsJoint.GetComponentInChildren<Light>();

		dLightProbe = GetComponentInChildren<DynamicLightProbe> ();
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
		if(firstPersonCamera.gameObject.activeInHierarchy)
			return;

		shadowController.enabled = true;
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

		darkMode = true;
	}

	public void EnableStoryMode()
	{
		if(!firstPersonCamera.gameObject.activeInHierarchy || interactScript.isInteractMode)
			return;

		shadowController.enabled = false;
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

		darkMode = false;
	}

}
