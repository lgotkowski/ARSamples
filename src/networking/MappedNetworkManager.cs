using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MappedNetworkManager : NetworkManager
{
    public Button m_Btn1;
    public Button m_Btn2;

    public GameObject m_PlayerAR;
    public GameObject m_PlayerDefault;

    public static string m_Platform;
    public Text m_DebugText;

    public int chosenCharacter = 0;

    //subclass for sending network messages
    public class NetworkMessage : MessageBase
    {
        public int chosenClass;
    }

    private void Start()
    {
        m_Btn1.onClick.AddListener(OnBtn1Clicked);
        m_Btn2.onClick.AddListener(OnBtn2Clicked);

        m_Platform = Application.platform.ToString();
        DebugScreen(m_Platform);
    }


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        string text = string.Format("Host: {0} | Conn: {1}", conn.hostId, conn.connectionId);
        DebugScreen(text);

        NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
        int selectedClass = message.chosenClass;
        Debug.Log("server add with message " + selectedClass);

        GameObject player;
        if (selectedClass == 0)
        {
            player = Instantiate(m_PlayerDefault);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        if (selectedClass == 1)
        {
            player = Instantiate(m_PlayerAR);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        NetworkMessage test = new NetworkMessage();
        test.chosenClass = chosenCharacter;

        ClientScene.AddPlayer(conn, 0, test);
    }

    public void OnBtn1Clicked()
    {
        chosenCharacter = 0;
    }

    public void OnBtn2Clicked()
    {
        chosenCharacter = 1;
    }

    /*
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        string text = string.Format("Plattform: {0} | Conn: {1}, | ID: {2}", Application.platform.ToString(), conn.connectionId.ToString(), playerControllerId.ToString());
        DebugScreen(text);

        GameObject playerToSpawn;
        if (Application.platform == RuntimePlatform.Android)
            playerToSpawn = m_PlayerAR;
        else
            playerToSpawn = m_PlayerDefault;

        GameObject player;
        player = Instantiate(playerToSpawn, Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
    */


    private void DebugScreen(string text)
    {
        m_DebugText.text = text;
    }
}
