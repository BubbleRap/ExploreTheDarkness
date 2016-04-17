using UnityEngine;
using System.Collections;

public class SkyBoxManager : MonoBehaviour {

	float lightExposure = 0.9f;
	float darkExposure = 0.2f;

	public void LightMode()
	{
		RenderSettings.skybox.SetFloat("_Exposure", lightExposure);
	}

	public void DarkMode()
	{
		RenderSettings.skybox.SetFloat("_Exposure", darkExposure);
	}

}
