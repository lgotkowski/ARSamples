using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OriginPlayer : MonoBehaviour
{
    public static GameObject m_RemotePlayer;

    void Start()
    {
        RemotePlayer.onNewRemotePlayer+= OnNewRemotePlayer;
    }

    void OnNewRemotePlayer(object sender, RemotePlayer remotePlayer)
    {
        
        if (remotePlayer.isLocalPlayer)
        {
            m_RemotePlayer = remotePlayer.gameObject;
            StartCoroutine(UpdateRemotePlayerTransform());
        }
    }

   private  IEnumerator UpdateRemotePlayerTransform()
    {
        while(m_RemotePlayer)
        {
            m_RemotePlayer.transform.position = transform.position;
            m_RemotePlayer.transform.rotation = transform.rotation;
            yield return new WaitForEndOfFrame(); 
        }
        
    }

    public static GameObject GetRemotePlayer()
    {
        return m_RemotePlayer;
    }
}
