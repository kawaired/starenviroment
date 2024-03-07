Shader "Unlit/Stars"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            float _StarSwitchTime;
            int _StarIndex;
            float _LightSwitchFac;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // sample the texture
                float starindex = (int)fmod(((_Time.y - _StarSwitchTime) > 0) * (_Time.y - _StarSwitchTime) * _LightSwitchFac,4);
                float2 staruv = float2((i.uv.x + starindex) * 0.25f,i.uv.y);
                float4 starcolor = tex2D(_MainTex, staruv);
                starcolor.xyz = starcolor.xyz * starcolor.w*0.5f;
                return starcolor;
            }
            ENDCG
        }
    }
}
