Shader "Custom/ReflectiveEyes" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1) 
		_MainTex ("Main Tex", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		
		CGPROGRAM
		#pragma surface surf Lambert alpha
		
		half4 _Color;
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 texColor = tex2D(_MainTex, IN.uv_MainTex);
			float angle = saturate(dot (normalize(IN.viewDir), normalize(IN.worldNormal)));
			//o.Emission = lerp(half3(0.0f), texColor.rgb, angle);
			if( angle > 0.99f )
				o.Emission = texColor.rgb;
			else
				o.Emission = half3(0.0f);
			o.Alpha = texColor.a;
		}
		ENDCG
	} 
}
