using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshUpdater : MonoBehaviour
{
    private NavMeshSurface m_navMeshSurface;
    private float m_UpdateInterval = 1f;

    void Start()
    {
        m_navMeshSurface = GetComponent<NavMeshSurface>();

        // temp test
        StartCoroutine(IntervalMeshUpdater());
    }

    private void OnUpdatedMesh()
    {
        m_navMeshSurface.BuildNavMesh();
    }

    private IEnumerator IntervalMeshUpdater()
    {
        while (true)
        {
            OnUpdatedMesh();
            yield return new WaitForSeconds(m_UpdateInterval);
        }
    }
}
