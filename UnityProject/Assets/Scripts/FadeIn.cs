using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {

	public Color fadeColor;
	private Texture2D fadeTexture = null; 

	private bool stillBlack = true;
	private bool stillFading = true;

	private float fadeTime = 3f;
	private float fadeStart = 0f; 

	public bool showText = true;

	[Multiline]
	public string intro = 
		"- No, I don't have time! You can do it yourself once!\n"+
			"- Don't be stupid, Silja, you know I can't!\n"+
			"- No! I'm APPOINTED!\n"+
			"- APPOINTED? You're NOT!\n\n"+
			"[Door slams]\n"+
			"- Silja!\n\n\n\n\n\n"+
			"[Press E]";

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
			guiColor.a = 1f - (Time.time - fadeStart)/fadeTime;
			print (guiColor.a);
			GUI.color = guiColor;
		}

		if( stillFading )
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),fadeTexture);

		if( showText && stillBlack)
			GUI.Label (new Rect (Screen.width / 2, Screen.height/2, 0, Screen.height * (8 / 10)), intro, style);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			stillBlack = false;
			fadeStart = Time.time;
		}

		if (Time.time - fadeStart > fadeTime)
		{
			stillFading = false;
		}
	}

}