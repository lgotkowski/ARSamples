using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class SelectionTool : AbstractTool
{
    public override ToolNames m_ToolName { get { return ToolNames.Selection; } }
    public List<TouchTag> m_TouchTags = new List<TouchTag>() { TouchTag.Selectable };
    private TouchFilter m_TouchFilter;

    public List<GameObject> m_SelectionList;

    // sender is the GameObject and the bool indicats selection state if it was selected or deselection;
    public static event EventHandler<SelectionEvent> onSelectionChanged = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    void OnTouchSelectable(object sender, TouchObserverEvent touchObservEventData)
    {
        GameObject selected = touchObservEventData.m_Selected;
        bool selectionState;

        if(m_SelectionList.Contains(selected))
        {
            selectionState = false;
            RemoveSelected(selected);
        }
        else
        {
            selectionState = true;
            AddSelected(selected);
        }
        onSelectionChanged(this, new SelectionEvent(m_SelectionList));
    }

    void AddSelected(GameObject selected)
    {
        m_SelectionList.Add(selected);
        SetSelectionstate(selected, true);
    }

    void RemoveSelected(GameObject selected)
    {
        m_SelectionList.Remove(selected);
        SetSelectionstate(selected, false);
    }

    void ClearSelection()
    {
        for (int i = 0; i < m_SelectionList.Count; i++)
        {
            SetSelectionstate(m_SelectionList[i], false);
        }
        m_SelectionList = new List<GameObject>();
        onSelectionChanged(this, new SelectionEvent(m_SelectionList));
    }

    void SetSelectionstate(GameObject selectedObject, bool selectionState)
    {
        Selectable selectable = selectedObject.GetComponent<Selectable>();
        selectable.SetSelection(selectionState);
    }

    protected override void OnToolActivated()
    {
        base.OnToolActivated();
        m_SelectionList = new List<GameObject>();
        m_TouchFilter = new TouchFilter(tags: null, touchTags: m_TouchTags);
        m_TouchFilter.onTouched += OnTouchSelectable;
    }

    protected override void OnToolDeactivated()
    {
        base.OnToolDeactivated();
        ClearSelection();
        m_TouchFilter.onTouched -= OnTouchSelectable;
        m_TouchFilter = null;
    }
}

public class SelectionEvent : EventArgs
{
    public SelectionEvent(List<GameObject> currentSelection)
    {
        m_CurrentSelection = currentSelection;
    }

    public List<GameObject> m_CurrentSelection;
}