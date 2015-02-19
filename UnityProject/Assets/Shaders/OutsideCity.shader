Shader "Custom/OutsideCity" {
	Properties {
		_TintColor ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Fog { Mode Linear Density 0.005 Range 50.0, 1500.0}
		
		CGPROGRAM
		#pragma surface surf Lambert

		fixed4 _TintColor;
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _TintColor.rgb;
			o.Alpha = c.a * _TintColor.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
