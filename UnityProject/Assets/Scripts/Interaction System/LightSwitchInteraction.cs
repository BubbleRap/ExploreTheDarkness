using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class LightSwitchInteraction : IInteractableObject 
{
	public Light[] _lightSource;

	public bool _defaultState = false;

	private Collider _collider;
	private SiljaBehaviour _cachedBeh;


	#region Lau-san stuff
	public Renderer[] _emissiveSurfaces;

	float emissionIntensity = 1.0f;
	Color[] emitColorOn;
	#endregion

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


		emitColorOn = new Color[_emissiveSurfaces.Length];
		for( int i = 0; i < _emissiveSurfaces.Length; i++ )
		{
			emitColorOn[i] = _emissiveSurfaces[i].material.GetColor("_EmissionColor");

			if( _defaultState )
			{
				_emissiveSurfaces[i].material.SetColor("_EmissionColor", emitColorOn[i]);
				DynamicGI.SetEmissive(_emissiveSurfaces[i], emitColorOn[i] * Mathf.LinearToGammaSpace(emissionIntensity));
			}
			else
			{
				_emissiveSurfaces[i].material.SetColor("_EmissionColor", Color.black);
				DynamicGI.SetEmissive(_emissiveSurfaces[i], Color.black);
			}
		}
	}

	public override bool Activate()
	{
		if( !ObjectivesManager.Instance.IsInteractionEligable( this ) )
			return false;

		foreach( Light light in _lightSource )
			light.enabled = !interactionIsActive;

		_collider.enabled = interactionIsActive = !interactionIsActive;

		if( interactionIsActive )
		{
			for( int i = 0; i < _emissiveSurfaces.Length; i++ )
			{
				_emissiveSurfaces[i].material.SetColor("_EmissionColor", emitColorOn[i]);
				DynamicGI.SetEmissive(_emissiveSurfaces[i], emitColorOn[i] * Mathf.LinearToGammaSpace(emissionIntensity));
			}
		}
		else
		{
			for( int i = 0; i < _emissiveSurfaces.Length; i++ )
			{
				_emissiveSurfaces[i].material.SetColor("_EmissionColor", Color.black);
				DynamicGI.SetEmissive(_emissiveSurfaces[i], Color.black);
			}
		}

		ObjectivesManager.Instance.OnInteractionComplete( this, interactionIsActive );

		return false;
	}

	private void Update()
	{
		base.Update();

		if( !interactionIsActive )
			return;

        if (_cachedBeh != null)
            _cachedBeh.flshCtrl.ChargeFlashlight();

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
