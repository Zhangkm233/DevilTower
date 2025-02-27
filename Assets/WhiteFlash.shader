Shader "Custom/WhiteFlash"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WhiteAmount ("White Amount", Range(0, 1)) = 0 // 控制白色混合程度
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
            float _WhiteAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // 将原色与白色按比例混合
                col.rgb = lerp(col.rgb, fixed3(2,2,2), _WhiteAmount);
                return col;
            }
            ENDCG
        }
    }
}