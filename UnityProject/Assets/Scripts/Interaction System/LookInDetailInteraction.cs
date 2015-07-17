using UnityEngine;
using System.Collections;

using UnityStandardAssets.ImageEffects;

public class LookInDetailInteraction : IInteractableObject 
{
	public enum ObjectOrientation
	{
		Y_up,
		Z_up
	}

	public ObjectOrientation orientation = ObjectOrientation.Y_up;
	[Range(0.001f, 0.5f)]
	public float _rotationSensetivity = 0.4f;
	public float _faceDistance = 0.25f;

	private SiljaBehaviour _siljaBeh;
	private Vector3 _prevMousePos;

	private Vector3 _originalPos;
	private Quaternion _originalRot;

	private Collider _collider;

	void Awake()
	{
		_originalPos = transform.position;
		_originalRot = transform.rotation;

		_collider = GetComponent<Collider>();
	}

	public override bool Activate()
	{
		interactionIsActive = !interactionIsActive;
		IsCompleted = interactionIsActive;

		if( _siljaBeh == null )
		{
			GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
			_siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();
		}


		if( interactionIsActive )
		{
			OnInvestigateEnabled();
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
			
		transitioner.AddFPPCompleteAction( () =>
		{
			camControl.CamControlType = CameraFollow.CameraControlType.CCT_Overwritten;

			Transform fpCamTransform = transitioner.FPPCameraTransform;
			transform.position = fpCamTransform.TransformPoint(Vector3.forward * _faceDistance);

			Camera.main.GetComponent<DepthOfField>().focalLength = _faceDistance;
			Camera.main.GetComponent<DepthOfField>().aperture = 50f;

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
		
		
		_siljaBeh.ShiftToThirdPerson();
	}

	private new void Update()
	{
		base.Update();

		Vector3 mouseVelocity = Input.mousePosition - _prevMousePos;
		_prevMousePos = Input.mousePosition;

		if( interactionIsActive )
		{
			if( Input.GetMouseButton(0) )
			{
				transform.Rotate( -mouseVelocity.y * _rotationSensetivity, -mouseVelocity.x * _rotationSensetivity, 0f );
			}
		}
	}
}
