using UnityEngine;
using System.Collections;

public class LookInDetailInteraction : IInteractableObject 
{
	[Range(0.001f, 0.1f)]
	public float _rotationSensetivity = 0.1f;

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

	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;

		if( _siljaBeh == null )
		{
			GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
			_siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();
		}

		if( interactionIsActive )
		{
			_siljaBeh.EnableDarkMode();
			_siljaBeh.IsFPSLookEnabled = false;
			_siljaBeh.IsFPSMoveEnabled = false;

			Transform fpCamTransform = _siljaBeh.firstPersonCamera.transform;
			fpCamTransform.LookAt(transform.position);

			transform.position = fpCamTransform.TransformPoint(Vector3.forward * 0.5f);
			transform.rotation = fpCamTransform.rotation;

			_collider.enabled = false;
		}
		else
		{
			_siljaBeh.EnableStoryMode();

			transform.position = _originalPos;
			transform.rotation = _originalRot;

			_collider.enabled = true;
		}
	}

	private void Update()
	{
		Vector3 mouseVelocity = Input.mousePosition - _prevMousePos;
		_prevMousePos = Input.mousePosition;

		if( interactionIsActive )
		{
			if( Input.GetMouseButton(0) )
			{
				transform.Rotate( mouseVelocity.y * _rotationSensetivity, mouseVelocity.x * _rotationSensetivity, 0f );
			}
		}
	}
}
