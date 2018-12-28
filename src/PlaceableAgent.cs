using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlaceableAgent : MonoBehaviour, IPointerDownHandler
{
    private bool m_selected = false;
    private NavMeshAgent m_NavMeshAgent;

    void Start()
    {
        m_NavMeshAgent = transform.parent.GetComponent<NavMeshAgent>();
    }
   
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        m_selected = !m_selected;

        if (m_selected)
        {
            GetComponent<Renderer>().material.color = new Color(1, 0, 0);
            NavMeshTouch.onTouchNavSurface += OnUpdateDestination;
            SelectionList.AddToSelection(gameObject);
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(1, 1, 1);
            NavMeshTouch.onTouchNavSurface -= OnUpdateDestination;
            SelectionList.RemoveFromSelection(gameObject);
        }
    }

    private void OnUpdateDestination(object sender, PointerEventData pointerData)
    {
        m_NavMeshAgent.SetDestination(pointerData.pointerCurrentRaycast.worldPosition);
    }
}
