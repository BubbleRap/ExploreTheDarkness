using UnityEngine;
using System.Collections;

public class ShadowModeController : MonoBehaviour 
{
	public CameraFade cameraFadeController;
	public DynamicLightProbe probeController;
	public Respawn respawnController;

	[Range(0f, 1f)]
	public float lightTreshold = 0.1f;
	[Range(0f, 0.1f)]
	public float fadingOutSpeed = 0.005f;
	[Range(0f, 0.1f)]
	public float fadingInSpeed = 0.01f;

	void Start()
	{
		cameraFadeController.fadedOut += () =>
		{
			// restart when out of light
			respawnController.RespawnToLastPosition();
		};
	}

	void Update()
	{
		if( probeController.lightIntensity > lightTreshold )
			cameraFadeController.fadeIntensity -= fadingInSpeed;
		else
			cameraFadeController.fadeIntensity += fadingOutSpeed;
	}
}
