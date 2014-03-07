using UnityEngine;
using System.Collections;

public class LuminocityCalc : MonoBehaviour 
{

	public RenderTexture renderTexture = null; 
	public Camera[] cameras;

	void Update () 
	{
		int a_Width = 16, a_Height = 16;

//		List<Camera> cameras = new List<Camera>(Camera.allCameras);

		renderTexture = RenderTexture.GetTemporary(a_Width, a_Height, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		RenderTexture.active = renderTexture;  

		Texture2D result = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
		int lightIntensity = 0;

		foreach (Camera camera in cameras)
		{
//			if (camera.enabled)
//			{

			float fov = camera.fov;



			camera.targetTexture = renderTexture;
			camera.Render();
			camera.targetTexture = null;


			result.ReadPixels(new Rect(0.0f, 0.0f, renderTexture.width, renderTexture.height), 0, 0, false);
			result.Apply();

			Color32 [] buffer = result.GetPixels32();
			for( int i = 0; i < buffer.Length; i++ )
				lightIntensity += (buffer[i].r  + buffer[i].g + buffer[i].b) / 3;


			camera.fov = fov;
//			}
		}
		

		
		RenderTexture.active = null;
		
		RenderTexture.ReleaseTemporary(renderTexture);

		print (lightIntensity / cameras.Length);
	}
}
