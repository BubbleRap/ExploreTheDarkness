
Shader "Hidden/Gamma Effect" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest
		#include "UnityCG.cginc"
		
		uniform sampler2D _MainTex;
		uniform half _Gamma;
		
		half4 frag (v2f_img i) : COLOR
		{
			half4 orig = tex2D(_MainTex, i.uv);
			half4 col = half4( pow( orig.rgb, _Gamma ), orig.a);	
			
			return col;
		}
		ENDCG

	}
}

Fallback Off

}