using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    enum Axis { X,Y,Z }

    [SerializeField] private Axis  m_axis  = Axis.X;
    [SerializeField] private float m_speed = .1f;

    private float   m_totalAngle = 0f;
    private Vector3 m_rotationAxis;

    void Update()
    {
        switch (m_axis)
        {
            case Axis.X:
                m_rotationAxis = Vector3.right;
                break;
            case Axis.Y:
                m_rotationAxis = Vector3.up;
                break;
            case Axis.Z:
                m_rotationAxis = Vector3.forward;
                break;
        }
        m_totalAngle += Time.deltaTime * m_speed;
        transform.RotateAround(transform.position, m_rotationAxis, m_totalAngle);
    }
}
