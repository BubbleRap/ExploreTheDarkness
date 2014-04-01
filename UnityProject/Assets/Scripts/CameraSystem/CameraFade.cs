using UnityEngine;
using System.Collections;

public class CameraFade : MonoBehaviour 
{
	public delegate void OnTriggerEvent();
	public OnTriggerEvent fadedOut = null;

	public Texture2D blackTexture;
	public Color fadeColor;
	private Renderer fadeFilter = null;

	[Range(0f,1f)]
	public float fadeIntensity = 0f;

	private Material glMaterial = null;

	void Awake()
	{
		fadeFilter = GetComponentInChildren<Renderer> ();
		glMaterial = fadeFilter.sharedMaterial;
	}

	void Update()
	{
		glMaterial.color = fadeColor * fadeIntensity;
	}

	void OnGUI() {
		//GUI.color = new Color(0, 0, 0, fadeIntensity);
		//GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), blackTexture );
	}

	void LateUpdate()
	{
		fadeIntensity = Mathf.Clamp01(fadeIntensity);

		if( fadeIntensity >= 1f )
			if( fadedOut != null )
				fadedOut();
	}
}
