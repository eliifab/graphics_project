Shader "Water/WaterShader"
{
    Properties
    {
        _ColorMain("Main Color", color) = (1,1,1,1)
        _ColorDark("Dark Color", color) = (0,0,0,1)
        _DepthLevel("Depth Level", Float) = 1
        _Opacity("Water Opacity", Float) = 0.8
        _FoamTex("Foam Texture", 2D) = "white" {}
        _FoamQuantity("Foam Quantity", Float) = 1
        _FoamSize("Foam Size", Float) = 128
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

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
                float3 worldPos : TEXCOORD1;
                float3 normal : NORMAL;
                float3 viewDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float4 vertex : SV_POSITION;
            };

            struct WaveVertex {
                float3 position;
                float3 tangent;
                float3 binormal;
            };

            // Attributes declaration
            fixed4 _ColorMain;
            fixed4 _ColorDark;
            sampler2D _FoamTex;
            float _Opacity;
            float _DepthLevel;
            float _FoamQuantity;
            float _FoamSize;
            sampler2D _CameraDepthTexture;

            // MAKE A GERSTNER WAVE
            // Position | direction, steepness, lambda (wavelength), speed
            WaveVertex gerstnerWave(float4 vPos, float2 d, float st, float l, float speed) {

                WaveVertex wave;

                // Wavelength
                float k = 2 * 3.14159 / l;
                // Amplitude
                float amp = st / k;
                // Phase speed
                float c = sqrt(9.81 / k);
                // FUNCTION
                float f = k * (dot(d, float2(vPos.x, vPos.z)) - c * speed * _Time.y);

                wave.tangent = float3(
                    1 - d.x * d.x * st * sin(f),
                    d.x * st * cos(f),
                    -d.x * d.y * st * sin(f)
                );

                wave.binormal = float3(
                    -d.x * d.y * st * sin(f),
                    d.y * st * cos(f),
                    1 - d.y * d.y * st * sin(f)
                );

                wave.position = float3(
                    d.x * amp * cos(f),
                    amp * sin(f),
                    d.y * amp * cos(f)
                );

                return wave;
            }


            v2f vert (appdata v)
            {
                v2f o;
                o.uv = v.uv * _FoamSize;
                
                // Gerstner waves declaration and computation
                WaveVertex wave1 = gerstnerWave(v.vertex, float2(1,-1), 0.015, 2, 0.2);
                WaveVertex wave2 = gerstnerWave(v.vertex, float2(-1,3), 0.02, 4, 0.1);
                WaveVertex wave3 = gerstnerWave(v.vertex, float2(1, 0), 0.005, 5, 0.3);
                WaveVertex wave4 = gerstnerWave(v.vertex, float2(-1, -10), 0.002, 10, 0.05);
                WaveVertex wave5 = gerstnerWave(v.vertex, float2(5, 2), 0.01, 7, 0.01);
                WaveVertex wave6 = gerstnerWave(v.vertex, float2(1, 3), 0.008, 20, 1);
                WaveVertex wave7 = gerstnerWave(v.vertex, float2(0, 1), 0.8, 0.07, 0.01);
                WaveVertex wave8 = gerstnerWave(v.vertex, float2(-5, 1), 0.8, 0.05, 0.01);
                WaveVertex wave9 = gerstnerWave(v.vertex, float2(2, -7), 1, 0.01, 0.01);

                float3 vert = float3(v.vertex.x, 0, v.vertex.z);
                vert += 
                    wave1.position + wave2.position + wave3.position + 
                    wave4.position + wave5.position + wave6.position + 
                    wave7.position + wave8.position + wave9.position;
                vert.y *= 0.5;

                float3 tang = 
                    wave1.tangent + wave2.tangent + wave3.tangent +
                    wave4.tangent + wave5.tangent + wave6.tangent +
                    wave7.tangent + wave8.tangent + wave9.tangent;

                float3 binorm = 
                    wave1.binormal + wave2.binormal + wave3.binormal +
                    wave4.binormal + wave5.binormal + wave6.binormal +
                    wave7.binormal + wave8.binormal + wave9.binormal;

                // Fill the info to send to the fragment
                o.normal = normalize(cross(binorm, tang));
                o.worldPos = vert;
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                o.vertex = UnityObjectToClipPos(vert);
                o.screenPos = ComputeScreenPos(o.vertex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Get the depth
                float2 screenPosUV = i.screenPos.xy / i.screenPos.w;    // Scene Position
                float zDepth = i.vertex.z / i.vertex.w;                 // Pixel Depth
                float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenPosUV));    // Scene Depth
                float depthFade = saturate((depth - zDepth) / _DepthLevel);                             // Depth fade

                // Base color calculation
                float c = clamp(i.worldPos.y * 15, -0.5, 1);
                fixed4 color = lerp(_ColorDark, _ColorMain, c);

                // Fresnel for foam
                fixed4 foamTex = tex2D(_FoamTex, i.uv);
                float fresnel = saturate(1 - dot(i.viewDir, i.normal));
                fixed4 finalColor = fresnel < 0.05 * _FoamQuantity || c > 0.9 ? lerp(color, foamTex + color, fresnel) : color;

                // Final color
                return fixed4(finalColor.x, finalColor.y, finalColor.z, depthFade * _Opacity + (1 - _Opacity));
            }
            ENDCG
        }
    }
}
