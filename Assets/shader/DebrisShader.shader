Shader "Unlit/DebrisShader"
{
    Properties
    {
        _DebrisTex("debristex", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent-21" "RenderType" = "Transparent" }

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

            sampler2D _DebrisTex;
            float4 _DebrisTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // sample the texture
                float4 debriscolor = tex2D(_DebrisTex, i.uv);
                return debriscolor;
            }
            ENDCG
        }
    }
}
