using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "TimeEvents", menuName = "ScriptableObjects/TimeEvents")]
public class TimeEvents : ScriptableObject
{
    [HideInInspector]public event UnityAction<float> GoBackInTimeEvent;
    [HideInInspector]public event UnityAction<float> PreviewBackInTimeEvent;
    [HideInInspector]public event UnityAction SaveStateEvent;
    [HideInInspector]public event UnityAction SlowTimeEvent;
    [HideInInspector]public event UnityAction RestoreTimeEvent;
    [HideInInspector]public event UnityAction StopTimeEvent;
    [HideInInspector]public event UnityAction ContinueTimeEvent;
    public bool isTimeSlowed = false;

    private void OnDisable() {
        GoBackInTimeEvent = null;
        SaveStateEvent = null;
        PreviewBackInTimeEvent = null;
        SlowTimeEvent = null;
        RestoreTimeEvent = null;
        StopTimeEvent = null;
        ContinueTimeEvent = null;
    }
    public void RaiseSaveStateEvent()
    {
        if(SaveStateEvent != null)
            SaveStateEvent.Invoke();
    }
    public void RaiseSlowTimeEvent()
    {
        isTimeSlowed = true;
        if(SlowTimeEvent != null)
            SlowTimeEvent.Invoke();
    }
    public void RaiseRestoreTimeEvent()
    {
        isTimeSlowed = false;
        if(RestoreTimeEvent != null)
            RestoreTimeEvent.Invoke();
    }

    public void RaiseGoBackInTimeEvent(float time)
    {
        if(GoBackInTimeEvent != null)
            GoBackInTimeEvent.Invoke(time);
    }
    public void RaisePreviewBackInTimeEvent(float time)
    {
        if(PreviewBackInTimeEvent != null)
            PreviewBackInTimeEvent.Invoke(time);
    }
    public void RaiseStopTimeEvent()
    {
        if(StopTimeEvent != null)
            StopTimeEvent.Invoke();
    }
    public void RaiseContinueTimeEvent()
    {
        if(ContinueTimeEvent != null)
            ContinueTimeEvent.Invoke();
    }

}
