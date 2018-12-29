using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshAreaGenerator : MonoBehaviour
{
    public GameObject m_MeshAreaPrefab;

    private List<GameObject> m_Areas = new List<GameObject>();

    public GameObject AddArea()
    {
        m_Areas.Add(Instantiate(m_MeshAreaPrefab, Vector3.zero, Quaternion.identity));
        return m_Areas[m_Areas.Count - 1];
    }

    public void RemoveArea(int index)
    {
        m_Areas.RemoveAt(index);
    }

    public void RemoveArea(GameObject area)
    {
        m_Areas.Remove(area);
    }

    public GameObject SelectArea(int index)
    {
        return m_Areas[index];
    }

    
}
