Shader "Unlit/PolarGradientTwoColorShaderWithAlpha"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 0, 1, 1)
        _RangeValue ("Range Value", Range (0, 2)) = 0.5
        _MainTex ("Shape Texture", 2D) = "" {}
    }
    
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Lighting Off
            AlphaTest Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color1;
            float4 _Color2;
            float _RangeValue;
            sampler2D _MainTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float2 toPolar(float2 cartesian)
            {
                return float2(0.5 + atan2(cartesian.x, -cartesian.y) / (2 * 3.14159265359), length(cartesian)) * _RangeValue;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float alpha = tex2D(_MainTex, i.uv).a;

                // Center and scale UV coordinates
                i.uv -= 0.5;
                i.uv *= 1;

                // Convert to polar coordinates
                float2 polarCoord = toPolar(i.uv);

                fixed4 color = lerp(_Color1, _Color2, polarCoord.x * (_RangeValue * 0.25));

                color.a *= alpha;

                return color;
            }
            ENDCG
        }
    }
}
