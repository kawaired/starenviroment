Shader "Unlit/HorizonPlanetShader"
{
    Properties
    {
        _MainTex("maintex",2D) = "white" {}
        //_ShadowTex("shadowtex",2D) = "white"{}
        _MaskTex("masktex",2D) = "white"{}
        _LiquidTex("liquidetex",2D) = "white"{}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
            //sampler2D _ShadowTex;
            //float4 _ShadowTex_ST;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            sampler2D _LiquidTex;
            float4 _LiquidTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 maincolor = tex2D(_MainTex, i.uv);
                //float4 shadowcolor = tex2D(_ShadowTex, i.uv);
                float4 maskcolor = tex2D(_MaskTex, i.uv);
                float4 liquidcolor = tex2D(_LiquidTex, i.uv);
                float4 finalcolor = float4(maincolor.xyz * maskcolor.w + liquidcolor.xyz * (1 - maskcolor.w), maincolor.w);
                //return liquidcolor;
                //return maincolor;
                //return maskcolor.w;
                return finalcolor;
                //  return float4(maincolor.xyz * maincolor.w + atmospherecolor.xyz * atmospherecolor.w, maincolor.w + atmospherecolor.w);
                return maincolor;
            }
            ENDCG
        }
    }
}
