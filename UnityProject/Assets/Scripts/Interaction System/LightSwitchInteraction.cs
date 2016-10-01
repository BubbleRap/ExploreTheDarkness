using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class LightSwitchInteraction : IInteractableObject 
{
	public Light[] _lightSource;

	private Collider _collider;
    private bool m_interacted = true;


	#region Lau-san stuff
	public Renderer[] _emissiveSurfaces;

	float emissionIntensity = 1.0f;
	Color[] emitColorOn;
	#endregion

	private void Awake()
	{
        base.Awake();

		foreach( Collider col in GetComponents<Collider>() )
			if( col.isTrigger )
			{
				_collider = col;
				break;
			}
        _collider.enabled = m_interacted;

		foreach( Light light in _lightSource )
            light.gameObject.SetActive(m_interacted);


		emitColorOn = new Color[_emissiveSurfaces.Length];
		for( int i = 0; i < _emissiveSurfaces.Length; i++ )
		{
			emitColorOn[i] = _emissiveSurfaces[i].material.GetColor("_EmissionColor");

            if( m_interacted )
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

        m_interacted = !m_interacted;

		foreach( Light light in _lightSource )
            light.enabled = m_interacted;

        _collider.enabled = m_interacted;

        if( m_interacted )
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

        ObjectivesManager.Instance.OnInteractionComplete( this, m_interacted );

		return false;
	}
}
