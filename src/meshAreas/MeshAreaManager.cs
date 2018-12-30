using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshAreaGui))]
public class MeshAreaManager : MonoBehaviour
{
    public bool m_Debug = false;
    public MeshArea m_MeshAreaPrefab;
    public RectTransform m_Target;
    public Transform m_DebugTarget;

    [SerializeField]
    private int m_SelectedIndex;
    private MeshArea m_SelectedArea;
    [SerializeField]
    private List<GameObject> m_Areas = new List<GameObject>();

    private MeshAreaGui m_MeshAreaGui;

    private void Start()
    {
        m_MeshAreaGui = GetComponent<MeshAreaGui>();

        m_MeshAreaGui.onAddAreaClicked += OnAddArea;
        m_MeshAreaGui.onRemoveAreaClicked += OnRemoveArea;
        m_MeshAreaGui.onSelectAreaClicked += OnSelectArea;

        m_MeshAreaGui.onAddPointClicked += OnAddPoint;
        m_MeshAreaGui.onRemovePointClicked += OnRemovePoint;
        m_MeshAreaGui.onGenerateMeshClicked += OnGenerateMesh;
    }

    // EventHandlers
    private void OnAddArea(object sender, object eventData)
    {
        AddArea();
        // select the new area
        SelectArea(m_Areas.Count - 1);
    }

    void OnRemoveArea(object sender, object eventData)
    {
        if (m_SelectedArea)
        {
            m_Areas.Remove(m_SelectedArea.gameObject);
            Destroy(m_SelectedArea.gameObject);
            SelectArea(Utils.GetLastIndex(m_Areas));
        }
    }

    void OnAddPoint(object sender, object eventData)
    {
        if (m_SelectedArea)
        {
            // Implement : cast point

            if (m_Debug)
                m_SelectedArea.AddPoint(m_DebugTarget.position);
            else
            {
                Vector3 point = new Vector3();
                point = Utils.GetPointOnSurfaceFromScreenPoint(m_Target.position);
                m_SelectedArea.AddPoint(point);
            }
        }
    }

    void OnRemovePoint(object sender, object eventData)
    {
        if (m_SelectedArea)
        {
            m_SelectedArea.RemovePoint(m_SelectedArea.LastPointInex());
        }
    }

    void OnGenerateMesh(object sender, object eventData)
    {
        if (m_SelectedArea)
            m_SelectedArea.GenerateMesh();
    }

    void OnSelectArea(object sender, int index)
    {
        SelectArea(index);
    }

    // Internal Methods
    private void AddArea()
    {
        MeshArea meshArea = Instantiate(m_MeshAreaPrefab, Vector3.zero, Quaternion.identity) as MeshArea;
        meshArea.SetNameIndex(m_Areas.Count);
        m_Areas.Add(meshArea.gameObject);
    }

    private void RemoveArea(int index)
    {
        m_Areas.RemoveAt(index);
    }

    private void RemoveArea(GameObject area)
    {
        m_Areas.Remove(area);
    }

    private void SelectArea(int index)
    {
        m_SelectedIndex = index;
        if (m_Areas.Count > index)
            m_SelectedArea = m_Areas[index].GetComponent<MeshArea>();
        else
            m_SelectedArea = null;
    }
}
