using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{
    [SerializeField] private float m_radius = 1f;
    [SerializeField] private float m_phase  = 0f;
    [SerializeField] private float m_speed  = 1f;

    private float m_timerCounter = 0;
    private Vector3 m_originalPos;

    void Start()
    {
        m_originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_timerCounter += m_speed * Time.deltaTime; // multiply all this with some speed variable (* speed);
        float x = Mathf.Cos (m_timerCounter + m_phase);
        float y = 0;
        float z = Mathf.Sin (m_timerCounter + m_phase);
        transform.position = m_originalPos + m_radius * new Vector3 (x, y, z);
    }
}
