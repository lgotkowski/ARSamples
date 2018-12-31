using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class ARPlaneToggle : MonoBehaviour
{
    private MeshCollider m_MeshCollider;

    private void Start()
    {

        MeshAreaShelfController.onEditingMeshArea += OnEditingMeshArea;
        m_MeshCollider = GetComponent<MeshCollider>();
    }

    void OnEditingMeshArea(object sender, bool state)
    {
        m_MeshCollider.enabled = state;
    }
}
