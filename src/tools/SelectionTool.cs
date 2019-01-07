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

    // sender is the  GameObject and the bool indicats state if it was selected or deselection;
    public static event EventHandler<bool> onSelectionChanged = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    void OnTouchSelectable(object sender, PointerEventData eventData)
    {
        GameObject selected = sender as GameObject;
        if(m_SelectionList.Contains(selected))
        {
            RemoveSelected(selected);
            onSelectionChanged(selected, false);
        }
        else
        {
            AddSelected(selected);
            onSelectionChanged(selected, true);
        }
    }

    void AddSelected(GameObject seleced)
    {
        m_SelectionList.Add(seleced);
    }

    void RemoveSelected(GameObject seleced)
    {
        m_SelectionList.Remove(seleced);
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
        m_SelectionList = new List<GameObject>();
        m_TouchFilter.onTouched -= OnTouchSelectable;
        m_TouchFilter = null;
    }
}