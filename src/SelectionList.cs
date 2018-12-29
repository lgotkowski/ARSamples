using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SelectionListEventArgs : EventArgs
{
    public List<GameObject> m_List;

    public SelectionListEventArgs(List<GameObject> selection)
    {
        m_List = selection;
    }
}

public static class SelectionList 
{
    
    static List<GameObject> m_SelectionList = new List<GameObject>();
    public static event EventHandler<SelectionListEventArgs> onSelectionChanged;
    //public static event EventHandler onSelectionChanged = delegate { };

    public static void AddToSelection(GameObject instance)
    {
        if (!m_SelectionList.Contains(instance))
        {
            m_SelectionList.Add(instance);
            RaiseSelectionChanged();
        }
    }

    public static void RemoveFromSelection(GameObject instance)
    {
        if(m_SelectionList.Contains(instance))
        {
            m_SelectionList.Remove(instance);
            RaiseSelectionChanged();
        }
    }

    private static void RaiseSelectionChanged()
    {
        //onSelectionChanged(m_SelectionList, m_SelectionList);
        if(onSelectionChanged != null)
            onSelectionChanged(null, new SelectionListEventArgs(m_SelectionList));
        //onSelectionChanged(null, new SelectionListEventArgs() { m_List = m_SelectionList });
    }

    public static bool IsEmpty()
    {
        if (m_SelectionList.Count == 0)
            return true;
        return false;
    }
}
