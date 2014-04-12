using UnityEngine;
using System.Collections;

public class ShadowModeController : MonoBehaviour 
{
	public CameraFade cameraFadeController;
	public DynamicLightProbe probeController;
	public Respawn respawnController;


	private Light teddyLight = null;

	[Range(0f, 1f)]
	[HideInInspector]
	public float lightTreshold = 0.15f;
	[Range(0f, 0.1f)]
	[HideInInspector]
	public float fadingOutSpeed = 0.006f;
	[Range(0f, 0.1f)]
	[HideInInspector]
	public float fadingInSpeed = 0.012f;

	public Material lilbroGlowMaterial = null;

	void Start()
	{
		teddyLight = respawnController.GetComponentInChildren<Light>();
		teddyLight.enabled = true;
		teddyLight.intensity = 0.75f;
		lilbroGlowMaterial.color = Color.white;

		cameraFadeController.fadedOut += () =>
		{
			// restart when out of light
			//respawnController.RespawnToLastPosition();
			//cameraFadeController.fadeIntensity = 0f;
		};
	}

	void Update()
	{
		//if( probeController.lightIntensity > lightTreshold )
		//	cameraFadeController.fadeIntensity -= fadingInSpeed;
		//else
		//	cameraFadeController.fadeIntensity += fadingOutSpeed;

		if( probeController.lightIntensity > lightTreshold )
			teddyLight.intensity += fadingInSpeed;
		else
			teddyLight.intensity -= fadingOutSpeed;


		teddyLight.intensity = Mathf.Clamp(teddyLight.intensity, 0f, 0.75f);
		lilbroGlowMaterial.color = new Color(teddyLight.intensity, teddyLight.intensity, teddyLight.intensity, 1f);
	}
}
