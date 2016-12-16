Shader "Custom/Highlight" {
    Properties
    {
        _MainTex("Main Texture", 2D) = "white"
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    fixed _Cutoff;
    half _Intensity;

    fixed4 _Color;

    sampler2D _MainTex;
    sampler2D _SecondTex;

    float4 vert_drawObject(float4 vertex : POSITION) : POSITION {

        float4 mvPos = mul(UNITY_MATRIX_MV, vertex);
        float4 mvPivot = mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1));

        mvPos -= mvPivot;
        mvPos.xy *= (1 + _Cutoff);
        mvPos += mvPivot;

        return mul(UNITY_MATRIX_P, mvPos);
    }

    fixed4 frag_drawObject(float4 pos : SV_POSITION) : SV_TARGET {
        return _Color;
    }

    fixed4 frag_overlay(v2f_img i) : SV_TARGET {
        fixed4 mcol = tex2D(_MainTex, i.uv);
        fixed4 scol = tex2D(_SecondTex, i.uv);

        return mcol + scol * _Intensity;
    }
   
    ENDCG
	
	SubShader {

		ZWrite off

        //
        // 0 : 
        Pass {
            ColorMask 0
            
            Stencil {
                Ref 1
                Comp always
                Pass replace
            }

            CGPROGRAM
            #pragma vertex vert_drawObject
            #pragma fragment frag_drawObject
            ENDCG

        }

        // 
        // 1:  
        Pass {
            ColorMask RGB
            
            Stencil {
                Ref 1
                Comp always
                Pass replace
            }

            CGPROGRAM
            #pragma vertex vert_drawObject
            #pragma fragment frag_drawObject
            ENDCG
        }

        // 2:
        Pass {     
            Stencil {
                Ref 2
                Comp always
                Pass replace
            }
           
            SetTexture [_MainTex] { 
                constantColor[_Color] combine constant
            }
        }

        // 3: overlay stencil
        Pass {
            ZTest always 

            Stencil {
                Ref 1
                Comp equal
            }

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag_overlay
            ENDCG
        }    

        // 4: debug stencil drawer
        Pass {
            Stencil {
                Ref 1
                Comp equal
            }
    
            SetTexture [_MainTex] { 
                constantColor[_Color] combine constant 
            }
        }
	} 
}
