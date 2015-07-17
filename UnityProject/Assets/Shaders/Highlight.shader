Shader "Custom/Highlight" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "black" {}
		_OccludeMap ("Occlusion Map", 2D) = "black" {}
	}
	
	SubShader {

		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		
		
		// OVERLAY GLOW
		
		Pass {
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
			
				sampler2D _MainTex;
				sampler2D _OccludeMap;
				
				half4 _Color;
			
				half4 frag(v2f_img IN) : COLOR 
				{
					return tex2D (_MainTex, IN.uv) + tex2D(_OccludeMap, IN.uv) * _Color.a;
				}
			ENDCG
		}
		
		// OVERLAY SOLID
		
		Pass {
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
			
				sampler2D _MainTex;
				sampler2D _OccludeMap;
				
				half4 _Color;
			
				half4 frag(v2f_img IN) : COLOR 
				{
					half4 mCol = tex2D (_MainTex, IN.uv);
					half4 oCol = tex2D (_OccludeMap, IN.uv);
					
					half solid = step (1.0 - _Color.a, length(oCol));
					half4 c = mCol + solid * half4(_Color.rgb, 1.0);
					return c;
				}
			ENDCG
		}
	
		
		// OCCLUSION
		
		Pass {
				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
			
				sampler2D _MainTex;
				sampler2D _OccludeMap;
			
				half4 frag(v2f_img IN) : COLOR 
				{
					return tex2D (_MainTex, IN.uv) - tex2D(_OccludeMap, IN.uv);
				}
			ENDCG
		}
	} 
}
