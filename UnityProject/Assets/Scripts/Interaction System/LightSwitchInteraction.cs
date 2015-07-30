using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class LightSwitchInteraction : IInteractableObject 
{
	public Light[] _lightSource;
	public bool _defaultState = false;

	private Collider _collider;
	private SiljaBehaviour _cachedBeh;

	private void Awake()
	{
		foreach( Collider col in GetComponents<Collider>() )
			if( col.isTrigger )
			{
				_collider = col;
				break;
			}
		_collider.enabled = interactionIsActive = _defaultState;

		foreach( Light light in _lightSource )
			light.enabled = interactionIsActive;
	}

	public override bool Activate()
	{
		if( !ObjectivesManager.Instance.IsInteractionEligable( this ) )
			return false;

		foreach( Light light in _lightSource )
			light.enabled = !interactionIsActive;

		_collider.enabled = interactionIsActive = !interactionIsActive;

		ObjectivesManager.Instance.OnInteractionComplete( this, interactionIsActive );

		return false;
	}

	private void Update()
	{
		base.Update();

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
