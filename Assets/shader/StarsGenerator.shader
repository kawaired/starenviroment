Shader "Unlit/StarsGenerator"
{
    Properties
    {
        _MainTex ("maintex", 2D) = "white" {}
        _SmallStarTex("smallstartex",2D) = "white"{}
        _MediumStarTex("mediustartex",2D) = "white"{}
        _BigStarTex("bigstartex",2D) = "white"{}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define MAX_STAR 1000
            #define Star_H_Scale 0.25

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            sampler2D _SmallStarTex;
            float4 _SmallStarTex_ST;
            float4 _SmallStarTex_TexelSize;

            sampler2D _MediumStarTex;
            float4 _MediumStarTex_ST;
            float4 _MediumStarTex_TexelSize;

            sampler2D _BigStarTex;
            float4 _BigStarTex_ST;
            float4 _BigStarTex_TexelSize;

            float _PixelSizeH;
            float _PixelSizeV;
            float _StarNum;
            float4 _RandomStarDatas[MAX_STAR];

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 CalculateStarColor(float4 stardata,float2 uv,float range, sampler2D startex)
            {
                float2 offset = float2(_PixelSizeH * uv.x - fmod(stardata.x + _Time.y * 5, _PixelSizeH), _PixelSizeV * uv.y - fmod(stardata.y + _Time.y * 5, _PixelSizeV));
     
                float2 staruv = float2(((offset.x + 0.5f * range) / range + (int)fmod(stardata.z+_Time.y*0.5f,4)) * Star_H_Scale, (offset.y + 0.5f * range) / range);
                float4 starcolor = tex2D(startex, staruv);
                return starcolor * ((abs(offset.x) < 0.5f * range) && (abs(offset.y) < 0.5f * range)) * starcolor.w;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 offset = float2(0,0);
                float4 finalcolor = float4(0, 0, 0, 1);
                float2 staruv = float2(0, 0);
                float4 starcolor = float4(0, 0, 0, 0);
                for (int j = 0; j < min(_StarNum, MAX_STAR); j++)
                {
                    float4 stardata = _RandomStarDatas[j];
                    starcolor = CalculateStarColor(stardata, i.uv, 3, _SmallStarTex) * (stardata.w == 0) + CalculateStarColor(stardata,i.uv,3, _MediumStarTex)*(stardata.w==1) + CalculateStarColor(stardata, i.uv, 5, _BigStarTex) * (stardata.w == 2);
                    finalcolor.xyz = finalcolor.xyz + starcolor.xyz;
                }

                return finalcolor;
            }
            ENDCG
        }
    }
}
