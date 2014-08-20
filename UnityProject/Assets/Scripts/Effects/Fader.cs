using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour 
{
	// implementing the real real singleton
	private static Fader _instance = null;
	public static Fader Instance
	{
		get 
		{
			if( _instance == null )
			{
				GameObject goInstance = new GameObject("Fader Singleton Instance");
				_instance = goInstance.AddComponent<Fader>();
				_instance.cachedGuiTexture = goInstance.AddComponent<GUITexture>();

				_instance.cachedGuiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);

				Texture2D blackSample = new Texture2D(1,1, TextureFormat.RGBA32, false, false);
				blackSample.SetPixel(0,0, Color.black);
				blackSample.Apply();

				_instance.cachedGuiTexture.texture = blackSample;
			}
			return _instance;
		}
	}

	private GUITexture cachedGuiTexture = null;

	public void FadeScreen( bool state, float time = 1f )
	{
		StopAllCoroutines();
		StartCoroutine(FadeEffect(state, time));
	}

	private IEnumerator FadeEffect(bool state, float time)
	{
		float timer = 0f;
		while( timer <= timer )
		{
			timer += Time.deltaTime;

			float lerpValue = Mathf.Clamp01(state ? timer / time : 1f - timer / time);
			cachedGuiTexture.color = Color.Lerp(Color.clear, Color.black, lerpValue);
			yield return null;
		}
	}
}
