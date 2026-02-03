//float Test(float t) 
//{
//    float dist = 1.0;
//    return dist;
//}

//
void RoundedRectSDF_float(float2 UV, float2 Size, float Radius, out float Distance)
{
    float2 p = (UV - 0.5) * 2.0;
    float2 q = abs(p) - Size * 0.5 + Radius;
    Distance = length(max(q, 0.0)) - Radius;
}