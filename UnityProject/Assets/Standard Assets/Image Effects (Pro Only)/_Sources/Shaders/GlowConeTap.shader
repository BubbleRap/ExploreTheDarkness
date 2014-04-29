Shader "Hidden/GlowConeTap" {

Properties {
	_Color ("Color", color) = (1,1,1,0)
	_MainTex ("", 2D) = "white" {}
}

Category {
	ZTest Always Cull Off ZWrite Off Fog { Mode Off }

	Subshader {
		Pass {
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				#include "UnityCG.cginc"

				struct v2f {
					float4 pos : POSITION;
					float2 uv : TEXCOORD0;
					half4 blur_uv[2] : TEXCOORD1;
				};

				float4 _MainTex_TexelSize;
				float4 _BlurOffsets;

				v2f vert (appdata_img v)
				{
					v2f o;
					float offX = _MainTex_TexelSize.x * _BlurOffsets.x;
					float offY = _MainTex_TexelSize.y * _BlurOffsets.y;

					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
					float2 blur_uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy-float2(offX, offY));
					o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord.xy); 
					
					o.blur_uv[0].xy = blur_uv + float2( offX, offY);
					o.blur_uv[0].zw = blur_uv + float2(-offX, offY);
					o.blur_uv[1].xy = blur_uv + float2( offX,-offY);
					o.blur_uv[1].zw = blur_uv + float2(-offX,-offY);
					return o;
				}
				
				sampler2D _MainTex;
				fixed4 _Color;

				fixed4 frag( v2f i ) : COLOR
				{
					fixed4 c;
					c  = tex2D( _MainTex, i.blur_uv[0].xy );
					c += tex2D( _MainTex, i.blur_uv[0].zw );
					c += tex2D( _MainTex, i.blur_uv[1].xy );
					c += tex2D( _MainTex, i.blur_uv[1].zw );
					c.rgb *= _Color.rgb;
					return c * _Color.a;//(1.0 - tex2D(_MainTex, i.uv).a); 
				}
			ENDCG
		}
	}

	Subshader {
		Pass {
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant alpha}
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}		
		}

	}
}

Fallback off

}
