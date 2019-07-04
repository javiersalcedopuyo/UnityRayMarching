using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMarchingShape : MonoBehaviour
{
  enum ShapeType
  {
    CUBE,
    SPHERE,
    TORUS,
    FLOOR_PLANE,
    BACKGROUND_PLANE,
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

  [SerializeField] private ShapeType m_shape     = ShapeType.CUBE;
  [SerializeField] private Vector2   m_radii = Vector2.one;
  [SerializeField] private Color     m_color     = Color.white;
  [SerializeField] private BlendType m_blend     = BlendType.NONE;
  [SerializeField][Range(0,1)] private float m_blendStr = 0.1f;
  //TODO: Add emissiveColor
  //TODO: Add glossiness

  public int GetShapeType()       { return (int)m_shape; }
  public int GetBlendType()       { return (int)m_blend; }
  public float GetBlendStrength() { return m_blendStr; }
  public Vector3 GetPos()         { return transform.position; }
  public Vector3 GetScale()       { return transform.localScale*0.5f; }
  public Vector2 GetTorusR1R2()   { return m_radii; }
  public Color GetColor()         { return m_color; }
  public Vector3 GetRot()
  {
    Vector3 euler = transform.localEulerAngles;
    return euler * Mathf.Deg2Rad;
  }
  public Matrix4x4 GetTRMat()
  {
    return Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse;  }
}
