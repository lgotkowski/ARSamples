using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class MeshAreaGui : MonoBehaviour
{
    // GUI References
    public Button m_BtnAddArea;

    public GameObject m_AreaButtonsGui;
    public RectTransform m_AreaButtonsParent;

    public Button m_BtnRemoveArea;
    public Button m_BtnAddPoint;
    public Button m_BtnRemovePoint;
    public Button m_BtnRemoveAllPoints;
    public Button m_BtnGenerateMesh;
    
    // GUI prefabs for creation
    public GameObject m_BtnAreaPrefab;

    [SerializeField]
    private int m_SelectedIndex;
    [SerializeField]
    private List<GameObject> m_AreaButtons = new List<GameObject>();

    public event EventHandler onAddAreaClicked;
    public event EventHandler onRemoveAreaClicked;
    public event EventHandler<int> onSelectAreaClicked;

    public event EventHandler onAddPointClicked;
    public event EventHandler onRemovePointClicked;
    public event EventHandler onRemoveAllPointsClicked;
    public event EventHandler onGenerateMeshClicked;



    // Start is called before the first frame update
    void Start()
    {
        m_BtnAddArea.onClick.AddListener(OnBtnAddAreaClicked);
        m_BtnRemoveArea.onClick.AddListener(OnBtnRemoveAreaClicked);

        m_BtnAddPoint.onClick.AddListener(OnBtnAddPointClicked);
        m_BtnRemovePoint.onClick.AddListener(OnBtnRemovePointClicked);
        //m_BtnRemoveAllPoints.onClick.AddListener(OnBtnRemoveAllPointsClicked);
        m_BtnGenerateMesh.onClick.AddListener(OnBtnGenerateMeshClicked);
    }

    // EventHandler
    void OnBtnAddAreaClicked()
    {
        onAddAreaClicked(this, EventArgs.Empty);
        GameObject areaButton = Instantiate(m_BtnAreaPrefab);
        areaButton.transform.SetParent(m_AreaButtonsParent);
        areaButton.GetComponent<Button>().onClick.AddListener(OnBtnSelectAreaClicked);
        m_AreaButtons.Add(areaButton);

        // Activate this area
        SelectAreaButton(areaButton);
    }

    void OnBtnRemoveAreaClicked()
    {
        if (m_AreaButtons.Count > m_SelectedIndex)
        {
            onRemoveAreaClicked(this, EventArgs.Empty);
            GameObject selectedButton = m_AreaButtons[m_SelectedIndex];
            m_AreaButtons.Remove(selectedButton);
            Destroy(selectedButton);
            // Select the last area
            SelectAreaButton(Utils.GetLastIndex(m_AreaButtons));
            if(m_AreaButtons.Count == 0)
            {
                ShowSelectedAreaGui(false);
            }
        }
    }

    void OnBtnSelectAreaClicked()
    {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        SelectAreaButton(clickedButton.gameObject);
    }

    void OnBtnAddPointClicked()
    {
        onAddPointClicked(this, EventArgs.Empty);
    }

    void OnBtnRemovePointClicked()
    {
        onRemovePointClicked(this, EventArgs.Empty);
        
    }

    void OnBtnRemoveAllPointsClicked()
    {
        onRemoveAllPointsClicked(this, EventArgs.Empty);
    }

    void OnBtnGenerateMeshClicked()
    {
        onGenerateMeshClicked(this, EventArgs.Empty);
        ShowSelectedAreaGui(false);
    }

    // Internal Methods
    void SelectAreaButton(GameObject button)
    {
        m_SelectedIndex = m_AreaButtons.IndexOf(button);
        onSelectAreaClicked(this, m_SelectedIndex);
        ShowSelectedAreaGui(true);
    }

    void SelectAreaButton(int index)
    {
        m_SelectedIndex = index;
        onSelectAreaClicked(this, m_SelectedIndex);
        ShowSelectedAreaGui(true);
    }

    void ShowSelectedAreaGui(bool state)
    {
        m_AreaButtonsGui.SetActive(state);
    }
}
