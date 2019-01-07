using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PlaceEvent : EventArgs
{
    public PlaceEvent(GameObject prefab, string folderName)
    {
        m_folderName = folderName;
        m_prefab = prefab;
    }
    public GameObject m_prefab;
    public string m_folderName;
}

public class PlacementGui : AbstractToolGui
{
    public override ToolNames m_ToolName { get { return ToolNames.Placement; } }

    public int m_ButtonWidth = 200;
    public int m_ButtonHeight = 200;
    public Button m_ButtonPrefab;
    public RectTransform m_ParentButtons;
    public string m_placeablesFolderName = "placeables";
    public List<GameObject> m_Placeables = new List<GameObject>();
    private List<Button> m_Buttons = new List<Button>();
    private int m_SelectedIndex = -1;

    public Color m_SelectionColor = new Color(0, 1, 0);
    private ColorBlock m_SelectedColorBlock;
    private ColorBlock m_UnSelectedColorBlock;

    public static event EventHandler<PlaceEvent> onPlaceableItemSelected = delegate { };

    // Start is called before the first frame update
    [ExecuteInEditMode]
    void Start()
    {
        base.Start();
        CreateGui();
    }

    void CreateGui()
    {
        foreach (GameObject placeable in m_Placeables)
        {
            m_Buttons.Add(CreateButton(placeable.name));
        }
        UpdateSize();
    }

    Button CreateButton(string name)
    {
        Button button = Instantiate(m_ButtonPrefab, m_ParentButtons);
        button.onClick.AddListener(OnButtonClicked);
        Text label = button.GetComponentInChildren<Text>();
        label.text = name;
        return button;
    }

    void UpdateSize()
    {
        float width = m_Placeables.Count * m_ButtonWidth;//* m_ParentButtons.rect.height;
        m_ParentButtons.sizeDelta = new Vector2(width, m_ButtonHeight);
    }

    void OnButtonClicked()
    {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int index = m_Buttons.IndexOf(clickedButton);
        SetSelection(index);
        PlaceEvent placeEvent = new PlaceEvent(m_Placeables[index], m_placeablesFolderName);
        onPlaceableItemSelected(this, placeEvent);
    }

    void SetSelection(int index)
    {
        if (m_SelectedIndex != -1)
        {
            m_Buttons[m_SelectedIndex].colors = m_UnSelectedColorBlock;
        }
        m_SelectedIndex = index;

        Button btn = m_Buttons[m_SelectedIndex];

        m_UnSelectedColorBlock = btn.colors;
        m_SelectedColorBlock = btn.colors;

        m_SelectedColorBlock.normalColor = m_SelectionColor;
        m_SelectedColorBlock.highlightedColor = m_SelectionColor;
        btn.colors = m_SelectedColorBlock;
    }

    public GameObject GetSelectedPlaceable()
    {
        return m_Placeables[m_SelectedIndex];
    }
}
