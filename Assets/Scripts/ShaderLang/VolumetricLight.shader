Shader "Unlit/VolumetricLight"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Scattering("Scattering", float) = 0
        _Steps("Steps", float) = 25
        _JitterVolumetric("JitterVolumetric", float) = 0
        _MaxDistance("MaxDistance", float) = 75
        _Intensity("Intensity", float) = 0
        _GaussSamples("GaussSamples", int) = 0
        _GaussAmount("GaussAmount", float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent"
        "Queue" = "Transparent"}
        Cull Off 
        ZWrite Off 
        ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #pragma multi_compile _ _USE_DRAW_PROCEDURAL

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "./GeneralFunctions.hlsl"

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
            float3 _SunDirection;
            float3 _SunColor;

            float _Scattering;
            float _Steps;
            float _MaxDistance;
            float _JitterVolumetric;
            float _Intensity;

            //This function will tell us if a certain point in world space coordinates is in light or shadow of the main light
            float ShadowAtten(float3 worldPosition)
            {
                return MainLightRealtimeShadow(TransformWorldToShadowCoord(worldPosition));
            }

            //Unity already has a function that can reconstruct world space position from depth
            float3 GetWorldPos(float2 uv)
            {
                #if UNITY_REVERSED_Z
                    float depth = SampleSceneDepth(uv);
                #else
                                // Adjust z to match NDC for OpenGL
                    float depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(uv));
                #endif
                    return ComputeWorldSpacePosition(uv, depth, UNITY_MATRIX_I_VP);
            }

            // Mie scaterring approximated with Henyey-Greenstein phase function.
            float ComputeScattering(float lightDotView)
            {
                float result = 1.0f - _Scattering * _Scattering;
                result /= (4.0f * PI * pow(1.0f + _Scattering * _Scattering - (2.0f * _Scattering) * lightDotView, 1.5f));
                return result;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformWorldToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float frag(v2f i) : SV_Target
            {
                float3 worldPos = GetWorldPos(i.uv);
                float3 startPosition = _WorldSpaceCameraPos;
                float3 rayVector = worldPos - startPosition;
                float3 rayDirection = normalize(rayVector);
                float rayLength = length(rayVector);

                rayLength = min(rayLength, _MaxDistance);
                worldPos = startPosition + rayDirection * rayLength;

                float stepLength = rayLength / _Steps;
                float3 step = rayDirection * stepLength;

                float rayStartOffset = random01(i.uv) * stepLength * _JitterVolumetric / 100;
                float3 currentPosition = startPosition + rayStartOffset * rayDirection;
                float accumFog = 0;
                for (float i = 0; i < _Steps - 1; i++)
                {
                    float shadowMapValue = ShadowAtten(currentPosition);

                    if (shadowMapValue > 0)
                    {
                        float kernelColor = ComputeScattering(dot(rayDirection, _SunDirection));
                        accumFog += kernelColor;
                    }

                    currentPosition += step;
                }

                accumFog /= _Steps;

                return accumFog;
            }
            ENDHLSL
        }

        Pass
        {
            Name "Gaussian Blur x"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" 

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformWorldToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            int _GaussSamples;
            float _GaussAmount;

            static const real gauss_filter_weights[] = { 0.14446445, 0.13543542, 0.11153505, 0.08055309, 0.05087564, 0.02798160, 0.01332457, 0.00545096 };
            #define BLUR_DEPTH_FALLOFF 100.0

            float frag(v2f i) : SV_Target
            {
                float col = 0;
                float accumResult = 0;
                float accumWeights = 0;
                float depthCenter;
                #if UNITY_REVERSED_Z
                    depthCenter = SampleSceneDepth(i.uv);
                #else
                    // Adjust z to match NDC for OpenGL
                    depthCenter = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(i.uv));
                #endif

                for (float j = -_GaussSamples; j <= _GaussSamples; j++) 
                {
                    float2 uv = i.uv + float2(j * _GaussAmount / 1000, 0);
                    float kernelSample = tex2D(_MainTex, uv);
                    float depthKernel;

                    #if UNITY_REVERSED_Z
                        depthKernel = SampleSceneDepth(i.uv);
                    #else
                        // Adjust z to match NDC for OpenGL
                        depthKernel = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(i.uv));
                    #endif

                    float depthDiff = abs(depthKernel - depthCenter);
                    float r2 = depthDiff * BLUR_DEPTH_FALLOFF;
                    float g = exp(-r2 * r2);
                    float weight = g * gauss_filter_weights[abs(j)];

                    accumResult += weight * kernelSample;
                    accumWeights += weight;
                }

                col = accumResult / accumWeights;
                return col;
            }

            ENDHLSL
        }    

        Pass
        {
            Name "Gaussian Blur y"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" 

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformWorldToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            int _GaussSamples;
            float _GaussAmount;

            static const real gauss_filter_weights[] = { 0.14446445, 0.13543542, 0.11153505, 0.08055309, 0.05087564, 0.02798160, 0.01332457, 0.00545096 };
            #define BLUR_DEPTH_FALLOFF 100.0
                
            float frag(v2f i) : SV_Target
            {
                float col = 0;
                float accumResult = 0;
                float accumWeights = 0;
                float depthCenter;
                #if UNITY_REVERSED_Z
                    depthCenter = SampleSceneDepth(i.uv);
                #else
                    // Adjust z to match NDC for OpenGL
                    depthCenter = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(i.uv));
                #endif

                for (float j = -_GaussSamples; j <= _GaussSamples; j++)
                {
                    float2 uv = i.uv + float2(0, j * _GaussAmount / 1000);
                    float kernelSample = tex2D(_MainTex, uv);
                    float depthKernel;

                    #if UNITY_REVERSED_Z
                        depthKernel = SampleSceneDepth(i.uv);
                    #else
                        // Adjust z to match NDC for OpenGL
                        depthKernel = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(i.uv));
                    #endif

                    float depthDiff = abs(depthKernel - depthCenter);
                    float r2 = depthDiff * BLUR_DEPTH_FALLOFF;
                    float g = exp(-r2 * r2);
                    float weight = g * gauss_filter_weights[abs(j)];

                    accumResult += weight * kernelSample;
                    accumWeights += weight;
                }

                col = accumResult / accumWeights;
                return col;
            }

            ENDHLSL
        }

        Pass
        {
            Name "Compositing"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformWorldToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            TEXTURE2D(_VolumetricTexture);
            SAMPLER(sampler_VolumetricTexture);
            TEXTURE2D(_LowResDepth);
            SAMPLER(sampler_LowResDepth);
            float4 _SunColor;
            float _Intensity;

            float3 frag(v2f i) : SV_Target
            {
                float col = 0;
                int offset = 0;
                float d0 = SampleSceneDepth(i.uv);

                // Depth in the adjacent lower res pixels
                float d1 = _LowResDepth.Sample(sampler_LowResDepth, i.uv, int2(0, 1)).x;
                float d2 = _LowResDepth.Sample(sampler_LowResDepth, i.uv, int2(0, -1)).x;
                float d3 = _LowResDepth.Sample(sampler_LowResDepth, i.uv, int2(1, 0)).x;
                float d4 = _LowResDepth.Sample(sampler_LowResDepth, i.uv, int2(-1, 0)).x;

                // Difference between two values
                d1 = abs(d0 - d1);
                d2 = abs(d0 - d2);
                d3 = abs(d0 - d3);
                d4 = abs(d0 - d4);

                float dmin = min(min(d1, d2), min(d3, d4));

                if (dmin == d1)
                    offset = 0;

                else if (dmin == d2)
                    offset = 1;

                else if (dmin == d3)
                    offset = 2;

                else  if (dmin == d4)
                    offset = 3;

                // Sampling the chosen fragment
                switch(offset)
                {
                    case 0:
                        col = _VolumetricTexture.Sample(sampler_VolumetricTexture, i.uv, int2(0, 1));
                        break;
                    case 1:
                        col = _VolumetricTexture.Sample(sampler_VolumetricTexture, i.uv, int2(0, -1));
                        break;
                    case 2:
                        col = _VolumetricTexture.Sample(sampler_VolumetricTexture, i.uv, int2(1, 0));
                        break;
                    case 3:
                        col = _VolumetricTexture.Sample(sampler_VolumetricTexture, i.uv, int2(-1, 0));
                        break;
                    default:
                        col = _VolumetricTexture.Sample(sampler_VolumetricTexture, i.uv);
                        break;
                }

                float3 finalShaft = saturate(col) * normalize(_SunColor) * _Intensity;

                float3 screen = tex2D(_MainTex, i.uv);

                return screen + finalShaft;
            }

            ENDHLSL
        }

        Pass
        {
            Name "SampleDepth"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformWorldToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float frag(v2f i) : SV_Target
            {
                #if UNITY_REVERSED_Z
                    real depth = SampleSceneDepth(i.uv);
                #else
                    // Adjust z to match NDC for OpenGL
                    real depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(i.uv));
                #endif
                return depth;
            }

        ENDHLSL
        }
    }
}
