float inverseLerp(float a, float b, float v) {
    return (v - a) / (b - a);
}

float remap(float origFrom, float origTo, float targetFrom, float targetTo, float value) {
    float rel = inverseLerp(origFrom, origTo, value);
    return lerp(targetFrom, targetTo, rel);
}

float random(float2 p)
{
    return frac(sin(dot(p, float2(41, 289))) * 45758.5453) - 0.5;
}

float random01(float2 p)
{
    return frac(sin(dot(p, float2(41, 289))) * 45758.5453);
}

float2 twirlUV(float2 UV, float2 center, float strength, float2 offset) 
{
    float2 delta = UV - center;
    float angle = strength * length(delta);
    float x = cos(angle) * delta.x - sin(angle) * delta.y;
    float y = sin(angle) * delta.x + cos(angle) * delta.y;
    return float2(x + center.x + offset.x, y + center.y + offset.y);
}