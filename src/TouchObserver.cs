using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public enum TouchTag { Selectable, ARPlane, NavAgent, Walkable}

public class TouchObserver : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public List<TouchTag> m_TouchTags;
    public bool m_Touchable = true;
    public bool m_Dragable = true;

    public static event EventHandler<TouchObserverEvent> onObjectTouched = delegate { };
    public static event EventHandler<TouchObserverEvent> onObjectBeginDrag = delegate { };
    public static event EventHandler<TouchObserverEvent> onObjectDrag = delegate { };
    public static event EventHandler<TouchObserverEvent> onObjectEndDrag = delegate { };

    public List<TouchTag> GetTouchTags()
    {
        return m_TouchTags;
    }

    public void SetTouchTags(List<TouchTag> touchTags)
    {
        m_TouchTags = touchTags;
    }

    public void AddTouchTags(List<TouchTag> touchTags)
    {
        touchTags.Union(GetTouchTags()).ToList();
        touchTags.Distinct().ToList();
        SetTouchTags(touchTags);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(m_Touchable)
            onObjectTouched(this, new TouchObserverEvent(gameObject, eventData));    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(m_Dragable)
            onObjectBeginDrag(this, new TouchObserverEvent(gameObject, eventData));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_Dragable)
            onObjectDrag(this, new TouchObserverEvent(gameObject, eventData));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_Dragable)
            onObjectEndDrag(this, new TouchObserverEvent(gameObject, eventData));
    }
}

// Todo: implement to the EventHandler for TouchObserver and TouchFilter and all who subscribe to it.
public class TouchObserverEvent : EventArgs
{
    public TouchObserverEvent(GameObject seleted, PointerEventData pointerData)
    {
        m_Selected = seleted;
        m_PointerData = pointerData;
    }
    public GameObject m_Selected;
    public PointerEventData m_PointerData;
}
