using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Extend the list of names for new tools
public enum ToolNames { Selection, Alignment, MeshArea, Placement };

public abstract class AbstractTool : MonoBehaviour
{
    public abstract ToolNames m_ToolName { get; }

    private bool m_Active = false;
    private static AbstractTool s_ActiveTool;
    private static List<AbstractTool> s_Tools = new List<AbstractTool>();

    public static event EventHandler onActivToolChanged = delegate { };

    // Start is called before the first frame update
    protected void Start()
    {
        s_Tools.Add(this);
    }

    protected virtual void OnToolActivated()
    {
        // can be used for tool setup actions;
    }

    protected virtual void OnToolDeactivated()
    {
        // can be used for tool cleanup actions;
    }

    public static AbstractTool GetActiveTool()
    {
        return s_ActiveTool;
    }

    public static void SetActiveTool(ToolNames toolName)
    {
        foreach (AbstractTool tool in AbstractTool.s_Tools)
        {
            if(tool.m_ToolName == toolName)
            {
                AbstractTool oldTool = GetActiveTool();
                if (oldTool)
                    oldTool.SetToolActive(false);
                tool.SetToolActive(true);
                break;
            }
        }
    }

    public void SetToolActive(bool state)
    {
        
        m_Active = state;
        if(state)
        {
            s_ActiveTool = this;
            OnToolActivated();
            onActivToolChanged(this, EventArgs.Empty);
        }
        else
        {
            OnToolDeactivated();
        }
        
    }

    public bool IsActive()
    {
        return m_Active;
    }
  
}



