Shader "WaterStartingMenu/WaterShader"
{
    Properties
    {
        _ColorMain("Main Color", color) = (1,1,1,1)
        _ColorDark("Dark Color", color) = (0,0,0,1)
        _ColorDepths("Depths Color", color) = (0,0,0,1)
        _ColorShadows("Shadow Color", color) = (0.5,0.5,0.5,1)
        _DepthLevel("Depth Level", Float) = 1
        _Opacity("Water Opacity", Float) = 0.8
        _DepthIntensity("Depth Intensity", Float) = 1
        _FoamTex("Foam Texture", 2D) = "white" {}
        _FoamQuantity("Foam Quantity", Float) = 1
        _FoamSize("Foam Size", Float) = 128
        _DirLight("Directional Light", Vector) = (0,0,0)
        _Size("Size", Float) = 1

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
            fixed4 _ColorDepths;
            fixed4 _ColorShadows;
            sampler2D _FoamTex;
            float _Opacity;
            float _DepthLevel;
            float _DepthIntensity;
            float _FoamQuantity;
            float _FoamSize;
            sampler2D _CameraDepthTexture;
            fixed3 _DirLight;
            float _Size;

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
                WaveVertex wave1 = gerstnerWave(v.vertex, float2(1,-1), 0.05, 4, 0.8);
                WaveVertex wave2 = gerstnerWave(v.vertex, float2(-1, 3), 0.03, 7, 0.4);
                WaveVertex wave3 = gerstnerWave(v.vertex, float2(1, 0), 0.03, 11, 1.2);
                WaveVertex wave4 = gerstnerWave(v.vertex, float2(-1, -10), 0.01, 35, 0.1);
                WaveVertex wave5 = gerstnerWave(v.vertex, float2(5, 2), 0.02, 15, 0.3);
                WaveVertex wave6 = gerstnerWave(v.vertex, float2(1, 3), 0.007, 30, 2);
                WaveVertex wave7 = gerstnerWave(v.vertex, float2(0, 1), 0.12, 1.6, 0.1);
                WaveVertex wave8 = gerstnerWave(v.vertex, float2(-5, 1), 0.1, 0.08, 0.01);
                WaveVertex wave9 = gerstnerWave(v.vertex, float2(2, -7), 0.15, 0.01, 0.01);

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
                float depthFadeOpacity = saturate((depth - zDepth) / _DepthLevel);                      // Depth fade
                float isFar = 1 - saturate((depth - zDepth) / (_DepthLevel * 6));
                float depthFadeColor = saturate((depth - zDepth) / _DepthLevel * _DepthIntensity) * (1 - isFar);

                // Base color calculation
                float c = clamp(i.worldPos.y * 15, -0.5, 1);
                fixed4 depths = lerp(fixed4(1, 1, 1, 1), _ColorDepths, depthFadeColor);
                fixed4 color = lerp(_ColorDark, _ColorMain, c) * depths;

                // Fresnel for foam
                fixed4 foamTex = tex2D(_FoamTex, i.uv);
                //foamTex.a = isFar;
                float fresnel = saturate(1 - dot(i.viewDir, i.normal));
                fixed4 finalColor = fresnel < 0.05 * _FoamQuantity || c > 0.9 
                    ? lerp(color, foamTex + color, fresnel) 
                    : color;

                // Addition of simple shadows
                finalColor = lerp(finalColor, finalColor * _ColorShadows, max(0, dot(normalize(-_DirLight), i.normal)));

                // Final color
                return fixed4(finalColor.x, finalColor.y, finalColor.z, depthFadeOpacity * _Opacity + (1 - _Opacity));
            }
            ENDCG
        }
    }
}
