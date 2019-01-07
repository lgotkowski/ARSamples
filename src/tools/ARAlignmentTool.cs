using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using System;

class ARAlignmentTool : AbstractTool
{
    public override ToolNames m_ToolName { get { return ToolNames.Alignment; } }

    public List<TouchTag> m_TouchTags = new List<TouchTag>() { TouchTag.ARPlane };
    public ARSessionOrigin m_ARSessionOrigin;
    public GameObject m_OriginInstance;
    public GameObject m_OriginPrefab;

    private TouchFilter m_TouchFilter;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        GetARSessionOrigin();
        GetOrCreateWorldOrigin();
    }

    void GetARSessionOrigin()
    {
        if(!m_ARSessionOrigin)
        {
            GameObject ar_GameObject = GameObject.FindGameObjectWithTag("ARSessionOrigin");
            if(ar_GameObject)
                m_ARSessionOrigin = ar_GameObject.GetComponent<ARSessionOrigin>();
            else
                throw new Exception("Missing 'ARSessionOrigin' in Alignment Tool");
        }
    }

    void GetOrCreateWorldOrigin()
    {
        m_OriginInstance = GameObject.FindGameObjectWithTag("WorldOrigin");
        if(!m_OriginInstance)
        {
            m_OriginInstance = Instantiate(m_OriginPrefab, Vector3.zero, Quaternion.identity);
        }
        m_OriginInstance.SetActive(false);
    }

    void OnARBeginDrag(object sender, PointerEventData eventData)
    {
        Vector3 position = eventData.pointerCurrentRaycast.worldPosition;
        SetWorldPosition(position);
        ApplyNewOrigin();
    }

    void OnARDrag(object sender, PointerEventData eventData)
    {
        Quaternion rotation = CalculateAimRotation(eventData);
        SetWorldRotation(rotation);
    }

    void OnAREndDrag(object sender, PointerEventData eventData)
    {
        Quaternion rotation = CalculateAimRotation(eventData);
        SetWorldRotation(rotation);
        ApplyNewOrigin();
    }

    Quaternion CalculateAimRotation(PointerEventData eventData)
    {
        Vector3 position = eventData.pointerCurrentRaycast.worldPosition;
        Vector3 forward = position - m_OriginInstance.transform.position;
        Vector3 up = eventData.pointerCurrentRaycast.worldNormal;
        Quaternion newRotation = Quaternion.LookRotation(forward, up);
        return newRotation;
    }

    private void SetWorldPosition(Vector3 newOriginPosition)
    {
        //m_ARSessionOrigin.MakeContentAppearAt(m_OriginInstance.transform, newOriginPosition, m_OriginInstance.rotation);
        m_OriginInstance.transform.position = newOriginPosition;
    }

    private void SetWorldRotation(Quaternion newOriginRotation)
    {
        //m_ARSessionOrigin.MakeContentAppearAt(m_OriginInstance.transform, m_OriginInstance.position, newOriginRotation);
        m_OriginInstance.transform.rotation = newOriginRotation;
    }

    private void ApplyNewOrigin()
    {
        Vector3 tmpPos = m_OriginInstance.transform.position;
        Quaternion tmpRot = m_OriginInstance.transform.rotation;

        m_OriginInstance.transform.position = Vector3.zero;
        m_OriginInstance.transform.rotation = Quaternion.identity;

        m_ARSessionOrigin.MakeContentAppearAt(m_OriginInstance.transform, tmpPos, tmpRot);
    }

    protected override void OnToolActivated()
    {
        base.OnToolActivated();
        m_TouchFilter = new TouchFilter(tags: null, touchTags: m_TouchTags);
        m_TouchFilter.onBeginDrag += OnARBeginDrag;
        m_TouchFilter.onDrag += OnARDrag;
        m_TouchFilter.onEndDrag += OnAREndDrag;
        m_OriginInstance.SetActive(true);
    }

    protected override void OnToolDeactivated()
    {
        base.OnToolDeactivated();
        m_TouchFilter.onBeginDrag -= OnARBeginDrag;
        m_TouchFilter.onDrag -= OnARDrag;
        m_TouchFilter.onEndDrag -= OnAREndDrag;
        m_TouchFilter = null;
        m_OriginInstance.SetActive(false);
    }
}
