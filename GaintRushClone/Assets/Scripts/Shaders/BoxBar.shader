Shader "Unlit/BoxBar"
{
   
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Slider("Slider",Range(0,1))=0.5
        _StartColor("StartColor",Color)=(1,0,0,1)
        _EndColor("EndColor",Color)=(0,1,0,1)
        _EmptyColor("EmptyColor",Color)=(0,0,0,1)
        
       _ColorStartRange("Color Start",Range(0,1))=0
       _ColorEndRange("Color Ende",Range(0,1))=1
       _BorderOffset("BorderOffset",Range(0,1))=0.5
       _Health("Health",Range(0,1))=0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent""Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal:NORMAL;
            };
             float CARP(float X,float Y, float Z){
                return (Z-X)/(Y-X);
                }
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Slider;
            float4 _EndColor;
            float4 _StartColor;
            float4 _EmptyColor;
            float _ColorStartRange;
            float _ColorEndRange;
            float _Health;
            float _BorderOffset;
            v2f vert (MeshData v)
            {
                v2f o;
                o.normal= v.normal;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
             
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {

               float2 coord=i.uv;
                coord.x*=8;
                float2 pointsOnLinesg=float2(clamp(coord.x,0.5,7.5),0.5);
                float sdf=distance(coord,pointsOnLinesg)*2-1;
                clip(-sdf);

                float borderOffset=sdf+_BorderOffset;

            

               // return float4(borderOffset.xxx,1);

               float pd=fwidth(borderOffset);
              
                float boarderMask=1-saturate(borderOffset/pd);
               //return float4(pd.xxx,1);

                float healthMask=_Health>i.uv.x;
             
                float3 healthBarColor=tex2D(_MainTex,i.uv);
                float3 colorLerp=lerp(_EmptyColor,healthBarColor,_Health);
                if(_Health<0.2){
                     float flash=cos(_Time.y*4)*0.4+1;
                     healthBarColor*=flash;
                }
                return float4(healthBarColor*healthMask*boarderMask,1);   
            }
            ENDCG
        }
    }
}
