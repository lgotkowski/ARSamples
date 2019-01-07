using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class PlacementTool : AbstractTool
{
    public override ToolNames m_ToolName { get { return ToolNames.Placement; } }

    private GameObject m_PlaceableParent;
    public List<TouchTag> m_TouchTags = new List<TouchTag>() { TouchTag.ARPlane};

    public GameObject m_Prefab;
    public string m_folderName;
    public GameObject m_Instance;


    private TouchFilter m_TouchFilter;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        m_PlaceableParent = new GameObject("Placeables_Parent");
        m_PlaceableParent.transform.position = Vector3.zero;
        m_PlaceableParent.transform.rotation = Quaternion.identity;
        PlacementGui.onPlaceableItemSelected += OnPlaceableItemSelected;
    }

    void OnPlaceableItemSelected(object sender, PlaceEvent placeEvent)
    {
        m_folderName = placeEvent.m_folderName;
        m_Prefab = placeEvent.m_prefab;
    }

    void OnGroundTouched(object sender, PointerEventData eventData)
    {
        Vector3 position = eventData.pointerCurrentRaycast.worldPosition;
        Vector3 up = eventData.pointerCurrentRaycast.worldNormal;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, up);
        if (m_Prefab)
            InstantiateByRemotePlayer(m_folderName, m_Prefab, position, rotation);
    }

    void OnGrounDrag(object sender, PointerEventData eventData)
    {
        if (m_Instance)
        {
            Quaternion rotation = CalculateAimRotation(eventData);
            TransformByRemotePlayer(m_Instance, m_Instance.transform.position, rotation);
            //m_Instance.transform.rotation = rotation;
        }
    }

    void OnGroundDragEnd(object sender, PointerEventData eventData)
    {
        // Put the game object in the default layer (0)
        if(m_Instance && m_Prefab)
        {
            m_Instance.layer = 0;
        }
        m_Instance = null;
    }

    Quaternion CalculateAimRotation(PointerEventData eventData)
    {
        Vector3 position = eventData.pointerCurrentRaycast.worldPosition;
        Vector3 forward = position - m_Instance.transform.position;
        Vector3 up = eventData.pointerCurrentRaycast.worldNormal;
        Quaternion newRotation = Quaternion.LookRotation(forward, up);
        return newRotation;
    }

    /*
    void CreatePlaceable(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        //m_Instance = Instantiate(prefab, position, rotation, m_PlaceableParent.transform);
        m_Instance = Instantiate(prefab, position, rotation);
        // Put the game object in the ignore raycast layer (2)
        m_Instance.layer = 2;
        Debug.Log(string.Format("Instance layer: {0}", m_Instance.layer));
    }
    */

    void InstantiateByRemotePlayer(string folderName, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        RemotePlayer remotePlayer = GetRemotePlayer();
        remotePlayer.NetworkInstantiate(OnInstantiatedByRemotePlayer, folderName, prefab, position, rotation);
    }

    void TransformByRemotePlayer(GameObject instane, Vector3 position, Quaternion rotation)
    {
        RemotePlayer remotePlayer = GetRemotePlayer();
        remotePlayer.NetworkTransformInstance(OnTransformedByRemotePlayer, instane, position, rotation);
    }

    void OnInstantiatedByRemotePlayer(object sender, GameObject instance)
    {
        m_Instance = instance;
    }

    void OnTransformedByRemotePlayer(object sender, GameObject instance)
    {
        
    }

    protected override void OnToolActivated()
    {
        base.OnToolActivated();
        m_TouchFilter = new TouchFilter(tags: null, touchTags: m_TouchTags);
        m_TouchFilter.onTouched += OnGroundTouched;
        m_TouchFilter.onDrag += OnGrounDrag;
        m_TouchFilter.onEndDrag += OnGroundDragEnd;
    }

    protected override void OnToolDeactivated()
    {
        base.OnToolDeactivated();
        m_TouchFilter.onTouched -= OnGroundTouched;
        m_TouchFilter.onDrag -= OnGrounDrag;
        m_TouchFilter.onEndDrag -= OnGroundDragEnd;
        m_TouchFilter = null;
    }

    RemotePlayer GetRemotePlayer()
    {
        GameObject remotePlayerObject = OriginPlayer.GetRemotePlayer();
        return remotePlayerObject.GetComponent<RemotePlayer>();
    }
}
