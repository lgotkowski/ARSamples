using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

[System.Serializable]
public class ToolGuiItem
{
    public ToolNames m_ToolName;
    public AbstractToolGui m_ToolGui;
}

public class ToolBarGui : MonoBehaviour
{
    public GameObject m_GuiParent;
    public RectTransform m_ButtonParent;
    public Button m_ButtonPrefab;
    public GameObject m_Spacer;

    [SerializeField]
    public List<ToolGuiItem> m_ToolGuiList = new List<ToolGuiItem>();
    private List<Button> m_ToolGuiButtonList = new List<Button>();
    private Button m_SelectedButton;

    public Color m_SelectionColor = new Color(0, 1, 0);
    private ColorBlock m_SelectedColorBlock;
    private ColorBlock m_UnSelectedColorBlock;

    // Start is called before the first frame update
    void Start()
    {
        CreateGUi();
    }

    void CreateGUi()
    {
        foreach(ToolGuiItem toolItem in m_ToolGuiList)
        {
            Button btn = CreateButton(toolItem.m_ToolName.ToString());
            m_ToolGuiButtonList.Add(btn);
        }
    }

    Button CreateButton(string name)
    {
        Button button = Instantiate(m_ButtonPrefab, m_ButtonParent);
        button.onClick.AddListener(OnButtonClicked);
        Text label = button.GetComponentInChildren<Text>();
        label.text = name;
        return button;
    }

    void OnButtonClicked()
    {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        SetSelection(clickedButton);
        int index = m_ToolGuiButtonList.IndexOf(clickedButton);

        ToolGuiItem toolGuiItem = m_ToolGuiList[index];
        SetSpacer(toolGuiItem);
        SetActiveTool(toolGuiItem);
    }

    void SetActiveTool(ToolGuiItem toolGuiItem)
    {
        AbstractTool.SetActiveTool(toolGuiItem.m_ToolName);
    }

    void SetSpacer(ToolGuiItem toolGuiItem)
    {
        if(toolGuiItem.m_ToolGui)
            m_Spacer.SetActive(false);
        else
            m_Spacer.SetActive(true);
    }

    void SetSelection(Button btn)
    {
        if(m_SelectedButton)
        {
            m_SelectedButton.colors = m_UnSelectedColorBlock;
        }

        m_UnSelectedColorBlock = btn.colors;
        m_SelectedColorBlock = btn.colors;
        m_SelectedColorBlock.normalColor = m_SelectionColor;
        m_SelectedColorBlock.highlightedColor = m_SelectionColor;
        btn.colors = m_SelectedColorBlock;

        m_SelectedButton = btn;
    }
}
