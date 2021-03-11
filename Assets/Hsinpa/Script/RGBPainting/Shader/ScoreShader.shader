Shader "Unlit/ScoreShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform sampler2D _PaintedTex;
            uniform float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 maskCol = tex2D(_MainTex, i.uv);
                fixed4 paintedCol = tex2D(_PaintedTex, i.uv);
                fixed4 invertColor = fixed4(1 - _Color.r, 1 - _Color.g, 1 - _Color.b, 0);
                fixed4 minColor = fixed4(0,0,0,0);
                fixed4 maxColor = fixed4(1,1,1,1);

                maskCol = maskCol - invertColor;
                paintedCol = paintedCol - invertColor;

                fixed4 r = clamp(maskCol - paintedCol, minColor, maskCol);

                return r;
            }
            ENDCG
        }
    }
}
