using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class MeshAreaShelfController : ShelfController
{
    public Transform m_TargetDebug;
    public RectTransform m_Target;
    public GameObject m_MeshAreaGuiParent;
    public Button m_BtnAddPoint;
    public Button m_BtnRemovePoint;
    public Button m_BtnGenerateMesh;

    private MeshArea m_SelectedMeshArea;

    public GameObject m_Selected;

    public static event EventHandler<bool> onEditingMeshArea = delegate { };

    // Start is called before the first frame update
    new void Start()
    {
        transform.position = Vector3.zero;
        base.Start();

        m_BtnAddPoint.onClick.AddListener(OnBtnAddPointClicked);
        m_BtnRemovePoint.onClick.AddListener(OnBtnRemovePointClicked);
        m_BtnGenerateMesh.onClick.AddListener(OnGenerateMeshClicked);

        ShowMeshAreaItemGUi(false);
    }

    protected override void OnSelectionIndexChanged(object sender, int index)
    {
        GameObject selectedGameObject = m_ShelfModel.GetSelectedGameObject();
        m_Selected = selectedGameObject;
        if (selectedGameObject)
        {
            m_SelectedMeshArea = selectedGameObject.GetComponent<MeshArea>();
            ShowMeshAreaItemGUi(true);
        }
        else
        {
            m_SelectedMeshArea = null;
            ShowMeshAreaItemGUi(false);
        }
    }

    void OnBtnAddPointClicked()
    {
        if (m_SelectedMeshArea)
        {
            if(Application.isEditor)
            {
                m_SelectedMeshArea.AddPoint(m_TargetDebug.position);
            }
            else
            {
                Vector3 point = new Vector3();
                point = Utils.GetPointOnSurfaceFromScreenPoint(m_Target.position);
                m_SelectedMeshArea.AddPoint(point);
            }
        }
    }

    void OnBtnRemovePointClicked()
    {
        if(m_SelectedMeshArea)
        {
            m_SelectedMeshArea.RemovePoint(m_SelectedMeshArea.LastPointInex());
        }
    }

    void OnGenerateMeshClicked()
    {
        if (m_SelectedMeshArea)
        {
            m_SelectedMeshArea.GenerateMesh();
            ShowMeshAreaItemGUi(false);
        }
    }

    void ShowMeshAreaItemGUi(bool state)
    {
        m_MeshAreaGuiParent.SetActive(state);
        onEditingMeshArea(this, state);
    }
}
