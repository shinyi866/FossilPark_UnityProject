Shader "Hsinpa/DisplayShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _MainTexNormal ("Main Normal Map", 2D) = "white" {}

        _DirtTex ("Dirt Texture", 2D) = "white" {}
        _DirtTexNormal ("Dirt Normal Map", 2D) = "white" {}

        _EraseTex ("EraseTex Texture", 2D) = "white" {}
        _DirtMaskTex ("Dirt Mask Texture", 2D) = "white" {}
        _DirtTransition ("Transition", Range(0, 1)) = 0

        _ColorHintPower ("Color Hint Power", Range(0, 1)) = 0
        _ColorHint ("ColorHint", Color) = (0,0,0,1)

        _LightIntensity ("Light Intensity", Range(1, 6)) = 1

        [MaterialToggle] _OverrideColor ("Override Color", Float) = 0
    }
    SubShader
    {
        Tags {  "RenderType"="Opaque" "LightMode"="ForwardBase" }

        Pass
        {
            CGPROGRAM
            #pragma multi_compile_fwdbase
            #pragma vertex vert
            #pragma fragment frag

            #include "Lighting.cginc"
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 tangent : TANGENT;
                float3 normal   : NORMAL;    // The vertex normal in model space.
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 diff : COLOR0; // diffuse lighting color
                float3 tbn[3] : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _MainTexNormal;

            sampler2D _DirtTex;
            sampler2D _DirtTexNormal;
            sampler2D _DirtMaskTex;
            sampler2D _EraseTex;

            float _DirtTransition;
            float4 _Color;
            float4 _ColorHint;
            float _ColorHintPower;
            float _LightIntensity;

            uniform float _OverrideColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                // dot product between normal and light direction for
                // standard diffuse (Lambert) lighting
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                // factor in the light color
                o.diff = nl * _LightColor0;

				float3 normal = UnityObjectToWorldNormal(v.normal);
				float3 tangent = UnityObjectToWorldNormal(v.tangent);
				float3 bitangent = cross(tangent, normal);

				o.tbn[0] = tangent;
				o.tbn[1] = bitangent;
				o.tbn[2] = normal;

                return o;
            }

            fixed FindTheLargestColor(fixed3 colorCode) {
                return max(max(colorCode.r, colorCode.g), colorCode.b);
            }

            fixed3 GetWorldNormal(float4 normalMap, float3 tbn[3]) {
				float3 surfaceNormal = tbn[2];
				return float3(tbn[0] * normalMap.r + tbn[1] * normalMap.g + tbn[2] * normalMap.b);
            }

            float ScaleNumberToRange(float target, float targetMax, float targetMin, float scaleMax, float scaleMin) {
                return  (scaleMax - scaleMin) * ((target - targetMin) / (targetMax - targetMin)) + scaleMax;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                fixed4 mainTexNormal = tex2D(_MainTexNormal, i.uv) * 2 - 1;
                fixed4 dirtTexNormal = tex2D(_DirtTexNormal, i.uv) * 2 - 1;

                fixed4 dirtTex = tex2D(_DirtTex, i.uv);
                fixed3 dirtMaskTex = tex2D(_DirtMaskTex, i.uv).rgb;
                fixed3 eraseTex = tex2D(_EraseTex, i.uv).rgb;

                fixed4 white = fixed4(1,1,1,1);
                fixed4 black = fixed4(0,0,0,1);

                fixed3 maskResult = fixed3(dirtMaskTex - eraseTex);
                maskResult = clamp(black, maskResult, white);

                //fixed4 dirtEffect = col - (col * (dirtTex + dirtTex) * FindTheLargestColor(maskResult));

                float multipier = FindTheLargestColor(maskResult);
                float colorResidual = 1 - abs(length(_Color.rgb - maskResult));
                fixed4 dirtEffect = dirtTex * multipier;

                //dirtEffect = dirtEffect * (_Color * ScaleNumberToRange(sin(_Time.z), 1, -1, 1, 0.85));

                if (colorResidual > 0.2 && _ColorHintPower < 1) {
                    dirtEffect = dirtTex * (_ColorHint * ScaleNumberToRange(sin(_Time.z), 1, -1, 1, _ColorHintPower));
                }
                
				float3 mainTexWorldNormal = GetWorldNormal(mainTexNormal, i.tbn);
                col *= dot(mainTexWorldNormal, _WorldSpaceLightPos0 );

                if ((length(dirtTex.rgb) * _OverrideColor) > 0) {
                    return colorResidual - dirtEffect * _ColorHint;
                }

                float3 dirtTexWorldNormal = GetWorldNormal(dirtTexNormal, i.tbn);
                dirtEffect *= dot(dirtTexWorldNormal, _WorldSpaceLightPos0 );
                dirtEffect = lerp(col , dirtEffect,  smoothstep(0.3, 1, multipier));                
                
                col = lerp(col , dirtEffect, _DirtTransition);
                
                col *= (_LightColor0 * _LightIntensity);

                return col;
            }
            ENDCG
        }
    }
}
