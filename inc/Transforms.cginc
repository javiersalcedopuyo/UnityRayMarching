float2x2 GetRotMat(float angle)
{
  float s = sin(angle);
  float c = cos(angle);
  return float2x2(c, -s, s, c);
}

float3 Rotate(float3 p, float3 anglesPerAxis)
{ // TODO: Add quaternions support
  p.yz = mul(GetRotMat(-anglesPerAxis.x), p.yz);
  p.xz = mul(GetRotMat(anglesPerAxis.y), p.xz);
  p.xy = mul(GetRotMat(-anglesPerAxis.z), p.xy);
  return p;
}