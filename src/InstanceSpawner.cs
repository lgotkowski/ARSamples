using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InstanceSpawner : MonoBehaviour
{
    public GameObject m_PrefabToInstantiate;
    public List<GameObject> m_PlacedInstances = new List<GameObject>();

    public static event EventHandler<GameObject> onInstanceSpawned;

    // Start is called before the first frame update
    void Start()
    {
        NavMeshTouch.onTouchNavSurface += OnTouchNavSurface;
    }

    private void OnTouchNavSurface(object sender, PointerEventData pointerData)
    {
        if (SelectionList.IsEmpty())
        {
            Quaternion rotation = Quaternion.LookRotation(forward: pointerData.pointerCurrentRaycast.worldNormal, Vector3.up);
            SpawnInstance(pointerData.pointerCurrentRaycast.worldPosition, rotation, m_PrefabToInstantiate);
        }
    }

    private void SpawnInstance(Vector3 position, Quaternion rotation, GameObject prefab)
    {
        m_PlacedInstances.Add(Instantiate(prefab, position, rotation));
        if (onInstanceSpawned != null)
            onInstanceSpawned(sender: this, e: m_PlacedInstances[m_PlacedInstances.Count - 1]);
    }
}
