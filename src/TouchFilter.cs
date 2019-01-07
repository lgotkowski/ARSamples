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

    public event EventHandler<PointerEventData> onTouched = delegate { };
    public event EventHandler<PointerEventData> onBeginDrag = delegate { };
    public event EventHandler<PointerEventData> onDrag = delegate { };
    public event EventHandler<PointerEventData> onEndDrag = delegate { };

    public TouchFilter(List<string> tags, List<TouchTag> touchTags)
    {
        m_Tags = tags;
        m_TouchTags = touchTags;

        TouchObserver.onObjectTouched += OnObjectTouched;
        TouchObserver.onObjectBeginDrag += OnObjectBeginDrag;
        TouchObserver.onObjectDrag += OnObjectDrag;
        TouchObserver.onObjectEndDrag += OnObjectEndDrag;
    }

    bool ContainsTag(GameObject sender)
    {
        bool state;
        GameObject touchedObject = sender;


        if(m_Tags != null && m_Tags.Contains(sender.tag))
        {
            return true;
        }

        List<TouchTag> touchTags = sender.GetComponent<TouchObserver>().GetTouchTags();
        if (touchTags == null)
            return false;

        foreach(TouchTag touchTag in touchTags)
        {
            if (m_TouchTags.Contains(touchTag))
                return true;
        }
        return false;
    }

    void OnObjectTouched(object sender, PointerEventData eventData)
    {
        if(ContainsTag(sender as GameObject))
        {
            onTouched(sender, eventData);
        }
    }

    void OnObjectBeginDrag(object sender, PointerEventData eventData)
    {
        if (ContainsTag(sender as GameObject))
        {
            onBeginDrag(sender, eventData);
        }
    }

    void OnObjectDrag(object sender, PointerEventData eventData)
    {
        if (ContainsTag(sender as GameObject))
        {
            onDrag(sender, eventData);
        }
    }

    void OnObjectEndDrag(object sender, PointerEventData eventData)
    {
        if (ContainsTag(sender as GameObject))
        {
            onEndDrag(sender, eventData);
        }
    }
}
