using UnityEngine;
using System.Collections;

public class SkyBoxManager : MonoBehaviour {

	public float lightExposure = 0.9f;
	public float darkExposure = 0.2f;

	void Start()
	{
		RenderSettings.skybox.SetFloat("_Exposure", lightExposure);
	}

	void OnDisable()
	{
		RenderSettings.skybox.SetFloat("_Exposure", lightExposure);
	}

	public void LightMode()
	{
		RenderSettings.skybox.SetFloat("_Exposure", lightExposure);
	}

	public void DarkMode()
	{
		RenderSettings.skybox.SetFloat("_Exposure", darkExposure);
	}

}
