using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkLobbyGui : MonoBehaviour
{
    public GameObject m_LobbyGui;
    public GameObject m_ToolbarGui;
    public InputField m_FieldIpAdress;
    public Button m_Host;
    public Button m_Connect;
    public NetworkManager m_NetworkManager;

    // Start is called before the first frame update
    void Start()
    {
        m_FieldIpAdress.onValueChanged.AddListener(OnIpAdressChanged);
        m_Host.onClick.AddListener(OnHost);
        m_Connect.onClick.AddListener(OnConnect);
        Show(true);
    }

    void OnIpAdressChanged(string text)
    {
        m_NetworkManager.networkAddress = text;
    }

    void OnHost()
    {
        m_NetworkManager.StartHost();
        Show(false);
    }

    void OnConnect()
    {
        m_NetworkManager.StartClient();
        Show(false);
    }

    void Show(bool state)
    {
        Debug.Log(string.Format("Show Lobby Gui: {0}", state));
        m_LobbyGui.SetActive(state);
        m_ToolbarGui.SetActive(!state);
    }
}
