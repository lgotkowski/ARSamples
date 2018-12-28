using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NavMeshTouch : MonoBehaviour, IPointerDownHandler
{
    public static event EventHandler<PointerEventData> onTouchNavSurface;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(string.Format("Clicked: {0} | WPos: {1} | Pos: {2}", 
            eventData.pointerCurrentRaycast.gameObject.name,
            eventData.pointerCurrentRaycast.worldPosition, 
            eventData.position));

        if (onTouchNavSurface != null)
            onTouchNavSurface(this, eventData);
            //onTouchNavSurface(this, eventData.pointerCurrentRaycast.worldPosition);
    }
}
