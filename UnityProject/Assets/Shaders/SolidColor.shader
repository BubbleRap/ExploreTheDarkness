﻿Shader "Custom/SolidColor" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
    SubShader {

		Pass {
        
           	Tags {"RenderType"="Opaque"}
        	ZWrite On
        	ZTest LEqual
        	Fog { Mode Off }
        
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            fixed4 _Color;

            float4 vert(float4 v:POSITION) : POSITION {
                return mul (UNITY_MATRIX_MVP, v);
            }

            fixed4 frag() : SV_Target {
                return fixed4(_Color.rgb * _Color.a, 1.0);
            }

            ENDCG
        }	
    	
        Pass {
           	
           	Tags {"Queue"="Transparent"}
        	Cull Back
            Lighting Off
            ZWrite Off
            ZTest LEqual
            ColorMask RGBA
            Blend OneMinusDstColor One
        
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            fixed4 _Color;
            
            sampler2D _CameraDepthTexture;
            
            struct appdata_mine {
                float4 vertex : POSITION;
            };
            
            struct v2f {
                float4 vertex : POSITION;
                float4 projPos : TEXCOORD1;
            };
     
            v2f vert( appdata_mine v ) {
            
                v2f o;
                o.vertex = mul( UNITY_MATRIX_MVP, v.vertex );
                o.projPos = ComputeScreenPos(o.vertex);
                
                return o;
            }

            fixed4 frag( v2f i ) : SV_Target {
            
                float sceneZ = LinearEyeDepth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r);
                float partZ = i.projPos.z;
                
                float occlude = step( partZ, sceneZ );
                
                return fixed4(  occlude * 
                				_Color.rgb * _Color.a, occlude);
            }

            ENDCG
        }
    }
}