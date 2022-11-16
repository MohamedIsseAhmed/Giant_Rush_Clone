Shader "Unlit/TransparentWall"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorA (" _ColorA",Color)=(1,1,1,1)
        _ColorB (" _ColorB",Color)=(1,1,1,1)
        _ColorStart ("_ColorStart", Range(0,1)) = 1
        _ColorEnd ("_ColorEnd",  Range(0,1)) = 0
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"    
        
        }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
               
                float4 vertex : SV_POSITION;
            };

            
            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;

            v2f vert (MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv=v.uv;
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
              
                return o;
            }
            float inversLerp(float a,float b,float v)
            {
                return (v-a)/(b-a);
            }
            fixed4 frag (v2f i) : SV_Target
            {
               
                float t=saturate(inversLerp(_ColorStart,_ColorEnd,i.uv.y));
                t=frac(t);
                float4 outColor=lerp(_ColorA,_ColorB,t);
                return outColor;
            }
            ENDCG
        }
    }
}
