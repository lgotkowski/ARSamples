using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;


public class RemotePlayer : NetworkBehaviour
{

    public static event EventHandler<RemotePlayer> onNewRemotePlayer = delegate { };
    public event EventHandler<GameObject> onSpawnedInstance = delegate { };
    public event EventHandler<GameObject> onTransformedInstance = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        onNewRemotePlayer(this, this);
    }

    /// Network spawn methods
    public void NetworkInstantiate(EventHandler<GameObject> callBack, string folderName, GameObject prefab, Vector3 position, Quaternion rotation) //, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // prefab musst be in the Resource folder!
        string prefabPath = string.Format("{0}/{1}", folderName, prefab.name); //AssetDatabase.GetAssetPath(prefab);
        onSpawnedInstance += callBack;
        CmdInstantiate(this.gameObject, position, rotation, prefabPath);
        //callBack();
        //EventHandler test = new EventHandler(callBack);

    }

    [Command]
    void CmdInstantiate(GameObject client, Vector3 position, Quaternion rotation, string prefabPath)
    {
        GameObject prefab = Resources.Load(prefabPath) as GameObject; // (GameObject)AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
        GameObject instance = Instantiate(prefab, position, rotation);  //new GameObject("DummpyInstance");
        //ClientScene.RegisterPrefab(prefab);
        NetworkServer.Spawn(instance);

        NetworkInstanceId netId = instance.GetComponent<NetworkIdentity>().netId;

        RemotePlayer clientRemotePlayer = client.GetComponent<RemotePlayer>();
        clientRemotePlayer.RpcOnInstantiated(client, netId);
    }

    [ClientRpc]
    void RpcOnInstantiated(GameObject client, NetworkInstanceId netId)
    {
        // maybe check if I am the correct client?
        GameObject instance = ClientScene.FindLocalObject(netId);
        onSpawnedInstance(this, instance);
    }

    // Network transform methods
    public void NetworkTransformInstance(EventHandler<GameObject> callback, GameObject instance, Vector3 position, Quaternion rotation)
    {
        onTransformedInstance += callback;
        NetworkInstanceId netId = GetInstanceNetId(instance);
        CmdTransformInstance(this.gameObject, netId, position, rotation);
    }

    [Command]
    void CmdTransformInstance(GameObject client, NetworkInstanceId instanceNetId, Vector3 position, Quaternion rotation)
    {
        GameObject instamce = NetworkServer.FindLocalObject(instanceNetId);
        instamce.transform.position = position;
        instamce.transform.rotation = rotation;

        Debug.Log(string.Format("instance: {0} | Position: {1} | rotation: {2}", instamce, position, rotation));
        //RemotePlayer clientRemotePlayer = client.GetComponent<RemotePlayer>();
        RpcOnTransformedInstance(client, instanceNetId);
        //clientRemotePlayer.RpcOnTransformedInstance(client, instanceNetId);
    }

    [ClientRpc]
    void RpcOnTransformedInstance(GameObject client, NetworkInstanceId netId)
    {
        GameObject instance = ClientScene.FindLocalObject(netId);
        onTransformedInstance(this, instance);
        Debug.Log("RpcOnTransformedInstance");
    }

    // util methods
    public NetworkInstanceId GetInstanceNetId(GameObject instance)
    {
        return instance.GetComponent<NetworkIdentity>().netId;
    }
}
