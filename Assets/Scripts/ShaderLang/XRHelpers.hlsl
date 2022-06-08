#if VERSION_GREATER_EQUAL(12,0)
#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
#else
#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Fullscreen.hlsl"
#endif

struct FullScreenAttributes
{
    float3 positionOS   : POSITION;
    float2 uv           : TEXCOORD0;
    #if defined(STEREO_INSTANCING_ON) || _USE_DRAW_PROCEDURAL
    uint vertexID : SV_VertexID;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
 
struct FullScreenVaryings
{
    half4 positionCS    : SV_POSITION;
    half2 uv            : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};
 
FullScreenVaryings Vertex(FullScreenAttributes input)
{
    FullScreenVaryings output;
 
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
 
    #if VERSION_GREATER_EQUAL(10,0) && defined(STEREO_INSTANCING_ON) || _USE_DRAW_PROCEDURAL
    output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
    output.uv.xy = GetFullScreenTriangleTexCoord(input.vertexID);
    #else
    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
    output.uv.xy = input.uv;
    #endif
   
    return output;
}