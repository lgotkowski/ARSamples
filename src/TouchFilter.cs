using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

// Could be expanded to not only check for tags but for components as well if needed in future

public class TouchFilter 
{
    private List<string> m_Tags = new List<string>();
    private List<TouchTag> m_TouchTags = new List<TouchTag>();

    public event EventHandler<TouchObserverEvent> onTouched = delegate { };
    public event EventHandler<TouchObserverEvent> onBeginDrag = delegate { };
    public event EventHandler<TouchObserverEvent> onDrag = delegate { };
    public event EventHandler<TouchObserverEvent> onEndDrag = delegate { };

    public TouchFilter(List<string> tags, List<TouchTag> touchTags)
    {
        m_Tags = tags;
        m_TouchTags = touchTags;

        TouchObserver.onObjectTouched += OnObjectTouched;
        TouchObserver.onObjectBeginDrag += OnObjectBeginDrag;
        TouchObserver.onObjectDrag += OnObjectDrag;
        TouchObserver.onObjectEndDrag += OnObjectEndDrag;
    }

    bool ContainsTag(TouchObserver sender)
    {
        bool state;
        GameObject touchedObject = sender.gameObject;


        if(m_Tags != null && m_Tags.Contains(sender.tag))
        {
            return true;
        }

        List<TouchTag> touchTags = sender.GetTouchTags();
        if (touchTags == null)
            return false;

        foreach(TouchTag touchTag in touchTags)
        {
            if (m_TouchTags.Contains(touchTag))
                return true;
        }
        return false;
    }

    void OnObjectTouched(object sender, TouchObserverEvent touchObservEventData)
    {
        if(ContainsTag(sender as TouchObserver))
        {
            onTouched(sender, touchObservEventData);
        }
    }

    void OnObjectBeginDrag(object sender, TouchObserverEvent touchObservEventData)
    {
        if (ContainsTag(sender as TouchObserver))
        {
            onBeginDrag(sender, touchObservEventData);
        }
    }

    void OnObjectDrag(object sender, TouchObserverEvent touchObservEventData)
    {
        if (ContainsTag(sender as TouchObserver))
        {
            onDrag(sender, touchObservEventData);
        }
    }

    void OnObjectEndDrag(object sender, TouchObserverEvent touchObservEventData)
    {
        if (ContainsTag(sender as TouchObserver))
        {
            onEndDrag(sender, touchObservEventData);
        }
    }

    public void ClearOnTouched()
    {
        onTouched = delegate { };
    }

    public void ClearOnBeginDrag()
    {
        onTouched = delegate { };
    }

    public void ClearOnDrag()
    {
        onTouched = delegate { };
    }

    public void ClearOnEndDrag()
    {
        onTouched = delegate { };
    }
}
