using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(TouchObserver))]
public class Selectable : MonoBehaviour
{
    private bool m_SelectionState = false;
    private TouchObserver m_ToichObserver;

    public event EventHandler<bool> onSelectionChanged = delegate { };

    private void Start()
    {
        m_ToichObserver = GetComponent<TouchObserver>();
        m_ToichObserver.AddTouchTags(new List<TouchTag>() { TouchTag.Selectable });
    }

    public void SetSelection(bool state)
    {
        m_SelectionState = state;
        onSelectionChanged(this, m_SelectionState);
    }

    public bool IsSelected()
    {
        return m_SelectionState;
    }
}
