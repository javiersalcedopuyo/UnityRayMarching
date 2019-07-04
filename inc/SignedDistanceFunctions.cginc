// Following distance functions from http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
float FloorPlaneDistance(float3 p, float h) { return p.y - h; }
float BackgroundPlaneDistance(float3 p, float d) { return p.x - d; }

float SphereDistance(float3 p, float3 centre, float r)
{
    return distance(p, centre) - r;
}

float CubeDistance(float3 p, float3 centre, float3 s)
{
  float3 o = abs(p.xyz-centre) - s;
  float ud = length(max(o,0));
  float n = max(max(min(o.x,0),min(o.y,0)), min(o.z,0));
  return ud+n;
}

float TorusDistance(float3 p, float3 centre, float r1, float r2)
{
    float2 q = float2( length((p.xyz-centre).xz)-r1, p.xyz.y-centre.y );
    return length(q)-r2;
}

float PrismDistance(float3 p, float3 centre, float2 h)
{
    float3 q = abs(p-centre);
    return max( q.z-h.y, max(q.x*0.866025+p.y*0.5, -p.y)-h.x*0.5 );
}

float CylinderDistance(float3 p, float3 centre, float2 h)
{
    float2 d = abs( float2(length(p.xz), p.y) ) - h;
    return length( max(d, 0.0) ) + max( min(d.x,0), min(d. y,0) );
}
