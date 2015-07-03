Shader "Custom/SolidColor" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
    SubShader {
        Pass {
        
        	ZWrite Off
        
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            fixed4 _Color;

            float4 vert(float4 v:POSITION) : SV_POSITION {
                return mul (UNITY_MATRIX_MVP, v);
            }

            fixed4 frag() : SV_Target {
                return fixed4(_Color.rgb * _Color.a, 1f);
            }

            ENDCG
        }
    }
}