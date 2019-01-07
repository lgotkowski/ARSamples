using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public enum TouchTag { Selectable, ARPlane}

public class TouchObserver : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public List<TouchTag> m_TouchTags;
    public bool m_Touchable = true;
    public bool m_Dragable = true;

    public static event EventHandler<PointerEventData> onObjectTouched = delegate { };
    public static event EventHandler<PointerEventData> onObjectBeginDrag = delegate { };
    public static event EventHandler<PointerEventData> onObjectDrag = delegate { };
    public static event EventHandler<PointerEventData> onObjectEndDrag = delegate { };

    public List<TouchTag> GetTouchTags()
    {
        return m_TouchTags;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(m_Touchable)
            onObjectTouched(gameObject, eventData);    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(m_Dragable)
            onObjectBeginDrag(gameObject, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_Dragable)
            onObjectDrag(gameObject, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_Dragable)
            onObjectEndDrag(gameObject, eventData);
    }
}
