using UnityEngine;
using System.Collections;

public class ShadowModeController : MonoBehaviour 
{
	public CameraFade cameraFadeController;
	public DynamicLightProbe probeController;
	public Respawn respawnController;

	private Light teddyLight = null;

	[Range(0f, 1f)]
	public float lightTreshold = 0.1f;
	[Range(0f, 0.1f)]
	public float fadingOutSpeed = 0.005f;
	[Range(0f, 0.1f)]
	public float fadingInSpeed = 0.01f;

	void Start()
	{
		teddyLight = respawnController.GetComponentInChildren<Light>();

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

		//if( probeController.lightIntensity > lightTreshold )
		//	teddyLight.intensity += fadingInSpeed;
		//else
		//	teddyLight.intensity -= fadingOutSpeed;
//
		//teddyLight.intensity = Mathf.Clamp(teddyLight.intensity, 0.2f, 0.25f);
	}
}
