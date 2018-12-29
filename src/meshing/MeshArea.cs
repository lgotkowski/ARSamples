using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(MeshFilter), typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class MeshArea : MonoBehaviour
{
    private List<Vector3> m_Points = new List<Vector3>();

    // Components
    private MeshRenderer m_MeshRender;
    private MeshCollider m_MeshCollider;
    private MeshFilter m_MeshFilter;
    private LineRenderer m_LineRender;

    void Start()
    {
        m_LineRender = GetComponent<LineRenderer>();
        m_MeshRender = GetComponent<MeshRenderer>();
        m_MeshFilter = GetComponent<MeshFilter>();
        m_MeshCollider = GetComponent<MeshCollider>();
    }

    public void AddPoint()
    { }

    public void RemovePoint(int index)
    { }

    public void RemoveAllPoints()
    { }

    public void GenerateMesh()
    { }
}
