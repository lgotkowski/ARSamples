using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class Utils 
{
    private static RaycastHit m_Hit;
    private static RaycastHit m_HitNull;
    private static Ray m_Ray;
    private static Camera m_Camera;

    public static RaycastHit GetHitFromScreenPoint(Vector2 screenPoint)
    {
        if (m_Camera == null)
            m_Camera = Camera.main;
        m_Ray = m_Camera.ScreenPointToRay(screenPoint);
        Physics.Raycast(m_Ray, out m_Hit);
        return m_Hit;
        //if (Physics.Raycast(m_Ray, out m_Hit))
        //    return m_Hit;
        //else
        //    return m_HitNull;
    }

    public static Vector3 GetPointOnSurfaceFromScreenPoint(Vector2 screenPoint)
    {
        return GetHitFromScreenPoint(screenPoint).point;
    }

    public static int GetLastIndex<T>(List<T> list)
    {
        return Mathf.Max(0, list.Count - 1);
    }

    /*
    // Editor Script Only
    public List<string> GetPrefabsResourcePaths(List<GameObject> prefabs)
    {
        List<string> paths = new List<string>();
        foreach(GameObject prefab in prefabs)
        {
            
        }
        return paths;
    }

    string GetPRefabResourcePath(GameObject prefab)
    {
        string fullPathWithExtenstion = AssetDatabase.GetAssetPath(prefab);
        string pathWithExtension = fullPathWithExtenstion.Replace("Assets/Resources/", "");
        return pathWithExtension.Replace(".prefab", "");
    }
    */
}
