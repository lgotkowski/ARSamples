using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(ShelfModel))]
public class ShelfGui : MonoBehaviour
{
    public Color m_SelectionColor = new Color(0, 1, 0);
    private ColorBlock m_SelectedColorBlock;
    private ColorBlock m_UnSelectedColorBlock;

    private ShelfModel m_ShelfModel;
    private List<GameObject> m_ShelfGuiItems = new List<GameObject>();

    public Button m_AddShelfGuiItemButton;
    public Button m_RemoveShelfGuiItemButton;

    public GameObject m_ShelfGuiItemPrefab;
    public RectTransform m_ShelfGuiItemParent;

    public event EventHandler onAddShelfGuiItem = delegate { };
    public event EventHandler<int> onRemoveShelfGuiItem = delegate { };
    public event EventHandler<int> onShelfGuiItemClicked = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        if(!m_ShelfGuiItemPrefab.GetComponent<Button>())
            throw new Exception("Shelf Gui Item Prefab is missing a 'Button' component");

        m_AddShelfGuiItemButton.onClick.AddListener(OnAddShelfGuiItem);
        m_RemoveShelfGuiItemButton.onClick.AddListener(OnRemoveShelfGuiItem);

        m_UnSelectedColorBlock = m_ShelfGuiItemPrefab.GetComponent<Button>().colors;

        m_SelectedColorBlock = m_ShelfGuiItemPrefab.GetComponent<Button>().colors;
        m_SelectedColorBlock.normalColor = m_SelectionColor;
        m_SelectedColorBlock.highlightedColor = m_SelectionColor;
    }

    // Event Handler
    void OnAddShelfGuiItem()
    {
        AddShelfGuiItem();
        onAddShelfGuiItem(this, EventArgs.Empty);
        SelectShelfGuiItem(GetLastIndex());
    }

    void OnRemoveShelfGuiItem()
    {
        RemoveShelfGuiItem();
        onRemoveShelfGuiItem(this, m_ShelfModel.GetSelectedIndex());
        SelectShelfGuiItem(GetLastIndex());
    }

    void OnShelfGuiItemClicked()
    {
        Button clickedGuiItem = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int index = m_ShelfGuiItems.IndexOf(clickedGuiItem.gameObject);
        SelectShelfGuiItem(index);
    }

    // Public Methods
    public void SetModel(ShelfModel shelfModel)
    {
        if (m_ShelfModel)
            DisconnectModel(m_ShelfModel);

        m_ShelfModel = shelfModel;
        ConnectModel(m_ShelfModel);
    }

    public ShelfModel GetModel()
    {
        return m_ShelfModel;
    }

    private void ConnectModel(ShelfModel shelfModel)
    { 
        onAddShelfGuiItem += shelfModel.OnAddShelfItem;
        onRemoveShelfGuiItem += shelfModel.OnRemoveShelfItem;
        onShelfGuiItemClicked += shelfModel.OnSelectShelfItem;
    }

    private void DisconnectModel(ShelfModel shelfModel)
    {
        onAddShelfGuiItem -= shelfModel.OnAddShelfItem;
        onRemoveShelfGuiItem -= shelfModel.OnRemoveShelfItem;
        onShelfGuiItemClicked -= shelfModel.OnSelectShelfItem;
    }

    // Internal Methods
    void AddShelfGuiItem()
    {
        GameObject shelfGuiItem = Instantiate(m_ShelfGuiItemPrefab, m_ShelfGuiItemParent);
        shelfGuiItem.GetComponent<Button>().onClick.AddListener(OnShelfGuiItemClicked);
        m_ShelfGuiItems.Add(shelfGuiItem);
        UpdateItemParentWidth();
    }

    void RemoveShelfGuiItem()
    {
        if(m_ShelfGuiItems.Count > 0)
        {
            GameObject shelfGuiItem = m_ShelfGuiItems[GetLastIndex()];
            m_ShelfGuiItems.Remove(shelfGuiItem);
            Destroy(shelfGuiItem);
            UpdateItemParentWidth();
        }
    }

    void SelectShelfGuiItem(int index)
    {
        //SetSelectionColor(m_ShelfModel.GetSelectedIndex(), m_UnSelectedColorBlock);
        SetGuiItemActive(m_ShelfModel.GetSelectedIndex(), false);
        onShelfGuiItemClicked(this, index);
        //SetSelectionColor(index, m_SelectedColorBlock);
        SetGuiItemActive(index, true);
    }

    // Helper Methods
    int GetLastIndex()
    {
        return Utils.GetLastIndex(m_ShelfGuiItems);
    }

    void UpdateItemParentWidth()
    {
        float width = m_ShelfGuiItems.Count * m_ShelfGuiItemParent.rect.height;
        m_ShelfGuiItemParent.sizeDelta = new Vector2(width, m_ShelfGuiItemParent.rect.height);
    }

    void SetSelectionColor(int index, ColorBlock colorBlock)
    {
        if (m_ShelfGuiItems.Count > index)
        {
            m_ShelfGuiItems[index].GetComponent<Button>().colors = colorBlock;
            Debug.Log(string.Format("Assign Color to: {0}", index));
        }
    }

    void SetGuiItemActive(int index, bool state)
    {
        if (m_ShelfGuiItems.Count > index)
        {
            m_ShelfGuiItems[index].GetComponent<Outline>().enabled = state;
        }
        
    }
}
