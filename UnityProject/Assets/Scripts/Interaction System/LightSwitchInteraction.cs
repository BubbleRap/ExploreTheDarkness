using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class LightSwitchInteraction : IInteractableObject 
{
	public Light _lightSource;
	public bool _defaultState = false;

	private Collider _collider;
	private SiljaBehaviour _cachedBeh;

	private void Awake()
	{
		_collider = GetComponent<Collider>();
		_collider.enabled = _lightSource.enabled = interactionIsActive = _defaultState;
	}

	public override bool Activate()
	{
		return _collider.enabled = _lightSource.enabled = interactionIsActive = !interactionIsActive;
	}

	private void Update()
	{
		if( !interactionIsActive )
			return;

		if( _cachedBeh != null )
			_cachedBeh.RetriveLightProbeResult(1f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if( other.tag != "Player" )
			return;

		_cachedBeh = other.gameObject.GetComponent<SiljaBehaviour>();
	}

	private void OnTriggerExit(Collider other)
	{
		if( other.tag != "Player" )
			return;

		_cachedBeh = null;
	}

	private void OnDisable()
	{
		_cachedBeh = null;
	}

}
