// Distance functions from http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
float SphereDistance(float3 eye, float3 centre, float r) { return distance(eye, centre) - r; }

float CubeDistance(float3 eye, float3 centre, float3 size)
{
  float3 o = abs(eye-centre) -size;
  float ud = length(max(o,0));
  float n = max(max(min(o.x,0),min(o.y,0)), min(o.z,0));
  return ud+n;
}

float TorusDistance(float3 eye, float3 centre, float r1, float r2)
{
    float2 q = float2( length((eye-centre).xz)-r1, eye.y-centre.y );
    return length(q)-r2;
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
