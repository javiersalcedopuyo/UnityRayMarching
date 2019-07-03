// Following distance functions from http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
float SphereDistance(float3 eye, float3 centre, float4x4 invTR, float s)
{
    return distance( mul(invTR,float4(eye,1)), float4(centre,1) ) - s;
}

float CubeDistance(float3 eye, float3 centre, float4x4 invTR, float3 s)
{
  float4 p = mul( invTR, float4(eye,1));
  float3 o = abs(p.xyz-centre) - s;
  float ud = length(max(o,0));
  float n = max(max(min(o.x,0),min(o.y,0)), min(o.z,0));
  return ud+n;
}

float TorusDistance(float3 eye, float3 centre, float r1, float r2, float4x4 invTR)
{
    float o = 0;
    float4 p = mul( invTR, float4(eye,1) );
    float2 q = float2( length((p.xyz-centre).xz)-r1, p.xyz.y-centre.y );
    o = length(q)-r2;
    return o;
}

float PrismDistance(float3 eye, float3 centre, float2 h)
{
    float3 q = abs(eye-centre);
    return max( q.z-h.y, max(q.x*0.866025+eye.y*0.5, -eye.y)-h.x*0.5 );
}

float CylinderDistance(float3 eye, float3 centre, float2 h)
{
    float2 d = abs( float2(length(eye.xz), eye.y) ) - h;
    return length( max(d, 0.0) ) + max( min(d.x,0), min(d. y,0) );
}
