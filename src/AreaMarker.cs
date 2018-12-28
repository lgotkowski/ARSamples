using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PositionListEventArgs : EventArgs
{
    public List<Vector3> m_List;

    public PositionListEventArgs(List<Vector3> positions)
    {
        m_List = positions;
    }
}

[RequireComponent(typeof(LineRenderer), typeof(MeshFilter))]
public class AreaMarker: MonoBehaviour
{
    private List<Vector3> m_SampledPoints = new List<Vector3>();
    private static float m_SampleInterval = 0.2f;
    private static Vector3 m_SampledPoint = Vector3.zero;
    private static bool m_SamplingPaused = false;
    private static bool m_SamplingFinished = true;

    private PositionListEventArgs m_PositionListEventArgs;
    public static event EventHandler<PositionListEventArgs> onFinishedSampling;
    public static event EventHandler onStartedSampling;

    void Start()
    {
        m_PositionListEventArgs = new PositionListEventArgs(m_SampledPoints);
    }

    public void StartPointSampling()
    {
        // TODO: hook up to gui element
        if (m_SamplingFinished)
        {
            m_SamplingFinished = false;
            m_SamplingPaused = false;
            if (onStartedSampling != null)
                onStartedSampling(this, EventArgs.Empty);
            StartCoroutine(SamplePoints());
        }
    }

    public void FinishPointSampling()
    {
        // TODO: hook up to gui element
        m_SamplingFinished = true;
    }

    public void TogglePausePointSampling()
    {
        // TODO: hook up to gui element
        m_SamplingPaused = !m_SamplingPaused;
    }

    public List<Vector3> GetSampledPoints()
    {
        return m_SampledPoints;
    }

    private Vector3 GetPointOnSurface()
    {
        // TODO Implement subscribe the touch input? drag or sth?
        return Vector3.zero;
    }

    private void AddSamplePoint()
    {
        m_SampledPoint = GetPointOnSurface();
        if (m_SampledPoint != Vector3.zero)
        {
            m_SampledPoints.Add(m_SampledPoint);
        }
    }

    private IEnumerator SamplePoints()
    {
        while(m_SamplingFinished == false)
        {
            if (m_SamplingPaused == false)
                AddSamplePoint();
            yield return new WaitForSeconds(m_SampleInterval);
        }
        if (onFinishedSampling != null)
            onFinishedSampling(this, m_PositionListEventArgs);
    }
}
