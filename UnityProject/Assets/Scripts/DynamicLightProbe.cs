using UnityEngine;
using System.Collections;

public class DynamicLightProbe : MonoBehaviour 
{
	public Transform objectToProbe;
	public Vector3 probeOffset;

	public int renderTextureSize = 16;
	
	public float lightIntensity = 0;

	private Camera[] cameras;

	void Start()
	{
		cameras = transform.GetComponentsInChildren<Camera>();

		foreach( Camera cam in cameras )
			cam.enabled = false;

		if( objectToProbe == null )
			Debug.LogError("You didn't assign object to light probe!");
	}

	void Update()
	{
		if( objectToProbe == null )
			return;

		transform.position = objectToProbe.position + probeOffset;
		transform.rotation = objectToProbe.rotation;
	}

	void LateUpdate () 
	{
		if( objectToProbe == null )
			return;

		RenderTexture renderTexture = RenderTexture.GetTemporary(renderTextureSize, renderTextureSize, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		RenderTexture.active = renderTexture;  

		Texture2D result = new Texture2D(renderTextureSize, renderTextureSize, TextureFormat.RGB24, false);
		lightIntensity = 0;

		float camerasCount = (float) cameras.Length;

		
		foreach (Camera camera in cameras)
		{
			float fov = camera.fov;

			camera.targetTexture = renderTexture;
			camera.Render();
			camera.targetTexture = null;

			result.ReadPixels(new Rect(0.0f, 0.0f, renderTextureSize, renderTextureSize), 0, 0, false);
			result.Apply();

			Color [] buffer = result.GetPixels();
			for( int i = 0; i < buffer.Length; i++ )
			{
				buffer[i] -= RenderSettings.ambientLight;
				lightIntensity += (buffer[i].r  + buffer[i].g + buffer[i].b) / (3f * renderTextureSize * renderTextureSize * camerasCount);
			}

			camera.fov = fov;
		}

		objectToProbe.SendMessage("RetriveLightProbeResult", lightIntensity, SendMessageOptions.DontRequireReceiver);
	
		
		RenderTexture.active = null;
		
		RenderTexture.ReleaseTemporary(renderTexture);

	}

}
