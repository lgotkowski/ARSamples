using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class debug : MonoBehaviour
{
    public GameObject m_Prefab;
    event EventHandler<GameObject> onSpawned;

    // Start is called before the first frame update
    void Start()
    {
        //SendSpanw(m_Prefab);
    }


    void OnSpawned(object sender, GameObject go)
    {
        Debug.Log(string.Format("OnSpawned {0}", go));
    }

    void SendSpanw(GameObject prefab)
    {
        Spawn(callBack: OnSpawned, prefab: prefab);
    }

    void Spawn(EventHandler<GameObject> callBack, GameObject prefab)
    {
        onSpawned += callBack;
        GameObject instance = Instantiate(prefab);
        instance.name = "TestObject";
        onSpawned(this, instance);
    }
}
