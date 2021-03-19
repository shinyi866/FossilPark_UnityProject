Shader "Hsinpa/PaintShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Power ("Power", Float) = 0
        _Range ("Range", Float) = 0

        _MousePosition ("MousePosition", Vector) = (0,0,0)
        _Color ("Paint Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float4 worldSpacePos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Power;
            float _Range;
            float4 _Color;
            float2 _MousePosition;

            fixed4 ClampColor(fixed4 targetTexture, fixed4 brusher) {
                fixed4 targetColor = targetTexture + brusher;

                fixed r = clamp(targetColor.r, brusher.r, _Color.r);
                fixed g = clamp(targetColor.g, brusher.g, _Color.g);
                fixed b = clamp(targetColor.b, brusher.b, _Color.b);
                return fixed4(r, g, b, 1);
            }

            fixed4 AddColor(fixed4 targetTexture, fixed4 brusher) {
                fixed4 targetColor = targetTexture + brusher;

                fixed r = clamp(0, targetColor.r, 1);
                fixed g = clamp(0, targetColor.g, 1);
                fixed b = clamp(0, targetColor.b, 1);

                return fixed4(r, g, b, 1);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldSpacePos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 paintTex = tex2D(_MainTex, i.uv);
                
                float difference = distance(i.uv, _MousePosition.xy);

                if (difference < _Range) {
                    float depth = 1 - (difference / _Range);
                    fixed4 finalColor = (_Color * _Power * depth);

                    paintTex = AddColor(paintTex, finalColor);
                }

                return paintTex;
            }
            ENDCG
        }
    }
}
