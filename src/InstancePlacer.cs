using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class InstancePlacer : MonoBehaviour
{
    public UnityEngine.UI.Text m_DebugText;
    public GameObject m_PrefabToInstantiate;
    public List<GameObject> m_PlacedInstances = new List<GameObject>();
    public bool m_UseNormalTouch = false;

    public static event EventHandler<GameObject> onInstancePlaced;

    void Awake()
    {
        TouchDetector.onARTouch += OnARTouch;
        TouchDetector.onTouch += OnTouch;
    }

    void OnARTouch(object sender, ARRaycastHit hit)
    {
        if (m_PrefabToInstantiate)
            PlaceInstance(hit.pose.position, hit.pose.rotation, m_PrefabToInstantiate);
    }

    void OnTouch(object sender, Touch touch)
    {
        m_DebugText.text = string.Format("Touch Pos: {0}", touch.position.ToString());
    }

    private void PlaceInstance(Vector3 position, Quaternion rotation, GameObject prefab)
    {
        m_PlacedInstances.Add(Instantiate(prefab, position, rotation));
        if (onInstancePlaced != null)
            onInstancePlaced(sender: this, e: m_PlacedInstances[m_PlacedInstances.Count - 1]);
    }
}