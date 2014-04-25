using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {
	
	public Color fadeColor = Color.black;
	private Texture2D fadeTexture = null; 
	
	private bool stillBlack = false;
	private bool stillFading = true;
	
	public float fadeTime = 3f;
	private float fadeStart = 0f; 
	
	public bool showText = true;
	
	[Multiline]
	private string intro = 
		"See you next time ;)";
	
	public GUIStyle style;
	
	void Awake()
	{
		fadeTexture = new Texture2D(1,1, TextureFormat.RGBA32, false, false);
		fadeTexture.SetPixel(0,0, fadeColor);
		fadeTexture.Apply();
	}
	
	private void OnGUI() 
	{ 
		if( !stillBlack && stillFading)
		{
			Color guiColor = fadeColor;
			guiColor.a = (Time.time - fadeStart)/fadeTime;
			
			GUI.color = guiColor;
		}
		
		if( stillFading )
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),fadeTexture);

		if( showText && stillBlack)
			GUI.Label (new Rect (Screen.width / 2, Screen.height/2, 0, Screen.height * (8 / 10)), intro, style);
	}
	
	void Update()
	{		
		if (Time.time - fadeStart > fadeTime)
		{
			stillBlack = true;
		}
	}
	
}