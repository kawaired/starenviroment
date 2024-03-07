Shader "Unlit/Planet"
{
    Properties
    {
        _BaseTex ("basetex", 2D) = "white" {}
        _LiquidTex("liquidtex",2D) = "white"{}
        _ShadowTex("shadowtex",2D) = "white"{}
        _DynTex("dyntex",2D) = "white"{}
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
                float4 coord3:TEXCOORD2;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _BaseTex;
            float4 _BaseTex_ST;
            sampler2D _LiquidTex;
            float4 _LiquidTex_ST;
            sampler2D _ShadowTex;
            float4 _ShadowTex_ST;
            sampler2D _DynTex;
            float4 _DynTex_ST;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 basecolor = tex2D(_BaseTex,i.uv);
                float4 liquidcolor = tex2D(_LiquidTex,i.uv);
                float4 shadowcolor = tex2D(_ShadowTex, i.uv);
                float4 dyncolor = tex2D(_DynTex, i.uv);
                return float4((basecolor.xyz * dyncolor.w + (1 - dyncolor.w) * liquidcolor.xyz) * (1 - shadowcolor.w) * basecolor.w, basecolor.w) + float4(shadowcolor.xyz * (1 - basecolor.w), (1 - basecolor.w) * shadowcolor.w);
            }
            ENDCG
        }
    }
}
