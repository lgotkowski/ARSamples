using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlatformSetup : MonoBehaviour
{
    public enum PlattformEnum {AutoDetect, PC, AR, VR};
    public PlattformEnum m_Plattform;

    public GameObject m_SetupPCPrefab;
    public GameObject m_SetupARPrefab;
    public GameObject m_SetupVRPrefab;

    public Text m_DebugText;

    private GameObject m_WorldSetup;

    private void Start()
    {
        DetectPlatform();
    }

    void DetectPlatform()
    {
        if (m_Plattform == PlattformEnum.AutoDetect)
        {
            if(Application.platform == RuntimePlatform.Android)
            {
                m_Plattform = PlattformEnum.AR;
            }
            else
            {
                m_Plattform = PlattformEnum.PC;
            }
        }

        m_DebugText.text = m_Plattform.ToString();
        if (m_Plattform == PlattformEnum.AR)
        {
            SetupPlatform(m_SetupARPrefab);
            AdditionalARSetup();
        }
        else
        {
            SetupPlatform(m_SetupPCPrefab);
            AdditionalPCSetup();
        }
    }

    void SetupPlatform(GameObject setupPrefab)
    {
        //ClientScene.RegisterPrefab(setupPrefab);
        m_WorldSetup = Instantiate(setupPrefab, Vector3.zero, Quaternion.identity);
    }

    void AdditionalPCSetup()
    {
        
    }

    void AdditionalARSetup()
    {
        
    }

    void AdditionalVRSetup()
    {
        
    }
}
