using UnityEngine;
using System.Collections;

public class CameraFade : MonoBehaviour 
{
	public delegate void OnTriggerEvent();
	public OnTriggerEvent fadedOut = null;


	public Color fadeColor;

	[Range(0f,1f)]
	public float fadeIntensity = 0f;

	Material glMaterial = null;

	void Awake()
	{
//		glMaterial = new Material( "Shader \"Custom/GL\" {" +
//		                          "SubShader { Tags { \"Queue\" = \"Transparent\" \"RenderType\"=\"Transparent\"}" +
//		                          "Pass {" +
//		                          "   BindChannels { Bind \"Color\",color }" +
//		                          "   Blend SrcAlpha OneMinusSrcAlpha" +
//		                          "   ZWrite Off Cull Off Fog { Mode Off }" +
//		                          "} } }");
	}

	void LateUpdate()
	{
		fadeIntensity = Mathf.Clamp01(fadeIntensity);

		if( fadeIntensity >= 1f )
			if( fadedOut != null )
				fadedOut();
	}
	
	void OnPostRender()
	{
//		if (glMaterial == null)
//		{
//			print("Please assign a material on the inspector");
//			return;
//		}


//		GL.PushMatrix();
//		glMaterial.SetPass(0);
//		
//		GL.LoadPixelMatrix();
//		GL.Viewport(new Rect(0, 0, Screen.width, Screen.height));
//		GL.Color(fadeColor * fadeIntensity);
//		GL.Begin(GL.QUADS);
//		GL.Vertex3(0, 0, 0);
//		GL.Vertex3(0, Screen.height , 0);
//		GL.Vertex3(Screen.width , Screen.height, 0);
//		GL.Vertex3(Screen.width , 0, 0);
//		GL.End();
//		GL.PopMatrix();

	}
}
