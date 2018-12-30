using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LineRenderer), typeof(MeshFilter), typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class MeshArea : MonoBehaviour
{
    public GameObject m_PreviewPointPrefab;
    private string m_Name = "Area";
    [SerializeField]
    private List<GameObject> m_PreviewPoints = new List<GameObject>();

    // Components
    private MeshRenderer m_MeshRender;
    private MeshCollider m_MeshCollider;
    private MeshFilter m_MeshFilter;
    private LineRenderer m_LineRender;

    // Events
    public static event EventHandler onUpdatMeshAreaStarted;
    public static event EventHandler onUpdatMeshAreaFinished;

    void Start()
    {
        m_LineRender = GetComponent<LineRenderer>();
        m_MeshRender = GetComponent<MeshRenderer>();
        m_MeshFilter = GetComponent<MeshFilter>();
        m_MeshCollider = GetComponent<MeshCollider>();
    }

    public void SetNameIndex(int index)
    {
        m_Name = string.Format("Area_{0}", index);
    }

    public string GetName()
    {
        return m_Name;
    }

    public int LastPointInex()
    {
        return Mathf.Max(0,m_PreviewPoints.Count - 1);
    }

    public void AddPoint(Vector3 point)
    {
        GameObject previewPoint = Instantiate(m_PreviewPointPrefab, point, Quaternion.identity);
        m_PreviewPoints.Add(previewPoint);
        UpdateLineRender(true, GetPositions());
    }

    public void RemovePoint(int index)
    {
        if (m_PreviewPoints.Count > index)
        {
            GameObject previewPoint = m_PreviewPoints[index];
            RemovePoint(previewPoint);
        }
    }

    public void RemovePoint(GameObject previewPoint)
    {
        m_PreviewPoints.Remove(previewPoint);
        Destroy(previewPoint);
        UpdateLineRender(true, GetPositions());
    }

    public void RemoveAllPoints()
    {
        m_PreviewPoints.Clear();
        UpdateLineRender(true, GetPositions());
        DeletePreviewPoints();
    }

    public void GenerateMesh()
    {
        if(m_PreviewPoints.Count > 2)
        {
            if(onUpdatMeshAreaStarted != null)
                onUpdatMeshAreaStarted(this, EventArgs.Empty);
            // add a copy of first point to the end for better triangle
            m_PreviewPoints.Add(m_PreviewPoints[0]);

            // TODO: Check if list points is counter clockwise
            List<Triangle> triangles = new List<Triangle>();
            try
            {
                triangles = Triangulation.TriangulateConcavePolygon(GetPositions());
            }
            catch
            {
                m_PreviewPoints.Reverse();
                triangles = Triangulation.TriangulateConcavePolygon(GetPositions());
            }
            
            Vector3[] u_verticies = new Vector3[triangles.Count * 3];
            int[] u_triangles = new int[triangles.Count * 3];

            for (int i = 0; i < triangles.Count; i++)
            {
                u_verticies.SetValue(triangles[i].v1.position, i * 3 + 0);
                u_verticies.SetValue(triangles[i].v2.position, i * 3 + 1);
                u_verticies.SetValue(triangles[i].v3.position, i * 3 + 2);

                u_triangles.SetValue(i * 3 + 0, i * 3 + 0);
                u_triangles.SetValue(i * 3 + 1, i * 3 + 1);
                u_triangles.SetValue(i * 3 + 2, i * 3 + 2);
            }

            Mesh mesh = new Mesh();
            mesh.name = m_Name;
            mesh.Clear();
            mesh.vertices = u_verticies;
            mesh.triangles = u_triangles;

            UpdateMeshComponents(mesh);
            UpdateLineRender(false, new List<Vector3>()); // update with empty point list

            if(onUpdatMeshAreaFinished != null)
                onUpdatMeshAreaFinished(this, EventArgs.Empty);
        }
    }

    private void UpdateMeshComponents(Mesh mesh)
    {
        if (mesh == null)
        {
            m_MeshFilter.mesh.Clear();
        }

        m_MeshCollider.sharedMesh = mesh;
        m_MeshFilter.mesh = mesh;
    }

    private void UpdateLineRender(bool state, List<Vector3> points)
    {
        m_LineRender.enabled = state;
        m_LineRender.positionCount = m_PreviewPoints.Count;
        m_LineRender.SetPositions(points.ToArray());
    }

    private List<Vector3> GetPositions()
    {
        List<Vector3> points = new List<Vector3>();
        foreach(GameObject previewPoint in m_PreviewPoints)
        {
            points.Add(previewPoint.transform.position);
        }
        return points;
    }

    private void DeletePreviewPoints()
    {
        foreach (GameObject previewPoint in m_PreviewPoints)
        {
            Destroy(previewPoint);
        }
    }

    private void OnDestroy()
    {
        DeletePreviewPoints();
    }
}
