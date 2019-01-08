using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Linq;

[RequireComponent(typeof(Selectable))]
public class AgentNet : MonoBehaviour
{
    public NavMeshAgent m_NavMeshAgent;
    private bool m_Selected = false;
    private TouchFilter m_TouchFilter = new TouchFilter(tags: null, touchTags: new List<TouchTag>() { TouchTag.Walkable});
    private Selectable m_Selectable;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_NavMeshAgent)
            throw new Exception("Missing NavMeshAgent Component");
        m_Selectable = GetComponent<Selectable>();
        m_Selectable.onSelectionChanged += OnSelectionChanged;
    }

    void OnSelectionChanged(object sender, bool selectionState)
    {
        if (selectionState)
        {
            m_TouchFilter.onTouched += OnWalkableTouched;
        }
        else
        {
            m_TouchFilter.ClearOnTouched();
        }
    }

    void OnWalkableTouched(object sender, TouchObserverEvent touchObservEvent)
    {
        SetDestination(touchObservEvent.m_PointerData.pointerCurrentRaycast.worldPosition);
    }

    private void SetDestination(Vector3 position)
    {
        m_NavMeshAgent.SetDestination(position);
    }
}
