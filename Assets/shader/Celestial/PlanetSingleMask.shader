Shader "Unlit/PlanetSingleMask"
{
    Properties
    {
        _MainTex ("maintex", 2D) = "white" {}
        _MaskTex("masktex",2D) = "white"{}
        _ShadowTex("shadowtex",2D) = "white"{}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent-19" "RenderType" = "Transparent" }

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
            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            sampler2D _ShadowTex;
            float4 _ShadowTex_ST;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 maincolor = tex2D(_MainTex, i.uv);
                float4 maskcolor = tex2D(_MaskTex, i.uv);
                float4 shadowcolor = tex2D(_ShadowTex, i.uv);
                float4 finalcolor = float4(maincolor.xyz * (maskcolor.w) * (1 - shadowcolor.w) * maincolor.w, maincolor.w * maskcolor.w);
                return finalcolor;
            }
            ENDCG
        }
    }
}
