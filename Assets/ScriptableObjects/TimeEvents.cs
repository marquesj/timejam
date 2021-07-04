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

    private void OnDisable() {
        GoBackInTimeEvent = null;
        SaveStateEvent = null;
        PreviewBackInTimeEvent = null;
    }
    public void RaiseSaveStateEvent()
    {
        if(SaveStateEvent != null)
            SaveStateEvent.Invoke();
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

}
