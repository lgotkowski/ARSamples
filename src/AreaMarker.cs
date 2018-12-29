using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PositionListEventArgs : EventArgs
{
    public List<Vector3> m_List;

    public PositionListEventArgs(List<Vector3> positions)
    {
        m_List = positions;
    }
}

[RequireComponent(typeof(LineRenderer), typeof(MeshFilter), typeof(MeshRenderer))]
public class AreaMarker: MonoBehaviour
{
    public bool m_UseMouse = false;
    public Button m_BtnAdd;
    public Button m_BtnReset;
    public Button m_BtnFinish;
    public RectTransform m_CrossTarget;
    public Text m_Text;

    private float m_MinDistance = 0.1f;

    public List<Vector3> m_SampledPoints = new List<Vector3>();
    private static Vector3 m_NewPoint = Vector3.zero;

    private LineRenderer m_LineRender;
    private RaycastHit m_hit;
    private Ray m_Ray;

    private MeshRenderer m_MeshRender;
    private MeshCollider m_MeshCollider;
    private MeshFilter m_MeshFilter;

    public static event EventHandler<PositionListEventArgs> onFinishedSampling;
    public static event EventHandler onAddPoint;

    public static event EventHandler<Mesh> onUpdatedArea;

    void Start()
    {
        m_LineRender = GetComponent<LineRenderer>();
        m_MeshRender = GetComponent<MeshRenderer>();
        m_MeshFilter = GetComponent<MeshFilter>();
        m_MeshCollider = gameObject.AddComponent<MeshCollider>();
        m_BtnAdd.onClick.AddListener(OnAddPointClicked);
        m_BtnReset.onClick.AddListener(OnResetSamplingClicked);
        m_BtnFinish.onClick.AddListener(OnFinishSamplingClicked);

        onFinishedSampling += CreateMesh;

        m_Text.text = "Press 'Add' to start create play area.";
    }

    // Event handlers
    private void OnFinishSamplingClicked()
    {
        FinishSampling();
    }

    private void OnResetSamplingClicked()
    {
        ResetSampledPoints();
        UpdateMesh(null);
    }

    // Internal methods Start | Finish sampling
    private void OnAddPointClicked()
    {
        m_LineRender.enabled = true;
        AddSamplePoint();
        m_Text.text = "Added Sample Point...";
        if (onAddPoint != null)
            onAddPoint(this, EventArgs.Empty);
    }

    private void FinishSampling()
    {
        if (m_SampledPoints.Count > 2)
        {
            m_Text.text = "Finished Sampling!";
            // Add a copy of the first point to the end for mesh generation.
            m_SampledPoints.Add(m_SampledPoints[m_SampledPoints.Count - 1]);
            m_LineRender.enabled = false;

            if (onFinishedSampling != null)
                onFinishedSampling(this, new PositionListEventArgs(m_SampledPoints));
        }
        else
        {
            m_Text.text = "Not enought Points! (min 3)";
        }
    }

    // Sampling methods
    private Vector3 GetPointOnSurface()
    {
        if(m_UseMouse)
            m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        else
        {
            m_Ray = Camera.main.ScreenPointToRay(m_CrossTarget.position);
        }
        if (Physics.Raycast(m_Ray, out m_hit))
        {
            return m_hit.point;
        }
        return Vector3.zero;
    }

    private void ResetSampledPoints()
    {
        m_SampledPoints = new List<Vector3>();
        UpdateLineRender();
    }

    private void AddSamplePoint()
    {
        m_NewPoint = GetPointOnSurface();
        if (m_NewPoint != Vector3.zero)
        {
            if(m_SampledPoints.Count == 0)
                m_SampledPoints.Add(m_NewPoint);
            else
            {
                if ((m_SampledPoints[m_SampledPoints.Count - 1] - m_NewPoint).magnitude > m_MinDistance)
                {
                    m_SampledPoints.Add(m_NewPoint);
                    UpdateLineRender();
                }
            }
        }
    }

    private void UpdateLineRender()
    {
        m_LineRender.positionCount = m_SampledPoints.Count;
        m_LineRender.SetPositions(m_SampledPoints.ToArray());
    }

    private void UpdateMesh(Mesh mesh)
    {
        if (mesh == null)
            m_MeshFilter.mesh.Clear();

        m_MeshCollider.sharedMesh = mesh;
        m_MeshFilter.mesh = mesh;
    }

    private void CreateMesh(object sender, PositionListEventArgs posListData)
    {
        if (posListData.m_List.Count > 2)
        {
            m_Text.text = "Generating Mesh...";
            List<Triangle> triangles = Triangulation.TriangulateConcavePolygon(posListData.m_List);
            
            
            Vector3[] u_verticies = new Vector3[triangles.Count * 3];
            int[] u_triangles = new int[triangles.Count * 3];

            Debug.Log(string.Format("Triangle Count: {0}", triangles.Count));

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
            mesh.name = "PlayAreaMesh";
            mesh.Clear();
            mesh.vertices = u_verticies;
            mesh.triangles = u_triangles;
            UpdateMesh(mesh);

            if (onUpdatedArea != null)
                onUpdatedArea(this, mesh);
            m_Text.text = "Finish Generatin Mesh!";
        }
    }
}
