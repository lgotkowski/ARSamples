using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class ARPlaneToggle : MonoBehaviour
{
    private MeshCollider m_MeshCollider;

    private void Start()
    {
        AreaMarker.onAddPoint += OnAddPoint;
        AreaMarker.onFinishedSampling += OnFinishAreaSampling;
        m_MeshCollider = GetComponent<MeshCollider>();
    }

    void OnAddPoint(object sender, object eventData)
    {
        m_MeshCollider.enabled = true;
    }

    void OnFinishAreaSampling(object sender, PositionListEventArgs posList)
    {
        m_MeshCollider.enabled = false;
    }
}
