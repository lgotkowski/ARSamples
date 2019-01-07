using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShelfModel : MonoBehaviour
{
    private int m_SelectedIndex = 0;
    private List<GameObject> m_ShelfItems = new List<GameObject>();

    public GameObject m_SheflItemPrefab;
    public Transform m_ShelfItemParent;

    public event EventHandler<int> onSelectionIndexChanged = delegate { };

    // Event Handler
    public void OnAddShelfItem(object sender, object eventData)
    {
        AddShelfItem();
    }

    public void OnRemoveShelfItem(object sender, int index)
    {
        RemoveShelfItem(index);
    }

    public void OnSelectShelfItem(object sender, int index)
    {
        SelectShelfItem(index);
    }

    // Internal Methods
    void AddShelfItem()
    {
        GameObject shelfItem = Instantiate(m_SheflItemPrefab, m_ShelfItemParent);
        shelfItem.transform.position = Vector3.zero;
        m_ShelfItems.Add(shelfItem);
    }

    void RemoveShelfItem(int index)
    {
        if(m_ShelfItems.Count > index)
        {
            GameObject shelfItem = m_ShelfItems[index];
            m_ShelfItems.Remove(shelfItem);
            Destroy(shelfItem);
        }
    }

    void SelectShelfItem(int index)
    {
        if (m_ShelfItems.Count >= index)
        {
            m_SelectedIndex = index;
            onSelectionIndexChanged(this, m_SelectedIndex);
        }
    }

    // Helper Methods
    public int GetSelectedIndex()
    {
        return m_SelectedIndex;
    }

    public GameObject GetSelectedGameObject()
    {
        if (m_ShelfItems.Count > m_SelectedIndex)
            return m_ShelfItems[m_SelectedIndex];
        return null;
    }

    public int GetLastIndex()
    {
        return Utils.GetLastIndex(m_ShelfItems);
    }
}
