using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractToolGui : MonoBehaviour
{
    public abstract ToolNames m_ToolName { get; }
    public GameObject m_GuiParent;

    // Start is called before the first frame update
    protected void Start()
    {
        AbstractTool.onActivToolChanged += OnActiveToolChanged;
    }
    
    void OnActiveToolChanged(object sender, object eventData)
    {
        AbstractTool activeTool = sender as AbstractTool;
        if (activeTool.m_ToolName == m_ToolName)
            ActiveteToolGui(true);
        else
            ActiveteToolGui(false);
    }

    void ActiveteToolGui(bool state)
    {
        m_GuiParent.SetActive(state);
    }
}
