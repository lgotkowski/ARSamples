using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARSessionOrigin))]
public class TouchDetector : MonoBehaviour
{
    private ARSessionOrigin m_SessionOrigin;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public static event EventHandler<Touch> onTouch;
    public static event EventHandler<ARRaycastHit> onARTouch;
    public event Action<Touch> onTestAction;

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        onTouch += OnTouch;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (onTouch != null)
                    onTouch(this, touch);
            }
        }
    }

    void OnTouch(object sender, Touch touch)
    {
        if (m_SessionOrigin.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            if (onARTouch != null)
                onARTouch(this, s_Hits[0]);
        }
    }
}