// This source code is distributed under the terms of the Bad Code License.
// You are forbidden from distributing software containing this code to end users,
// because it is bad.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMarchingShape : MonoBehaviour
{
  enum ShapeType
  {
    CUBE,
    SPHERE,
    TORUS
    // TODO: Add more shapes
  }

  enum BlendType
  {
    NONE,
    BLEND,
    CUT,
    MASK
    // TODO: Find more possible blendings?
  }

  [SerializeField] private ShapeType m_shape = ShapeType.CUBE;
  [SerializeField] private Color     m_color = Color.white;
  [SerializeField] private BlendType m_blend = BlendType.NONE;
  [SerializeField][Range(0,1)] private float m_blendStr = 0.1f;

  public int GetShapeType()       { return (int)m_shape; }
  public int GetBlendType()       { return (int)m_blend; }
  public float GetBlendStrength() { return m_blendStr; }
  public Vector3 GetPos()         { return transform.position; }
  public Vector3 GetScale()       { return transform.localScale*0.5f; }
  public Vector3 GetRot()         { return transform.rotation.eulerAngles; }
  public Color GetColor()         { return m_color; }
}
