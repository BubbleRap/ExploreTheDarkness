﻿using UnityEngine;
using System.Collections;

public class ShadowModeController : MonoBehaviour 
{
	public Respawn respawnController;
	public Health healthController;
	
	private Light teddyLight = null;
	private AIBehaviour[] aiEntities = null;

	[Range(0f, 1f)]
	[HideInInspector]
	public float lightTreshold = 0.95f;
	[Range(0f, 3.00f)]
	[HideInInspector]
	public float fadingOutSpeed = 3.00f;
	[Range(0f, 3.0f)]
	[HideInInspector]
	public float fadingInSpeed = 3.000f;

	public Material lilbroGlowMaterial = null;

	private float timeLeft = 10.0f;

	void Awake()
	{
		teddyLight = respawnController.GetComponentInChildren<Light>();


		aiEntities = FindObjectsOfType<AIBehaviour>();
    }

	void Start()
	{
		//teddyLight.enabled = true;
		teddyLight.intensity = 1.75f;
		lilbroGlowMaterial.color = Color.white;
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

			timeLeft -= Time.deltaTime;

			if(timeLeft <= 0.0f)
			{
				healthController.looseLife();
				timeLeft = 10;
			}

			if(RenderSettings.ambientLight.b < 0.14f)
				RenderSettings.ambientLight = new Color(RenderSettings.ambientLight.b/2, RenderSettings.ambientLight.b/2, RenderSettings.ambientLight.b + 0.002f, 0.0f);
		}
		else
		{
			foreach( AIBehaviour aiEntity in aiEntities )
				aiEntity.DespawnAI();

			if(timeLeft == 10)
			{
				timeLeft = 10;
			}

			if(RenderSettings.ambientLight.b > 0.00f)
				RenderSettings.ambientLight = new Color(RenderSettings.ambientLight.r - 0.001f, RenderSettings.ambientLight.g - 0.001f, RenderSettings.ambientLight.b - 0.002f, 0.0f);
		}
	}

	public float getTeddyLight()
	{
		return teddyLight.intensity;
	}
}
